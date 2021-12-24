﻿using UniSpyServer.Servers.ServerBrowser.Abstraction.BaseClass;
using System;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.ServerBrowser.Entity.Structure.Response.ServerList
{
    /// <summary>
    /// Only get the servers IP
    /// </summary>
    public sealed class ServerNetworkInfoListResponse : ServerListUpdateOptionResponseBase
    {
        public ServerNetworkInfoListResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            base.Build();
            SendingBuffer = _serverListData.ToArray();
        }
        protected override void BuildServersInfo()
        {
            throw new NotImplementedException();
        }
    }
}