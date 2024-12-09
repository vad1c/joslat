using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtime.API.Dotnet.SDK.Core.Model.Response
{
    public class ResponseCreated : BaseResponse
    {
        [JsonProperty("response")]
        public Response Response { get; set; }
    }

    public class Response 
    {
        [JsonProperty("response")]
        public string MyProperty { get; set; }
        [JsonProperty("object")]
        public string Object { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("status_details")]
        public StatusDetails StatusDetails { get; set; }
        [JsonProperty("output")]
        public List<Output>  Output { get; set; }
        [JsonProperty("usage")]
        public Usage Usage { get; set; }
    }

    public class StatusDetails 
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("error")]
        public Error Error { get; set; }
    }
    public class Error 
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public class Output 
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
    public class Usage 
    {
        [JsonProperty("total_tokens")]
        public string TotalTokens { get; set; }
        [JsonProperty("input_tokens")]
        public string InputTokens { get; set; }
        [JsonProperty("output_tokens")]
        public string OutputTokens { get; set; }
    }
}
