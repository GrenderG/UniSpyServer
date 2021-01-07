﻿using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Structure.Request;
using System.Collections.Generic;
using System.Linq;
using PresenceSearchPlayer.Entity.Structure.Result;

/////////////////////////Finished?/////////////////////////////////
namespace PresenceSearchPlayer.Handler.CmdHandler
{
    /// <summary>
    /// Uses a email and namespaceid to find all nick in this account
    /// </summary>
    public class NicksHandler : PSPCmdHandlerBase
    {
        protected new NicksResult _result
        {
            get { return (NicksResult)base._result; }
            set { base._result = value; }
        }
        protected new NicksRequest _request { get { return (NicksRequest)base._request; } }
        public NicksHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {

            using (var db = new retrospyContext())
            {
                var result = from u in db.Users
                             join p in db.Profiles on u.Userid equals p.Userid
                             join n in db.Subprofiles on p.Profileid equals n.Profileid
                             where u.Email == _request.Email
                             && u.Password == _request.Password
                             && n.Namespaceid == _request.NamespaceID
                             select new NicksDataBaseModel
                             {
                                 NickName = p.Nick,
                                 UniqueNick = n.Uniquenick
                             };

                //we store data in strong type so we can use in next step
                _result.DataBaseResults.AddRange(result.ToList());
            }

        }
    }
}
