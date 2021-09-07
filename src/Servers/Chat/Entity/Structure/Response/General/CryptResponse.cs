﻿using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Result.General;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.General
{
    internal sealed class CryptResponse : ResponseBase
    {
        private new CRYPTResult _result => (CRYPTResult)base._result;
        public CryptResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            var cmdParams = $"* {ChatConstants.ClientKey} {ChatConstants.ServerKey}";
            SendingBuffer = ChatIRCReplyBuilder.Build(
                ChatReplyName.SecureKey, cmdParams);
        }
    }
}