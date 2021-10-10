﻿using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;

namespace Chat.Entity.Structure.Request
{
    [RequestContract("UTM")]
    internal sealed class UnderTheTableMsgRequest : MsgRequestBase
    {
        public UnderTheTableMsgRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}