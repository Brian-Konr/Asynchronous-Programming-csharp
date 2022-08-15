using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomReportExtensions.Schemas
{
    public class QueryDelegateResponse
    {
        [JsonProperty(PropertyName = "isCompleted")]
        public bool IsCompleted { get; set; }

        [JsonProperty(PropertyName = "isFaulted")]
        public bool IsFaulted { get; set; }

        [JsonProperty(PropertyName = "signature")]
        public string? Signature { get; set; }

        [JsonProperty(PropertyName = "exception")]
        public string? Exception { get; set; }

        [JsonProperty(PropertyName = "result")]
        public string? Result { get; set; }
    }
}
