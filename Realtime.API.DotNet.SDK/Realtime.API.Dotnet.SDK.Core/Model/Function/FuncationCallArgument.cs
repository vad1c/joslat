using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtime.API.Dotnet.SDK.Core.Model.Function
{
    //TODO : BaseResponse
    public class FuncationCallArgument
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("event_id")]
        public string EventId { get; set; }
        [JsonProperty("response_id")]
        public string ResponseId { get; set; }
        [JsonProperty("item_id")]
        public string ItemId { get; set; }
        [JsonProperty("output_index")]
        public int OutputIndex { get; set; }
        [JsonProperty("call_id")]
        public string CallId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("arguments")]
        public string Arguments { get; set; }
    }


    // TODO delete
    public class WeatherArgument
    {
        [JsonProperty("city")]
        public string City { get; set; }
    }

    // TODO delete
    public class NoteArgument 
    {
        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }
    }
}
