﻿using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Message;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.Message;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Response.Message
{
    public sealed class UnderTheTableMsgResponse : ResponseBase
    {
        private new UnderTheTableMsgResult _result => (UnderTheTableMsgResult)base._result;
        private new UnderTheTableMsgRequest _request => (UnderTheTableMsgRequest)base._request;
        public UnderTheTableMsgResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = IRCReplyBuilder.Build(_result.UserIRCPrefix, ResponseName.UnderTheTableMsg, _result.Name, _request.Message);
        }
    }
}