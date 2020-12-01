﻿using Chat.Abstraction.BaseClass;
using UniSpyLib.Extensions;
using System.Collections.Generic;

namespace Chat.Entity.Structure.ChatCommand
{
    public class SETCHANKEYRequest : ChatChannelRequestBase
    {
        public Dictionary<string, string> KeyValue { get; protected set; }

        public SETCHANKEYRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override object Parse()
        {
            if(!(bool)base.Parse())
            {
                return false;
            }

            if (_longParam == null)
                return false;

            _longParam = _longParam.Substring(1);

            KeyValue = StringExtensions.ConvertKVStrToDic(_longParam);

            return true;
        }
    }
}
