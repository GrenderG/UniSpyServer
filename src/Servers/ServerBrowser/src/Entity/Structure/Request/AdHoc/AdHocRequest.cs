﻿using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using System;
using System.Net;
using UniSpyLib.Extensions;

namespace ServerBrowser.Entity.Structure.Request
{
    internal sealed class AdHocRequest : RequestBase
    {
        /// <summary>
        /// The game server client search for
        /// </summary>
        public string TargetServerIP => TargetIPEndPoint.Address.ToString();
        public string TargetServerHostPort => TargetIPEndPoint.Port.ToString();
        public IPEndPoint TargetIPEndPoint { get; private set; }

        public AdHocRequest(byte[] rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            //if(recv.Length<length)
            //{
            //    return false;
            //}
            CommandName = (RequestType)RawRequest[2];

            byte[] ip = ByteTools.SubBytes(RawRequest, 3, 4);
            byte[] port = ByteTools.SubBytes(RawRequest, 7, 2);
            Array.Reverse(port);

            //TODO fix for gbrome!!!!!!!!!!!!!!!!!!!
            TargetIPEndPoint = ByteTools.GetIPEndPoint(ip, port);
        }
    }
}