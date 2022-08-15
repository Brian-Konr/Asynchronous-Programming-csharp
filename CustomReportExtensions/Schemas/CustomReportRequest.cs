using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomReportExtensions.Schemas
{
    /// <summary>
    /// Request body schema of Custom Report Request 
    /// </summary>
    public class CustomReportRequest
    {
        /// <summary>
        /// dtno property
        /// </summary>
        [JsonProperty(PropertyName = "dtno")]
        public int Dtno { get; set; }

        /// <summary>
        /// ftno property
        /// </summary>
        [JsonProperty(PropertyName = "ftno")]
        public int Ftno { get; set; }

        /// <summary>
        /// params property
        /// </summary>
        [JsonProperty(PropertyName = "params")]
        public string? Params { get; set; }

        /// <summary>
        /// assignSpid property
        /// </summary>
        [JsonProperty(PropertyName = "assignSpid")]
        public string? AssignSpid { get; set; }

        /// <summary>
        /// keyMap property
        /// </summary>
        [JsonProperty(PropertyName = "keyMap")]
        public string? KeyMap { get; set; }

        /// <summary>
        /// Constructor for a request
        /// </summary>
        /// <param name="dtno">dtno property</param>
        /// <param name="ftno">ftno property</param>
        /// <param name="parameters">params property</param>
        /// <param name="assignSpid">assignSpid property</param>
        /// <param name="keyMap">keyMap property</param>
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
