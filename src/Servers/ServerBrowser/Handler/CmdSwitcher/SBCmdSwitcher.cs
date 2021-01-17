﻿using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace ServerBrowser.Handler.CommandSwitcher
{
    public class SBCmdSwitcher : UniSpyCmdSwitcherBase
    {
        protected new byte[] _rawRequest
        {
            get { return (byte[])base._rawRequest; }
            set { base._rawRequest = value; }
        }
        public SBCmdSwitcher(IUniSpySession session, byte[] rawRequest) : base(session, rawRequest)
        {
        }

        protected override void SerializeCommandHandlers()
        {
            foreach (var request in _requests)
            {
                var handler = new SBCmdHandlerFactory(_session, request).Serialize();
                if (handler == null)
                {
                    continue;
                }

                _handlers.Add(handler);
            }
        }

        protected override void SerializeRequests()
        {
            var request = new SBRequestFactory(_rawRequest).Serialize();
            request.Parse();
            if (!(bool)request.ErrorCode)
            {
                return;
            }
            _requests.Add(request);
        }
    }
}