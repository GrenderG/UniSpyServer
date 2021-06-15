﻿using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request.General
{
    internal sealed class REGISTERNICKRequest : ChatRequestBase
    {
        public REGISTERNICKRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string NamespaceID { get; protected set; }
        public string UniqueNick { get; protected set; }
        public string CDKey { get; protected set; }
        public override void Parse()
        {
            base.Parse();


            NamespaceID = _cmdParams[0];
            UniqueNick = _cmdParams[1];
            CDKey = _cmdParams[2];
        }
    }
}
