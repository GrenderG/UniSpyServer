﻿using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;

namespace Chat.Entity.Structure.Request
{
    [RequestContract("NOTICE")]
    public sealed class NoticeRequest : MsgRequestBase
    {
        public NoticeRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
