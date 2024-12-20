using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.RealtimeApi.Dotnet.SDK.Core.Model.Common
{
    public class OutputItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("role")]
        public string Role { get; set; }
        [JsonProperty("content")]
        public List<Content> Content { get; set; }
    }
}
