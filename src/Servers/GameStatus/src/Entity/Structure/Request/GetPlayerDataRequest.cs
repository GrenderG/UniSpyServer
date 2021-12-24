﻿using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Contract;
using UniSpyServer.Servers.GameStatus.Entity.Enumerate;
using UniSpyServer.Servers.GameStatus.Entity.Exception;
using System;
using System.Collections.Generic;

namespace UniSpyServer.Servers.GameStatus.Entity.Structure.Request
{
    [RequestContract("getpd")]
    public sealed class GetPlayerDataRequest : RequestBase
    {
        public uint ProfileID { get; private set; }
        public PersistStorageType StorageType { get; private set; }
        public uint DataIndex { get; private set; }
        public List<string> Keys { get; private set; }
        public bool GetAllDataFlag { get; private set; }
        public GetPlayerDataRequest(string rawRequest) : base(rawRequest)
        {
            Keys = new List<string>();
        }

        public override void Parse()
        {
            base.Parse();


            if (RequestKeyValues.ContainsKey("pid"))
            {
                uint profileID;
                if (!uint.TryParse(RequestKeyValues["pid"], out profileID))
                {
                    throw new GSException("pid format is incorrect.");
                }
                ProfileID = profileID;
            }

            if (RequestKeyValues.ContainsKey("ptype"))
            {
                PersistStorageType storageType;
                if (!Enum.TryParse(RequestKeyValues["ptype"], out storageType))
                {
                    throw new GSException("ptype format is incorrect.");
                }
                StorageType = storageType;
            }


            if (RequestKeyValues.ContainsKey("dindex"))
            {
                uint dataIndex;
                if (!uint.TryParse(RequestKeyValues["dindex"], out dataIndex))
                {
                    throw new GSException("dindex format is incorrect.");
                }
                DataIndex = dataIndex;
            }

            if (!RequestKeyValues.ContainsKey("keys"))
            {
                throw new GSException("keys is missing.");
            }

            string keys = RequestKeyValues["keys"];
            if (keys == "")
            {
                GetAllDataFlag = true;
            }
            else
            {
                string[] keyArray = keys.Split('\x1');
                foreach (var key in keyArray)
                {
                    Keys.Add(key);
                }
                GetAllDataFlag = false;
            }
        }
    }
}