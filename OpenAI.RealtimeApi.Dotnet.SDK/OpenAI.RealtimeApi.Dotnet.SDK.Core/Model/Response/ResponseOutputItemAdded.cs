using Newtonsoft.Json;
using OpenAI.RealtimeApi.Dotnet.SDK.Core.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.RealtimeApi.Dotnet.SDK.Core.Model.Response
{
    public class ResponseOutputItemAdded : BaseResponse
    {
        [JsonProperty("response_id")]
        public string ResponseId { get; set; }

        [JsonProperty("output_index")]
        public int OutputIndex { get; set; }

        [JsonProperty("item")]
        public OutputItem Item { get; set; }
    }

}
