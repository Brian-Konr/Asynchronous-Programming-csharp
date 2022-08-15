using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomReportExtensions.Schemas
{
    /// <summary>
    /// Response body schema for Custom Report Request 
    /// </summary>
    public class QueryDelegateResponse
    {
        /// <summary>
        /// isCompleted property
        /// </summary>
        [JsonProperty(PropertyName = "isCompleted")]
        public bool IsCompleted { get; set; }

        /// <summary>
        /// isFaulted property
        /// </summary>
        [JsonProperty(PropertyName = "isFaulted")]
        public bool IsFaulted { get; set; }

        /// <summary>
        /// signature property
        /// </summary>
        [JsonProperty(PropertyName = "signature")]
        public string? Signature { get; set; }

        /// <summary>
        /// exception property
        /// </summary>
        [JsonProperty(PropertyName = "exception")]
        public string? Exception { get; set; }

        /// <summary>
        /// result property
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        public string? Result { get; set; }

        public QueryDelegateResponse(bool isCompleted, bool isFaulted, string? signature, string? exception, string? result)
        {
            IsCompleted = isCompleted;
            IsFaulted = isFaulted;
            Signature = signature;
            Exception = exception;
            Result = result;
        }
    }
}
