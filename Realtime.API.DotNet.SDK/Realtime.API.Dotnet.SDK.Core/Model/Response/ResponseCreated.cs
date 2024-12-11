using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtime.API.Dotnet.SDK.Core.Model.Response
{
    public class ResponseCreated : BaseResponse
    {
        [JsonProperty("response")]
        public Common.Response Response { get; set; }
    }
    
}
