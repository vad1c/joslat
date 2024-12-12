using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtime.API.Dotnet.SDK.Core.Model.Response
{
    /// <summary>
    /// response.content_part.added
    /// </summary>
    public class ResponseContentPartAdded : BaseResponse
    {
        [JsonProperty("response_id")]
        public string ResponseId { get; set; }
        [JsonProperty("item_id")]
        public string ItemId { get; set; }
        [JsonProperty("output_index")]
        public int OutputIndex { get; set; }
        [JsonProperty("content_index")]
        public int ContentIndex { get; set; }
        [JsonProperty("part")]
        public Common.Content Part { get; set; }

    }
}
