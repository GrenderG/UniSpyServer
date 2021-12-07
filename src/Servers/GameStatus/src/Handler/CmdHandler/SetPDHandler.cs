﻿using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Contract;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Request;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Result;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel.MySql;

namespace UniSpyServer.Servers.GameStatus.Handler.CmdHandler
{
    /// <summary>
    /// Set persist storage data
    /// </summary>
    [HandlerContract("setpd")]
    public sealed class SetPDHandler : CmdHandlerBase
    {
        private new SetPlayerDataRequest _request => (SetPlayerDataRequest)base._request;
        public SetPDHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new SetPDResult();
        }

        protected override void DataOperation()
        {
            using (var db = new UnispyContext())
            {

                var result = from p in db.Pstorage
                             where p.Profileid == _request.ProfileID
                             && p.Dindex == _request.DataIndex
                             && p.Ptype == (uint)_request.StorageType
                             select p;

                Pstorage ps;
                if (result.Count() == 0)
                {
                    //insert a new record in database
                    ps = new Pstorage();
                    ps.Dindex = _request.DataIndex;
                    ps.Profileid = _request.ProfileID;
                    ps.Ptype = (uint)_request.StorageType;
                    ps.Data = _request.KeyValueString;
                    db.Pstorage.Add(ps);
                }
                else if (result.Count() == 1)
                {
                    //update an existed record in database
                    ps = result.First();
                    ps.Data = _request.KeyValueString;
                }

                db.SaveChanges();
            }
        }

        protected override void ResponseConstruct()
        {
            throw new System.NotImplementedException();
        }
    }
}
