﻿using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request;
using System;
using System.Net;
using UniSpyServer.UniSpyLib.MiscMethod;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Redis
{
    public sealed class UserInfo
    {
        [Newtonsoft.Json.JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint RemoteEndPoint { get; set; }
        public InitRequest RequestInfo { get; set; }
        public DateTime LastPacketRecieveTime;
        public int RetryNATNegotiationTime;

        public UserInfo()
        {
        }
    }
}