﻿using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Misc;
using System;

namespace UniSpyServer.Servers.PresenceConnectionManager.Structure.Data
{
    public sealed class UserInfo
    {
        public const ushort SessionKey = 1111;
        public const string LoginTicket = "0000000000000000000000__";

        public DateTime CreatedTime { get; private set; }
        public Guid UserGuid { get; private set; }
        public PCMBasicInfo BasicInfo { get; private set; }
        public PCMSDKRevision SDKRevision { get; private set; }
        public PCMUserStatus Status { get; set; }
        public PCMUserStatusInfo StatusInfo { get; set; }

        public UserInfo(Guid guid)
        {
            UserGuid = guid;
            CreatedTime = DateTime.Now;
            BasicInfo = new PCMBasicInfo();
            Status = new PCMUserStatus();
            SDKRevision = new PCMSDKRevision();
            StatusInfo = new PCMUserStatusInfo();
        }
    }
}