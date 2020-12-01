﻿using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Enumerate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStatus.Entity.Structure.Request
{
    public class SetPDRequest : GSRequestBase
    {
        public SetPDRequest(Dictionary<string, string> request) : base(request)
        {
        }
        public uint ProfileID { get; protected set; }
        public PersistStorageType StorageType { get; protected set; }
        public uint DataIndex { get; protected set; }
        public uint Length { get; protected set; }
        public string KeyValueString { get; protected set; }

        public override GSError Parse()
        {
            var flag = base.Parse();
            if (flag != GSError.NoError)
            {
                return flag;
            }

            if (!_rawRequest.ContainsKey("pid") || !_rawRequest.ContainsKey("ptype") || !_rawRequest.ContainsKey("dindex") || !_rawRequest.ContainsKey("length"))
            {
                return GSError.Parse;
            }

            uint profileID;
            if (!uint.TryParse(_rawRequest["pid"], out profileID))
            {
                return GSError.Parse;
            }
            ProfileID = profileID;

            uint storageType;
            if (!uint.TryParse(_rawRequest["ptype"], out storageType))
            {
                return GSError.Parse;
            }

            if (!Enum.IsDefined(typeof(PersistStorageType), storageType))
            {
                return GSError.Parse;
            }

            StorageType = (PersistStorageType)storageType;

            uint dindex;
            if (!uint.TryParse(_rawRequest["dindex"], out dindex))
            {
                return GSError.Parse;
            }
            DataIndex = dindex;

            uint length;
            if (!uint.TryParse(_rawRequest["length"], out length))
            {
                return GSError.Parse;
            }
            Length = length;

            //we extract the key value data
            foreach (var d in _rawRequest.Skip(5))
            {
                if (d.Key == "lid")
                    break;
                KeyValueString += @"\" + d.Key + @"\" + d.Value;
            }

            return GSError.NoError;
        }
    }
}
