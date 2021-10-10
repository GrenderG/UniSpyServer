﻿using System.Xml.Serialization;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyRequestBase : IUniSpyRequest
    {
        [XmlIgnoreAttribute]
        public object CommandName { get; protected set; }
        [XmlIgnoreAttribute]
        public object RawRequest { get; protected set; }
        public UniSpyRequestBase() { }
        public UniSpyRequestBase(object rawRequest)
        {
            RawRequest = rawRequest;
            LogWriter.LogCurrentClass(this);
        }

        public abstract void Parse();
    }
}