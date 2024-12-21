using Newtonsoft.Json;
using Navbot.RealtimeApi.Dotnet.SDK.Core.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navbot.RealtimeApi.Dotnet.SDK.Core.Model.Response
{
    /// <summary>
    /// session.created
    /// </summary>
    public class SessionCreated : BaseResponse
    {
        public SessionCreated() {
            base.Type = "session.created";
        }

        [JsonProperty("session")]
        public Session Session { get; set; }
    }

    
}
