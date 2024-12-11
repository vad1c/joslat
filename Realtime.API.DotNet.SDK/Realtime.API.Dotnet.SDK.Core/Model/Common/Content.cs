using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtime.API.Dotnet.SDK.Core.Model.Common
{
    public class Content
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("transcript")]
        public string Transcript { get; set; }

    }
}
