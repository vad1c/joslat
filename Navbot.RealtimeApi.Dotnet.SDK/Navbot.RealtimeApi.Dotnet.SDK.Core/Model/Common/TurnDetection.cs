using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navbot.RealtimeApi.Dotnet.SDK.Core.Model.Common
{
    public class TurnDetection
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("threshold")]
        public double Threshold { get; set; }

        [JsonProperty("prefix_padding_ms")]
        public int PrefixPaddingMs { get; set; }

        [JsonProperty("silence_duration_ms")]
        public int SilenceDurationMs { get; set; }
    }
}
