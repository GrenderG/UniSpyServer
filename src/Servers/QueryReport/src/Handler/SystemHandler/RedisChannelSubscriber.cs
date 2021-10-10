using QueryReport.Application;
using QueryReport.Entity.Exception;
using QueryReport.Entity.Structure.NATNeg;
using QueryReport.Entity.Structure.Request;
using QueryReport.Entity.Structure.Response;
using QueryReport.Entity.Structure.Result;
using QueryReport.Network;
using System.Net;
using UniSpyLib.Abstraction.BaseClass.Redis;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Entity.Structure;

namespace QueryReport.Handler.SystemHandler
{
    public class RedisChannelSubscriber : UniSpyRedisChannel<NatNegCookie>
    {
        public RedisChannelSubscriber() : base(UniSpyRedisChannelName.NatNegCookieChannel)
        {
        }

        public override void ReceivedMessage(NatNegCookie message)
        {
            var address = IPAddress.Parse(message.GameServerRemoteIP);
            int port = int.Parse(message.GameServerRemotePort);
            var endPoint = new IPEndPoint(address, port);

            if (!ServerFactory.Server.SessionManager.SessionPool.ContainsKey(message.GameServerRemoteEndPoint))
            {
                throw new QRException("Can not find game server in QR");
            }
            var session = ServerFactory.Server.SessionManager.SessionPool[message.GameServerRemoteEndPoint];
            
            // if (!ServerFactory.Server.SessionManager.SessionPool.TryGetValue(message.GameServerRemoteEndPoint, out session))
            // {
            //     throw new QRException("Can not find game server in QR");
            // }
            // if (!ServerFactory.Server.SessionManager.SessionPool.TryGetValue(endPoint, out session))
            // {
            //     throw new QRException("Can not find game server in QR");
            // }

            var result = new ClientMessageResult
            {
                NatNegMessage = message.NatNegMessage,
                MessageKey = 0,
                InstantKey = ((Session)session).InstantKey
            };
            var request = new ClientMessageRequest()
            {
                InstantKey = ((Session)session).InstantKey
            };
            var response = new ClientMessageResponse(request, result);
            response.Build();
            ServerFactory.Server.SendAsync(endPoint, response.SendingBuffer);
        }
    }
}