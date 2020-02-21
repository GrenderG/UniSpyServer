﻿using PresenceConnectionManager.Enumerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Handler.CommandHandler.Buddy.Status
{
    public class StatusHandler : GPCMHandlerBase
    {
        private uint _statusCode;
        public StatusHandler(GPCMSession session,Dictionary<string, string> recv) : base(session,recv)
        {
        }

        protected override void CheckRequest(GPCMSession session, Dictionary<string, string> recv)
        {
            base.CheckRequest(session,recv);
            if (recv.ContainsKey("status"))
            {
                if (!uint.TryParse(recv["status"], out _statusCode))
                {
                    _errorCode = GPErrorCode.Parse;
                }
            }
            else
                _errorCode = GPErrorCode.Parse;
            
               
        }

        protected override void DataBaseOperation(GPCMSession session, Dictionary<string, string> recv)
        {
            session.UserInfo.StatusCode = (GPStatus)_statusCode;
            session.UserInfo.StatusString = recv["statstring"];
            session.UserInfo.LocationString = recv["locstring"];
        }
    }
}
