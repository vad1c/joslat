using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navbot.RealtimeApi.Dotnet.SDK.Core.Model.Response
{
    /// <summary>
    /// rate_limits.updated
    /// </summary>
    public class RateLimitsUpdated : BaseResponse
    {
        [JsonProperty("rate_limits")]
        public List<RateLimit> RateLimits { get; set; }
    }

    public class RateLimit 
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("limit")]
        public int Limit { get; set; }
        [JsonProperty("remaining")]
        public int Remaining { get; set; }
        [JsonProperty("reset_seconds")]
        public int ResetSeconds { get; set; }
    }
}
