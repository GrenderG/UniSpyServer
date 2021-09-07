﻿using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Contract;
using PresenceSearchPlayer.Entity.Exception.General;
using PresenceSearchPlayer.Entity.Structure.Request;
using PresenceSearchPlayer.Entity.Structure.Response;
using PresenceSearchPlayer.Entity.Structure.Result;
using System.Linq;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;

namespace PresenceSearchPlayer.Handler.CmdHandler
{

    [HandlerContract("otherslist")]
    internal sealed class OthersListHandler : CmdHandlerBase
    {
        private new OthersListRequest _request => (OthersListRequest)base._request;

        private new OthersListResult _result
        {
            get => (OthersListResult)base._result;
            set => base._result = value;
        }

        public OthersListHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new OthersListResult();
        }

        protected override void DataOperation()
        {
            try
            {
                using (var db = new unispyContext())
                {
                    foreach (var pid in _request.ProfileIDs)
                    {
                        var result = from n in db.Subprofiles
                                     where n.Profileid == pid
                                     && n.Namespaceid == _request.NamespaceID
                                     //select new { uniquenick = n.Uniquenick };
                                     select new OthersListDatabaseModel
                                     {
                                         ProfileID = n.Profileid,
                                         Uniquenick = n.Uniquenick
                                     };

                        _result.DatabaseResults.AddRange(result.ToList());
                    }
                }
            }
            catch (System.Exception e)
            {
                throw new GPDatabaseException("Unknown error occurs in database operation.", e);
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new OthersListResponse(_request, _result);
        }
    }
}
