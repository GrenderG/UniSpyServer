﻿using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceConnectionManager.Entity.Structure.Request.Profile
{
    public class UpdateUIRequest : PCMRequestBase
    {
        public UpdateUIRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public override GPErrorCode Parse()
        {
            var flag = base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }

            if (_recv.ContainsKey(""))
            {

            }
            //cpubrandid
            //cpuspeed
            //memory
            //videocard1ram
            //videocard2ram
            //connectionid
            //connectionspeed
            //hasnetwork
            //pic
            return GPErrorCode.NoError;

        }
    }
}
