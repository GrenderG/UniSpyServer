﻿using UniSpyServer.Servers.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Response
{
    public sealed class ValidResponse : ResponseBase
    {
        private new ValidResult _result => (ValidResult)base._result;

        public ValidResponse(RequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            if (_result.IsAccountValid)
            {
                SendingBuffer = @"\vr\1\final\";
            }
            else
            {
                SendingBuffer = @"\vr\0\final\";
            }
        }
    }
}