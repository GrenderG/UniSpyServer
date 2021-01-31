﻿using Chat.Entity.Structure;
using Chat.Entity.Structure.Misc;
using UniSpyLib.Abstraction.Interface;
using Chat.Entity.Structure.Misc.ChannelInfo;
using Chat.Abstraction.BaseClass.Message;

namespace Chat.Abstraction.BaseClass
{
    internal abstract class ChatMsgHandlerBase : ChatChannelHandlerBase
    {
        protected new ChatMsgRequestBase _request => (ChatMsgRequestBase)base._request;
        protected new ChatMsgResultBase _result
        {
            get =>(ChatMsgResultBase)base._result;
            set =>base._result = value;
        }
        protected ChatChannelUser _reciever;
        public ChatMsgHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void RequestCheck()
        {
            switch (_request.RequestType)
            {
                case ChatMessageType.ChannelMessage:
                    base.RequestCheck();
                    break;
                case ChatMessageType.UserMessage:

                    if (_request.RequestType == ChatMessageType.UserMessage)
                    {
                        _reciever = _channel.GetChannelUserByNickName(_request.NickName);
                        if (_reciever == null)
                        {
                            _result.ErrorCode = ChatErrorCode.IRCError;
                            _result.IRCErrorCode = ChatIRCErrorCode.NoSuchNick;
                        }
                    }
                    break;
                default:
                    _result.ErrorCode = ChatErrorCode.Parse;
                    break;
            }
        }
        protected override void DataOperation()
        {
            switch (_request.RequestType)
            {
                case ChatMessageType.ChannelMessage:
                    ChannelMessageDataOpration();
                    break;
                case ChatMessageType.UserMessage:
                    UserMessageDataOperation();
                    break;
            }
        }

        protected virtual void ChannelMessageDataOpration()
        {
            _result.TargetName = _request.ChannelName;
        }
        protected virtual void UserMessageDataOperation()
        {
            _result.TargetName = _request.NickName;
        }
        protected override void Response()
        {
            // response can not be null!
            _response.Build();
            switch (_request.RequestType)
            {
                case ChatMessageType.ChannelMessage:
                    _channel.MultiCastExceptSender(_user, (string)_response.SendingBuffer);
                    break;
                case ChatMessageType.UserMessage:
                    _reciever.UserInfo.Session.SendAsync((string)_response.SendingBuffer);
                    break;
            }

        }
    }
}