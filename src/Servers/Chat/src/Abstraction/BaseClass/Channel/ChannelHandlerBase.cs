﻿using UniSpyServer.Servers.Chat.Entity.Exception.IRC.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc.ChannelInfo;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Abstraction.BaseClass
{
    public abstract class ChannelHandlerBase : LogedInHandlerBase
    {
        protected Channel _channel;
        protected ChannelUser _user;
        private new ChannelRequestBase _request => (ChannelRequestBase)base._request;
        public ChannelHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void RequestCheck()
        {
            base.RequestCheck();
            _channel = _session.UserInfo.GetJoinedChannel(_request.ChannelName);
            _user = _channel.GetChannelUserBySession(_session);
            if (_user == null)
            {
                throw new ChatIRCNoSuchNickException($"Can not find user with nickname: {_session.UserInfo.NickName} username: {_session.UserInfo.UserName}");
            }
        }
    }
}