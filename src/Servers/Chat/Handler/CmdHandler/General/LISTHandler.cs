﻿using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.Misc.ChannelInfo;
using Chat.Entity.Structure.Response.General;
using Chat.Handler.SystemHandler.ChannelManage;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.General
{
    //todo unfinished
    public class LISTHandler : ChatLogedInHandlerBase
    {
        new LISTRequest _request { get { return (LISTRequest)base._request; } }
        //:irc.foonet.com 321 Pants Channel :Users  Name\r\n:irc.foonet.com 323 Pants :End of /LIST\r\n
        public LISTHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void DataOperation()
        {
            base.DataOperation();
            //add list response header

            _sendingBuffer = "";
            if (ChatChannelManager.Channels.Count != 0)
            {
                foreach (var channel in ChatChannelManager.Channels)
                {
                    //TODO
                    //add channel information here
                }
            }
            //add list response tail
            BuildEndOfListRPL();
        }

        public void BuildListRPL(ChatChannel channel)
        {
            _sendingBuffer = LISTReply.BuildListStartReply(_session.UserInfo, channel.Property);
        }

        public void BuildEndOfListRPL()
        {
            _sendingBuffer += LISTReply.BuildListEndReply(_session.UserInfo);
        }

    }
}