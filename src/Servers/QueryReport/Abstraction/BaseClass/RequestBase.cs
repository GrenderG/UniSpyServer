﻿using QueryReport.Entity.Enumerate;
using QueryReport.Entity.Exception;
using System;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;

namespace QueryReport.Abstraction.BaseClass
{
    internal abstract class RequestBase : UniSpyRequestBase
    {
        public static readonly byte[] MagicData = { 0xFE, 0XFD };
        public uint InstantKey { get; protected set; }
        public new PacketType CommandName
        {
            get => (PacketType)base.CommandName;
            protected set => base.CommandName = value;
        }
        public new byte[] RawRequest
        {
            get => (byte[])base.RawRequest;
            protected set => base.RawRequest = value;
        }

        public RequestBase(object rawRequest) : base(rawRequest)
        {
        }

        protected RequestBase()
        {
        }

        public override void Parse()
        {
            if (RawRequest.Length < 3)
            {
                throw new QRException("Query report request is invalid.");
            }
            CommandName = (PacketType)RawRequest[0];
            InstantKey = BitConverter.ToUInt32(ByteTools.SubBytes(RawRequest, 1, 4));
        }
    }
}