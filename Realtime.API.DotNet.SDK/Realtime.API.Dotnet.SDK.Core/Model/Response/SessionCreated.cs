using Newtonsoft.Json;
using Realtime.API.Dotnet.SDK.Core.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtime.API.Dotnet.SDK.Core.Model.Response
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
