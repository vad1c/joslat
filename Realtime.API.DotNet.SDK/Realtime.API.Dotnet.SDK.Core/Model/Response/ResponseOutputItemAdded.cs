using Newtonsoft.Json;
using Realtime.API.Dotnet.SDK.Core.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtime.API.Dotnet.SDK.Core.Model.Response
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
