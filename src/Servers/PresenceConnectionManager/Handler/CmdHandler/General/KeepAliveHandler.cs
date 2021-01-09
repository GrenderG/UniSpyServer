﻿using UniSpyLib.Abstraction.Interface;
using System.Collections.Generic;
using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure.Response;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    public class KeepAliveHandler : PCMCmdHandlerBase
    {
        public KeepAliveHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            //we need to keep player cache online
            //so their friends can find him
        }

        protected override void ResponseConstruct()
        {
            _response = new KeepAliveResponse(_request,_result);
        }
    }
}
