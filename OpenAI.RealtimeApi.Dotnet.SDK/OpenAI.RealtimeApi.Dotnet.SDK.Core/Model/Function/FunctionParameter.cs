using Newtonsoft.Json;
using OpenAI.RealtimeApi.Dotnet.SDK.Core.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.RealtimeApi.Dotnet.SDK.Core.Model.Function
{
    public class FunctionParameter
    {
        [JsonProperty("type")]
        public string Type { get; set; } = "object";
        [JsonProperty("properties")]
        public Dictionary<string, FunctionProperty> Properties { get; set; }  // Properties of the parameters

        [JsonProperty("required")]
        public List<string> Required { get; set; }  // Required parameters
    }
}
