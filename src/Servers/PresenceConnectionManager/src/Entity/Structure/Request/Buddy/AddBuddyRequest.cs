﻿using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request.Buddy
{
    [RequestContract("addbuddy")]
    public sealed class AddBuddyRequest : RequestBase
    {
        public uint FriendProfileID { get; private set; }
        public string Reason { get; private set; }
        public AddBuddyRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();


            if (!RequestKeyValues.ContainsKey("sesskey") || !RequestKeyValues.ContainsKey("newprofileid") || !RequestKeyValues.ContainsKey("reason"))
            {
                throw new GPParseException("addbuddy request is invalid.");
            }

            uint friendPID;
            if (!uint.TryParse(RequestKeyValues["newprofileid"], out friendPID))
            {
                throw new GPParseException("newprofileid format is incorrect.");
            }

            FriendProfileID = friendPID;
            Reason = RequestKeyValues["reason"];
        }
    }
}