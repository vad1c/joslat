using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Realtime.API.Dotnet.SDK.Core.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtime.API.Dotnet.SDK.Core.Model.Response
{
    /// <summary>
    /// session.updated
    /// </summary>
    public class SessionUpdate
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("session")]
        public Session Session { get; set; }
    }
}
