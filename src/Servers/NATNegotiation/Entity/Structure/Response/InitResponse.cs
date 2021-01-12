﻿using System.Collections.Generic;
using UniSpyLib.Extensions;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Structure.Request;
using UniSpyLib.Abstraction.BaseClass;
using NATNegotiation.Entity.Structure.Result;

namespace NATNegotiation.Entity.Structure.Response
{
    public class InitResponse : NNResponseBase
    {
        protected new InitRequest _request => (InitRequest)base._request;
        protected new InitResult _result => (InitResult)base._result;
        public InitResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildNormalResponse()
        {
            List<byte> data = new List<byte>();
            data.AddRange(SendingBuffer);

            data.Add((byte)_request.PortType);
            data.Add(_request.ClientIndex);
            data.Add(_request.UseGamePort);

            data.AddRange(HtonsExtensions.IPStringToBytes(_result.LocalIP));
            data.AddRange(HtonsExtensions.UshortPortToBytes(_result.LocalPort));

            SendingBuffer = data.ToArray();
        }
    }
}
