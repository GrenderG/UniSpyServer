﻿using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using NATNegotiation.Entity.Structure;
using NATNegotiation.Handler.CommandHandler;

namespace NatNegotiation.Handler.CommandHandler
{
    public class AddressHandler : CommandHandlerBase
    {

        protected override void ConvertRequest(ClientInfo client, byte[] recv)
        {
            _initPacket = new InitPacket();
            _initPacket.Parse(recv);
        }

        protected override void ConstructResponsePacket(ClientInfo client, byte[] recv)
        {
            _sendingBuffer = _initPacket.GenerateResponse(NatPacketType.AddressReply, client.RemoteEndPoint);
        }
    }
}
