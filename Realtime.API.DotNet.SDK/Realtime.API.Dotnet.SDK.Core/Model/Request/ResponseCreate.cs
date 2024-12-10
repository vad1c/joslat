using Newtonsoft.Json;
using Realtime.API.Dotnet.SDK.Core.Model.Common;
using Realtime.API.Dotnet.SDK.Core.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtime.API.Dotnet.SDK.Core.Model.Request
{
    public class ResponseCreate
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; } = "session.create";
        [JsonProperty("response")]
        public Response Response { get; set; }
    }

    public class Response
    {
        [JsonProperty("modalities")]
        public List<string> Modalities { get; set; }
        [JsonProperty("instructions")]
        public string Instructions { get; set; }
        [JsonProperty("voice")]
        public string Voice { get; set; }
        [JsonProperty("outputAudioFormat")]
        public string OutputAudioFormat { get; set; }
        [JsonProperty("tools")]
        public List<Tool> Tools { get; set; }
        [JsonProperty("toolChoice")]
        public string ToolChoice { get; set; }
        [JsonProperty("temperature")]
        public double Temperature { get; set; }
        [JsonProperty("maxOutputTokens")]
        public int MaxOutputTokens { get; set; }
    }
    public class Tool
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("parameters")]
        public ToolParameters Parameters { get; set; }
    }
    public class ToolParameters
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("properties")]
        public ToolProperties Properties { get; set; }
        [JsonProperty("required")]
        public List<string> Required { get; set; }
    }

    public class ToolProperties
    {
        [JsonProperty("a")]
        public Property A { get; set; }
        [JsonProperty("b")]
        public Property B { get; set; }
    }

    public class Property
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
