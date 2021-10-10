﻿using System.Net;

namespace UniSpyLib.Abstraction.Interface
{
    public interface IUniSpySession
    {
        EndPoint RemoteEndPoint { get; }
        IPEndPoint RemoteIPEndPoint { get; }
        bool Send(IUniSpyResponse response);
        bool BaseSend(IUniSpyResponse response);
    }
}