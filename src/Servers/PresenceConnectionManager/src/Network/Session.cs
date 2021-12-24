﻿using UniSpyServer.Servers.PresenceConnectionManager.Entity.Enumerate;
using UniSpyServer.Servers.PresenceConnectionManager.Handler.CommandSwitcher;
using UniSpyServer.Servers.PresenceConnectionManager.Structure;
using UniSpyServer.Servers.PresenceConnectionManager.Structure.Data;
using UniSpyServer.UniSpyLib.Logging;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server;
namespace UniSpyServer.Servers.PresenceConnectionManager.Network
{
    /// <summary>
    /// Gamespy Client Manager
    /// This class is used to proccess the client login process,
    /// create new user accounts, and fetch profile information
    /// <remarks>gpcm.gamespy.com</remarks>
    /// </summary>
    public sealed class Session : UniSpyTcpSession
    {
        /// <summary>
        /// Indicates the date and time this connection was created
        /// </summary>
        public UserInfo UserInfo;
        public Session(UniSpyTcpServer server) : base(server)
        {
            UserInfo = new UserInfo(Id);
        }

        protected override void OnConnected()
        {
            SendServerChallenge();
            base.OnConnected();
        }
        protected override void OnReceived(string message) => new CmdSwitcher(this, message).Switch();
        public void SendServerChallenge()
        {
            // Only send the login challenge once
            if (UserInfo.BasicInfo.LoginStatus != LoginStatus.Connected)
            {
                Disconnect();
                // Throw the error                
                LogWriter.Warning("The server challenge has already been sent. Cannot send another login challenge.");
            }

            UserInfo.BasicInfo.LoginStatus = LoginStatus.Processing;
            string sendingBuffer = $@"\lc\1\challenge\{LoginChallengeProof.ServerChallenge}\id\{1}\final\";
            SendAsync(sendingBuffer);
        }
    }
}