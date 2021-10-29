using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UniSpyServer.WebServer.Abstraction;
using UniSpyServer.WebServer.Entity.Contract;

namespace UniSpyServer.WebServer.Entity.Structure.Request.SakeRequest
{
    [RequestContract("GetRandomRecords")]
    public class GetRandomRecordsRequest : SakeRequestBase
    {
        public string Max { get; set; }
        public List<FieldObject> Fields { get; set; }
        public GetRandomRecordsRequest(string rawRequest) : base(rawRequest)
        {
            Fields = new List<FieldObject>();
        }

        public override void Parse()
        {
            base.Parse();
            var max = _contentElement.Descendants().Where(p => p.Name.LocalName == "max").First().Value;
            Max = max;
            var fieldsNode = _contentElement.Descendants().Where(p => p.Name.LocalName == "fields").First();
            foreach (XElement element in fieldsNode.Nodes())
            {
                Fields.Add(new FieldObject(element.Value, element.Name.LocalName));
            }
        }
    }
}