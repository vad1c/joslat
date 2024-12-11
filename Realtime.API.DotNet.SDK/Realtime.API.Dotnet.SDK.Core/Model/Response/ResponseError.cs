using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtime.API.Dotnet.SDK.Core.Model.Response
{
    /// <summary>
    /// error
    /// </summary>
    public class ResponseError : BaseResponse
    {
        [JsonProperty("error")]
        public ResError Error { get; set; }
    }

    public class ResError 
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("param")]
        public string Param { get; set; }
        [JsonProperty("event_id")]
        public string EventId { get; set; }
      
    }
}
