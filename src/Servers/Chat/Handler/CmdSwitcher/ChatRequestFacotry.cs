﻿using Chat.Abstraction.BaseClass;
using Serilog.Events;
using System;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace Chat.Handler.CommandSwitcher
{
    internal sealed class ChatRequestFacotry : UniSpyRequestFactoryBase
    {
        private new string _rawRequest => (string)base._rawRequest;
        public ChatRequestFacotry(object rawRequest) : base(rawRequest)
        {
        }


        public override IUniSpyRequest Serialize()
        {
            string commandName = ChatRequestBase.GetCommandName(_rawRequest);
            Type requestType = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .FirstOrDefault(t => t.Name == commandName + "Request");
            if (requestType != null)
            {
                var request = Activator.CreateInstance(requestType, _rawRequest);
                if (request == null)
                {
                    LogWriter.ToLog(LogEventLevel.Error, $"Unknown request {commandName}!");
                    return null;
                }
                return (IUniSpyRequest)request;
            }
            else
            {
                LogWriter.ToLog(LogEventLevel.Error, $"Request: {commandName} not implemented!");
                return null;
            }
        }


    }
}
