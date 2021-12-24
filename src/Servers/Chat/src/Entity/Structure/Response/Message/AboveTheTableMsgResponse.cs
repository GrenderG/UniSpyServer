﻿using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Message;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.Message;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Response.Message
{
    public sealed class AboveTheTableMsgResponse : ResponseBase
    {
        private new AboveTheTableMsgRequest _request => (AboveTheTableMsgRequest)base._request;
        private new AboveTheTableMsgResult _result => (AboveTheTableMsgResult)base._result;
        public AboveTheTableMsgResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            string cmdParams = $"{_result.TargetName} {_request.Message}";
            SendingBuffer = IRCReplyBuilder.Build(_result.UserIRCPrefix, ResponseName.AboveTheTableMsg, cmdParams);
        }
    }
}