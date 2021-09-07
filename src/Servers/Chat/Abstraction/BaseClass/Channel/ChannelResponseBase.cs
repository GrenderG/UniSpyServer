﻿using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Abstraction.BaseClass
{
    internal abstract class ChannelResponseBase : ResponseBase
    {
        protected new ChannelRequestBase _request => (ChannelRequestBase)base._request;
        protected ChannelResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
    }
}