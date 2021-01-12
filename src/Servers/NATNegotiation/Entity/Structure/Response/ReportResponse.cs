﻿using NATNegotiation.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace NATNegotiation.Entity.Structure.Response
{
    public class ReportResponse : NNResponseBase
    {
        public ReportResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
    }
}
