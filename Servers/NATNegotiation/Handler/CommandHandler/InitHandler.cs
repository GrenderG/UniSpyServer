﻿using NatNegotiation.Entity.Structure.Packet;
using System.Net;

namespace NatNegotiation.Handler.CommandHandler
{
    public class InitHandler
    {

        public void Handle(NatNegServer server,EndPoint endPoint, byte[] recv)
        {

            InitPacket initPacket = new InitPacket(recv);
            byte[] sendingBuffer = initPacket.CreateReplyPacket();
            server.SendAsync(server.Socket.RemoteEndPoint, sendingBuffer);
        }
    }
}
