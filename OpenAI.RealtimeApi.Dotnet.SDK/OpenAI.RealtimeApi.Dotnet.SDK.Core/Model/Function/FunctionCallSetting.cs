using Newtonsoft.Json;
using OpenAI.RealtimeApi.Dotnet.SDK.Core.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.RealtimeApi.Dotnet.SDK.Core.Model.Function
{
    public class FunctionCallSetting
    {
        [JsonProperty("type")]
        public string Type { get; set; } = "function";

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("parameters")]
        public FunctionParameter Parameter { get; set; }  // Function Parameters
    }
}
