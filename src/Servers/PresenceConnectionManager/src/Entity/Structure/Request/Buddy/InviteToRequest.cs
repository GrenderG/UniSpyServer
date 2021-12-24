﻿using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request.Buddy
{
    /// <summary>
    /// Invite a user to a room or a game
    /// </summary>
    [RequestContract("inviteto")]
    public sealed class InviteToRequest : RequestBase
    {
        public uint ProductID { get; private set; }
        public uint ProfileID { get; private set; }
        public InviteToRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();

            if (!RequestKeyValues.ContainsKey("productid"))
            {
                throw new GPParseException("productid is missing.");
            }

            if (!RequestKeyValues.ContainsKey("sesskey"))
            {
                throw new GPParseException("sesskey is missing.");

            }

            uint productID;
            if (!uint.TryParse(RequestKeyValues["productid"], out productID))
            {
                throw new GPParseException("productid format is incorrect.");
            }

            ProductID = productID;

            uint profileID;
            if (!uint.TryParse(RequestKeyValues["profileid"], out profileID))
            {
                throw new GPParseException("profileid format is incorrect.");
            }
            ProfileID = profileID;
        }
    }
}