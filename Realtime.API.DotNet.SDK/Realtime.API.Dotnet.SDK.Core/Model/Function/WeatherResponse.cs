using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtime.API.Dotnet.SDK.Core.Model.Function
{
    public class WeatherResponse
    {
        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("temperature")]
        public string Temperature { get; set; }
    }
}
