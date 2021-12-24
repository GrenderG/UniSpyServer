﻿using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request.Profile;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Response;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Result;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel.MySql;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler
{
    [HandlerContract("newprofile")]
    public sealed class NewProfileHandler : Abstraction.BaseClass.CmdHandlerBase
    {
        private new NewProfileRequest _request => (NewProfileRequest)base._request;

        private new NewProfileResult _result{ get => (NewProfileResult)base._result; set => base._result = value; }
        public NewProfileHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new NewProfileResult();
        }
        protected override void DataOperation()
        {
            using (var db = new UnispyContext())
            {
                if (_request.IsReplaceNickName)
                {
                    var result = from p in db.Profiles
                                 where p.Profileid == _session.UserInfo.BasicInfo.ProfileID
                                 && p.Nick == _request.OldNick
                                 select p;

                    if (result.Count() != 1)
                    {
                        throw new GPDatabaseException("No user infomation found in database.");
                    }
                    else
                    {
                        result.First().Nick = _request.NewNick;
                    }

                    db.Profiles.Where(p => p.Profileid == _session.UserInfo.BasicInfo.ProfileID
                    && p.Nick == _request.OldNick).First().Nick = _request.NewNick;

                    db.SaveChanges();
                }
                else
                {
                    Profiles profiles = new Profiles
                    {
                        Profileid = _session.UserInfo.BasicInfo.ProfileID,
                        Nick = _request.NewNick,
                        Userid = _session.UserInfo.BasicInfo.UserID
                    };

                    db.Add(profiles);
                }
            }
            _result.ProfileID = _session.UserInfo.BasicInfo.ProfileID;
        }

        protected override void ResponseConstruct()
        {
            _response = new NewProfileResponse(_request, _result);
        }
    }
}