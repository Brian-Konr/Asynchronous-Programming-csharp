using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomReportExtensions.Schemas
{
    public class CustomReportRequest
    {
        [JsonProperty(PropertyName = "dtno")]
        public int Dtno { get; set; }

        [JsonProperty(PropertyName = "ftno")]
        public int Ftno { get; set; }

        [JsonProperty(PropertyName = "params")]
        public string? Params { get; set; }

        [JsonProperty(PropertyName = "assignSpid")]
        public string? AssignSpid { get; set; }

        [JsonProperty(PropertyName = "keyMap")]
        public string? KeyMap { get; set; }

        public CustomReportRequest(int dtno, int ftno, string? parameters, string? assignSpid, string? keyMap)
        {
            Dtno = dtno;
            Ftno = ftno;
            Params = parameters;
            AssignSpid = assignSpid;
            KeyMap = keyMap;
        }
    }
}
