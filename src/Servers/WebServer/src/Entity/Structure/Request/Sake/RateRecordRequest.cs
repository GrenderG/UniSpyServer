using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Contract;

namespace UniSpyServer.Servers.WebServer.Entity.Structure.Request.Sake
{
    [RequestContract("RateRecord")]
    public class RateRecordRequest : Abstraction.Sake.RequestBase
    {
        public string RecordId { get; set; }
        public string Rating { get; set; }
        public RateRecordRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            var recordid = _contentElement.Descendants().Where(p => p.Name.LocalName == "recordid").First().Value;
            RecordId = recordid;
            var rating = _contentElement.Descendants().Where(p => p.Name.LocalName == "rating").First().Value;
            Rating = rating;
        }
    }
}