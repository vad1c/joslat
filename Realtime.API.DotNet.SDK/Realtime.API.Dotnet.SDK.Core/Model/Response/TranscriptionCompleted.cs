using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtime.API.Dotnet.SDK.Core.Model.Response
{
    public class TranscriptionCompleted
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("item_id")]
        public string ItemId { get; set; }
        [JsonProperty("content_index")]
        public int ContentIndex { get; set; }
        [JsonProperty("transcript")]
        public string Transcript { get; set; }
    }
}
