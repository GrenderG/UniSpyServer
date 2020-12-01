﻿using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatCommand
{
    public class LOGINPREAUTHRequest : ChatRequestBase
    {
        public LOGINPREAUTHRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string AuthToken { get; protected set; }
        public string PartnerChallenge { get; protected set; }

        public override object Parse()
        {
            if(!(bool)base.Parse())
            {
                return false;
            }

            AuthToken = _cmdParams[0];
            PartnerChallenge = _cmdParams[1];
            return true;
        }
    }
}
