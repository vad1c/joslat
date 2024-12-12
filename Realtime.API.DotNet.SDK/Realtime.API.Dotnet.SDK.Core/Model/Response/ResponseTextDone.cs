using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtime.API.Dotnet.SDK.Core.Model.Response
{
    /// <summary>
    /// response.text.done
    /// </summary>
    public class ResponseTextDone : BaseResponse
    {
        [JsonProperty("response_id")]
        public string ResponseId { get; set; }
        [JsonProperty("item_id")]
        public string ItemId { get; set; }
        [JsonProperty("output_index")]
        public int OutputIndex { get; set; }
        [JsonProperty("content_index")]
        public int ContentIndex { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
