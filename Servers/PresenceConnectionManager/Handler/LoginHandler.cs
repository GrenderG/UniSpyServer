﻿using GameSpyLib.Common;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using PresenceConnectionManager.DatabaseQuery;
using PresenceConnectionManager.Enumerator;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using PresenceConnectionManager.Handler.LoginMethod;
using PresenceConnectionManager.Structure;

namespace PresenceConnectionManager.Handler
{
    public class LoginHandler
    {
        /// <summary>
        /// Our CRC16 object for generating Checksums
        /// </summary>
        protected static Crc16 Crc = new Crc16(Crc16Mode.Standard);
        private static uint _partnerid;
        public static GPCMSession Session { get; protected set; }



        public static Dictionary<string, string> Recv { get; protected set; }
        public static Dictionary<string, object> QueryResult;
        public static string SendingBuffer { get; protected set; }
        /// <summary>
        /// This method verifies the login information sent by
        /// the session, and returns encrypted data for the session
        /// to verify as well
        /// </summary>
        public static void ProcessLogin(GPCMSession session, Dictionary<string, string> recv)
        {
            Session = session;
            Recv = recv;

            if (IsContainAllKeys() != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(session, GPErrorCode.General, "Invalid response received from the session!");
                session.DisconnectByReason(DisconnectReason.InvalidLoginQuery);
                return;
            }

            if (recv.ContainsKey("uniquenick"))
            {
                UniquenickLogin.Login();
            }
            else if (recv.ContainsKey("authtoken"))
            {
                AuthTokenLogin.Login();
            }
            else if (recv.ContainsKey("user"))
            {
                NoUniquenickLogin.Login();
            }
            else
            {
                session.ToLog("Invalid login method!!");
                session.DisconnectByReason(DisconnectReason.GeneralError);
            }




            // Parse the partnerid, required since it changes the challenge for Unique nick and User login


            //if no match found we disconnect the game
            if (QueryResult == null)
            {
                if (recv.ContainsKey("uniquenick"))
                {
                    GameSpyUtils.SendGPError(session, GPErrorCode.LoginBadUniquenick, "The uniquenick provided is incorrect!");
                }
                else
                {
                    GameSpyUtils.SendGPError(session, GPErrorCode.LoginBadUniquenick, "The information provided is incorrect!");
                }
                session.DisconnectByReason(DisconnectReason.InvalidUsername);
                return;
            }

            // Check if user is banned
            string msg;
            DisconnectReason reason;
            GPErrorCode error = CheckUsersAccountAvailability(QueryResult, out msg, out reason);
            if (error != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(session, error, msg);
                session.DisconnectByReason(reason);
                return;
            }
        }

        public void SDKRevisionSupport()
        {
            switch (Convert.ToInt32(LoginHandler.Recv["sdkrevision"]))
            {
                case GameSpySDKRevision.Type1:

                    break;
                case GameSpySDKRevision.Type2:

                    break;
                case GameSpySDKRevision.Type3:

                    break;
                case GameSpySDKRevision.Type4:

                    break;
            }
        }

        public static void ProcessNickAndEmail()
        {
            if (Recv.ContainsKey("user"))
            {
                // "User" is <nickname>@<email>
                string user = Recv["user"];
                int Pos = user.IndexOf('@');
                //we add the nick and email to dictionary
                string nick = user.Substring(0, Pos);
                string email = user.Substring(Pos + 1);
                Recv.Add("nick", nick);
                Recv.Add("email", email);
            }
            Session.PlayerInfo.UserChallenge = Recv["challenge"];

            if (Recv.ContainsKey("uniquenick"))
            {
                Session.PlayerInfo.UniqueNick = Recv["uniquenick"];
                Session.PlayerInfo.UserData = Session.PlayerInfo.UniqueNick;
            }
            else if (Recv.ContainsKey("authtoken"))
            {

                Session.PlayerInfo.AuthToken = Recv["authtoken"].ToString();
                Session.PlayerInfo.UserData = Session.PlayerInfo.AuthToken;

            }
            else
            {
                Session.PlayerInfo.User = Recv["user"].ToString();
                Session.PlayerInfo.UserData = Session.PlayerInfo.User;
            }
            if (Recv.ContainsKey("namespaceid"))
            {
                Session.PlayerInfo.namespaceid = Convert.ToUInt32(Recv["namespaceid"]);
            }
            else
            {
                Session.PlayerInfo.namespaceid = 0;
            }
        }

        private static GPErrorCode IsContainAllKeys()
        {
            if (Recv.ContainsKey("partnerid"))
            {
                _partnerid = Convert.ToUInt32(Recv["partnerid"]);
            }
            // Make sure we have all the required data to process this login
            if (!Recv.ContainsKey("challenge") || !Recv.ContainsKey("response"))
            {
                return GPErrorCode.Parse;
            }
            else
            {
                return GPErrorCode.NoError;
            }
        }


        public static void SendLoginResponseChallenge()
        {
            try
            {
                // Use the GenerateProof method to compare with the "response" value. This validates the given password

                // if (Recv["response"] == GenerateProof(Recv["challenge"], Session.ServerChallengeKey, Recv["user"], Recv["authtoken"]?.Length > 0 ? 0 : _partnerid, _originalPassword))
                string response = GenerateProof(Session.PlayerInfo.UserChallenge, Session.PlayerInfo.ServerChallenge, Session.PlayerInfo.UserData, Session.PlayerInfo.partnerid > 0 ? 0 : Session.PlayerInfo.partnerid, QueryResult["password"].ToString());
                if (Recv["response"] == response)
                {
                    // Create session key
                    Session.PlayerInfo.SessionKey = Crc.ComputeChecksum(QueryResult["uniquenick"].ToString());

                    //actually we should store sesskey in database at namespace table, when we want someone's profile we just 
                    //access to the sesskey to find the uniquenick for particular game
                    LoginQuery.UpdateSessionKey(QueryResult, Convert.ToInt32(Recv["namespaceid"]), Session.PlayerInfo.SessionKey, Session.Id);

                    string responseProof = GenerateProof(Session.PlayerInfo.ServerChallenge, Session.PlayerInfo.UserChallenge, Session.PlayerInfo.UserData, 0, QueryResult["password"].ToString().ToLowerInvariant());
                    // Password is correct
                    SendingBuffer = string.Format(@"\lc\2\sesskey\{0}\proof\{1}\userid\{2}\profileid\{2}\uniquenick\{3}\lt\{4}__\id\1\final\",
                        Session.PlayerInfo.SessionKey,
                        //GenerateProof(Session.ServerChallengeKey, Recv["challenge"], Recv["user"], Recv["authtoken"]?.Length > 0 ? 0 : _partnerid, _originalPassword), // Do this again, Params are reversed!
                        responseProof,
                        QueryResult["profileid"],
                        QueryResult["uniquenick"],
                        // Generate LT whatever that is (some sort of random string, 22 chars long)
                        GameSpyLib.Common.Random.GenerateRandomString(22, GameSpyLib.Common.Random.StringType.Hex)
                        );
                    Session.SendAsync(SendingBuffer);
                }
                else
                {
                    // Log Incoming Connection
                    string statusString = string.Format(@"[Login Failed!] Nick:{0} - Profileid:{1} - IP:{2}", Session.PlayerInfo.User, QueryResult["profileid"], Session.Socket.RemoteEndPoint);
                    Session.ToLog(LogLevel.Info, statusString);
                    // Password is incorrect with database value.
                    GameSpyUtils.SendGPError(Session, GPErrorCode.LoginBadPassword, "The password provided is incorrect");
                    Session.DisconnectByReason(DisconnectReason.InvalidPassword);
                }
            }

            catch (Exception ex)
            {
                LogWriter.Log.Write(ex.ToString(), LogLevel.Error);
                Session.DisconnectByReason(DisconnectReason.GeneralError);
                return;
            }
        }
        private static GPErrorCode CheckUsersAccountAvailability(Dictionary<string, object> queryResult, out string msg, out DisconnectReason reason)
        {
            PlayerOnlineStatus currentPlayerStatus;
            UserStatus currentUserStatus;

            if (!Enum.TryParse(queryResult["statuscode"].ToString(), out currentPlayerStatus))
            {
                msg = "Invalid player data! Please contact an administrator.";
                reason = DisconnectReason.InvalidPlayer;
                return GPErrorCode.LoginBadUniquenick;
                //GameSpyUtils.SendGPError(session, GPErrorCode.LoginBadUniquenick, "Invalid player data! Please contact an administrator.");
                //session.DisconnectByReason(DisconnectReason.InvalidPlayer);
            }
            if (!Enum.TryParse(queryResult["userstatus"].ToString(), out currentUserStatus))
            {
                msg = "Invalid player data! Please contact an administrator.";
                reason = DisconnectReason.InvalidPlayer;
                return GPErrorCode.LoginBadUniquenick;
                //GameSpyUtils.SendGPError(session, GPErrorCode.LoginBadUniquenick, "Invalid player data! Please contact an administrator.");
                //session.DisconnectByReason(DisconnectReason.InvalidPlayer);
                //return;
            }
            // Check the status of the account.
            // If the single profile is banned, the account or the player status
            if (currentPlayerStatus == PlayerOnlineStatus.Banned)
            {
                msg = "Your profile has been permanently suspended.";
                reason = DisconnectReason.PlayerIsBanned;
                return GPErrorCode.LoginBadUniquenick;
                //GameSpyUtils.SendGPError(session, GPErrorCode.LoginBadUniquenick, "Your profile has been permanently suspended.");
                //session.DisconnectByReason(DisconnectReason.PlayerIsBanned);
                //return;
            }
            if (currentUserStatus == UserStatus.Created)
            {
                msg = "Your account is not verified. Please check your email inbox and verify the account.";
                reason = DisconnectReason.PlayerIsBanned;
                return GPErrorCode.LoginBadUniquenick;

                //GameSpyUtils.SendGPError(session, GPErrorCode.LoginBadUniquenick, "Your account is not verified. Please check your email inbox and verify the account.");
                //session.DisconnectByReason(DisconnectReason.PlayerIsBanned);
                //return;
            }
            if (currentUserStatus == UserStatus.Banned)
            {
                msg = "Your account has been permanently suspended.";
                reason = DisconnectReason.PlayerIsBanned;
                return GPErrorCode.LoginBadUniquenick;
                //GameSpyUtils.SendGPError(session, GPErrorCode.LoginBadUniquenick, "Your account has been permanently suspended.");
                //session.DisconnectByReason(DisconnectReason.PlayerIsBanned);
                //return;
            }

            msg = "No error";
            reason = DisconnectReason.NormalLogout;
            return GPErrorCode.NoError;
        }


        /// <summary>
        /// Generates an MD5 hash, which is used to verify the sessions login information
        /// </summary>
        /// <param name="challenge1">First challenge key</param>
        /// <param name="challenge2">Second challenge key</param>
        /// <param name="userdata">The user data to append to the proof</param>
        /// <param name="partnerid">The partnerid to append</param>
        /// <returns>
        ///     The proof verification MD5 hash string that can be compared to what the session sends,
        ///     to verify that the users entered password matches the specific user data in the database.
        /// </returns>
        private static string GenerateProof(string userChallenge, string serverChallenge, string userdata, uint partnerid, string passwordHash)
        {
            string realUserData = userdata;

            if (partnerid != (uint)PartnerID.Gamespy)
            {
                realUserData = string.Format("{0}@{1}", partnerid, userdata);
            }
            // Generate our string to be hashed
            StringBuilder HashString = new StringBuilder(passwordHash);
            HashString.Append(' ', 48); // 48 spaces
            HashString.Append(realUserData);
            HashString.Append(userChallenge);
            HashString.Append(serverChallenge);
            HashString.Append(passwordHash);
            return HashString.ToString().GetMD5Hash();
        }



    }

}
