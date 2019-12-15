﻿using GameSpyLib.Common;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Handler.General.SDKExtendFeature;
using PresenceConnectionManager.Handler.HandlerBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Handler.General.Login
{
    public class LoginHandler : LoginHandlerBase
    {
        public LoginHandler(GPCMSession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }

        /// <summary>
        /// This method verifies the login information sent by
        /// the session, and returns encrypted data for the session
        /// to verify as well
        /// </summary>
        public override void Handle(GPCMSession session)
        {          

            CheckRequest(session);

            if (_errorCode != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(session, _errorCode, _errorMsg);
                session.DisconnectByReason(session.PlayerInfo.DisconReason);
                return;
            }

            if (!PreProcessForLogin(session))
            {
                GameSpyUtils.SendGPError(session, GPErrorCode.Parse, "There was an error parsing an incoming request.");
                session.DisconnectByReason(DisconnectReason.GeneralError);
                return;
            }

            switch (session.PlayerInfo.LoginMethod)
            {
                case LoginMethods.UniqueNickname:
                    UniquenickLoginMethod(session);
                    break;
                case LoginMethods.AuthToken:
                    AuthTokenLoginMethod(session);
                    break;
                case LoginMethods.Username:
                    NoUniquenickLoginMethod(session);
                    break;
                default:
                    session.ToLog("Invalid login method!!");
                    GameSpyUtils.SendGPError(session, GPErrorCode.General, "There was an unknown error.");
                    session.DisconnectByReason(DisconnectReason.GeneralError);
                    return;
            }

            //if no match found we disconnect the session
            CheckDatabaseResult(session);
            if (_errorCode != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(session, _errorCode, _errorMsg);
                session.DisconnectByReason(session.PlayerInfo.DisconReason);
                return;
            }

            // Check if user is banned
            CheckUsersAccountAvailability(session);
            if (_errorCode != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(session, _errorCode, _errorMsg);
                session.DisconnectByReason(session.PlayerInfo.DisconReason);
                return;
            }

            SendLoginResponseChallenge(session);
            if (_errorCode != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(session, _errorCode, _errorMsg);
                session.DisconnectByReason(session.PlayerInfo.DisconReason);
                return;
            }

            SDKRevision.Switch(session, _recv);
        }

        protected override void CheckRequest(GPCMSession session)
        {
            // Make sure we have all the required data to process this login
            if (!_recv.ContainsKey("challenge") || !_recv.ContainsKey("response"))
            {
                _errorCode = GPErrorCode.Parse;
                _errorMsg = "Parsing error";
                session.PlayerInfo.DisconReason = DisconnectReason.InvalidLoginQuery;
            }
            else
            {
                _errorCode = GPErrorCode.NoError;
            }
        }
        /// <summary>
        /// We make a pre process for the data we received
        /// </summary>
        public bool PreProcessForLogin(GPCMSession session)
        {
            session.PlayerInfo.UserChallenge = _recv["challenge"];

            if (_recv.ContainsKey("uniquenick"))
            {
                session.PlayerInfo.UniqueNick = _recv["uniquenick"];
                session.PlayerInfo.UserData = session.PlayerInfo.UniqueNick;
                session.PlayerInfo.LoginMethod = LoginMethods.UniqueNickname;
            }
            else if (_recv.ContainsKey("authtoken"))
            {
                session.PlayerInfo.AuthToken = _recv["authtoken"].ToString();
                session.PlayerInfo.UserData = session.PlayerInfo.AuthToken;
                session.PlayerInfo.LoginMethod = LoginMethods.AuthToken;
            }
            else
            {
                // "User" is <nickname>@<email>, it will be splitted in this function

                string user = _recv["user"].ToString();

                int Pos = user.IndexOf('@');
                if (Pos == -1 || Pos < 1 || (Pos + 1) >= user.Length)
                {
                    // Ignore malformed user
                    // Pos == -1 : Not found
                    // Pos < 1 : @ or @example
                    // Pos + 1 >= Length : example@
                    return false;
                }

                string nick = user.Substring(0, Pos);
                string email = user.Substring(Pos + 1);

                session.PlayerInfo.Nick = nick;
                session.PlayerInfo.Email = email;
                session.PlayerInfo.UserData = user;
                session.PlayerInfo.LoginMethod = LoginMethods.Username;
            }

            if (_recv.ContainsKey("partnerid"))
            {
                session.PlayerInfo.Partnerid = Convert.ToUInt32(_recv["partnerid"]);
            }
            else
            {
                session.PlayerInfo.Partnerid = (uint)PartnerID.Gamespy; // Default partnerid: Gamespy
            }

            if (_recv.ContainsKey("namespaceid"))
            {
                session.PlayerInfo.Namespaceid = Convert.ToUInt32(_recv["namespaceid"]);
            }
            else
            {
                session.PlayerInfo.Namespaceid = 0; // Default namespaceid
            }

            //store sdkrevision
            if (_recv.ContainsKey("sdkrevision"))
            {
                session.PlayerInfo.SDKRevision = Convert.ToUInt32(_recv["sdkrevision"]);
            }

            return true;
        }

        #region Login Methods
        private static void AuthTokenLoginMethod(GPCMSession session)
        {
            GameSpyUtils.SendGPError(session, GPErrorCode.General, "Not implemeted");
            session.DisconnectByReason(DisconnectReason.ForcedLogout);
        }

        private void NoUniquenickLoginMethod(GPCMSession session)
        {
            _result = LoginQuery.GetUserFromNickAndEmail(session.PlayerInfo.Namespaceid, session.PlayerInfo.Nick, session.PlayerInfo.Email);
        }

        private void UniquenickLoginMethod(GPCMSession session)
        {
            _result = LoginQuery.GetUserFromUniqueNick(session.PlayerInfo.UniqueNick, session.PlayerInfo.Namespaceid);
        }
        #endregion

        private void CheckDatabaseResult(GPCMSession session)
        {
            if (_result == null)
            {
                switch (session.PlayerInfo.LoginMethod)
                {
                    case LoginMethods.UniqueNickname:
                        _errorCode = GPErrorCode.LoginBadUniquenick;
                        _errorMsg = "The uniquenick provided is incorrect!";
                        session.PlayerInfo.DisconReason = DisconnectReason.InvalidUsername;
                        break;

                    case LoginMethods.Username:
                        _errorCode = GPErrorCode.LoginBadNick;
                        _errorMsg = "The information provided is incorrect!";
                        session.PlayerInfo.DisconReason = DisconnectReason.InvalidUsername;
                        break;

                    case LoginMethods.AuthToken:
                        _errorCode = GPErrorCode.AuthAddBadForm;
                        _errorMsg = "The information provided is incorrect!";
                        session.PlayerInfo.DisconReason = DisconnectReason.InvalidLoginQuery;
                        break;
                }
            }
            else
            {
                //parse profileid to playerinfo
                session.PlayerInfo.Profileid = Convert.ToUInt32(_result["profileid"]);
            }
        }

        private void CheckUsersAccountAvailability(GPCMSession session)
        {
            bool isVerified = Convert.ToBoolean(_result["emailverified"]);
            bool isBanned = Convert.ToBoolean(_result["banned"]);
            if (!isVerified)
            {
                _errorMsg = "Your account is not verified. Please check your email inbox and verify the account.";
                session.PlayerInfo.DisconReason = DisconnectReason.InvalidPlayer;
                _errorCode = GPErrorCode.LoginBadProfile;
            }

            // Check the status of the account.
            // If the single profile is banned, the account or the player status
            if (isBanned)
            {
                _errorMsg = "Your profile has been permanently suspended.";
                session.PlayerInfo.DisconReason = DisconnectReason.PlayerIsBanned;
                _errorCode = GPErrorCode.LoginProfileDeleted;
            }
        }

        public void SendLoginResponseChallenge(GPCMSession session)
        {
            try
            {
                // Use the GenerateProof method to compare with the "response" value. This validates the given password
                string response = GenerateProof(session, session.PlayerInfo.UserChallenge, session.PlayerInfo.ServerChallenge, _result["password"].ToString());
                if (_recv["response"] == response)
                {
                    // Create session key
                    session.PlayerInfo.SessionKey = _crc.ComputeChecksum(_result["uniquenick"] + _recv["namespaceid"]);

                    //actually we should store sesskey in database at namespace table, when we want someone's profile we just 
                    //access to the sesskey to find the uniquenick for particular game
                    if (!LoginQuery.UpdateSessionKey(session.PlayerInfo.Profileid, session.PlayerInfo.Namespaceid, session.PlayerInfo.SessionKey, session.Id))
                    {
                        _errorCode = GPErrorCode.DatabaseError;
                        _errorMsg = "This request cannot be processed because of a database error.";
                        session.PlayerInfo.DisconReason = DisconnectReason.GeneralError;
                        return;
                    }

                    string responseProof = GenerateProof(session, session.PlayerInfo.ServerChallenge, session.PlayerInfo.UserChallenge, _result["password"].ToString());

                    string random = GameSpyRandom.GenerateRandomString(22, GameSpyRandom.StringType.Hex);
                    // Password is correct
                    _sendingBuffer = string.Format(
                        @"\lc\2\sesskey\{0}\proof\{1}\userid\{2}\profileid\{2}\uniquenick\{3}\lt\{4}__\id\1\final\",
                        session.PlayerInfo.SessionKey,
                        responseProof,
                        _result["profileid"],
                        _result["uniquenick"],
                        // Generate LT whatever that is (some sort of random string, 22 chars long)
                        random
                        );

                    session.PlayerInfo.LoginProcess = LoginStatus.Completed;
                    session.SendAsync(_sendingBuffer);
                }
                else
                {
                    _errorCode = GPErrorCode.LoginBadPassword;
                    _errorMsg = "Password is not correct";
                    session.PlayerInfo.DisconReason = DisconnectReason.InvalidPassword;
                }
            }

            catch (Exception ex)
            {
                GameSpyUtils.SendGPError(session, GPErrorCode.General, "There was an unknown error.");
                LogWriter.Log.Write(ex.ToString(), LogLevel.Error);
                session.DisconnectByReason(DisconnectReason.GeneralError);
                return;
            }
        }

        /// <summary>
        /// Generates an MD5 hash, which is used to verify the sessions login information
        /// </summary>
        /// <param name="challenge1">First challenge key</param>
        /// <param name="challenge2">Second challenge key</param>
        /// <returns>
        ///     The proof verification MD5 hash string that can be compared to what the session sends,
        ///     to verify that the users entered password matches the specific user data in the database.
        /// </returns>
        private static string GenerateProof(GPCMSession session, string challenge1, string challenge2, string passwordHash)
        {
            string realUserData = session.PlayerInfo.UserData;

            // Auth token does not have partnerid append.
            if (session.PlayerInfo.Partnerid != (uint)PartnerID.Gamespy && session.PlayerInfo.LoginMethod != LoginMethods.AuthToken)
            {
                realUserData = string.Format("{0}@{1}", session.PlayerInfo.Partnerid, session.PlayerInfo.UserData);
            }

            // Generate our string to be hashed
            StringBuilder HashString = new StringBuilder(passwordHash);
            HashString.Append(' ', 48); // 48 spaces
            HashString.Append(realUserData);
            HashString.Append(challenge1);
            HashString.Append(challenge2);
            HashString.Append(passwordHash);
            return HashString.ToString().GetMD5Hash();
        }
    }
}
