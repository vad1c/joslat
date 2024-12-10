using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtime.API.Dotnet.SDK.Core.Model.Response
{
    public class BaseResponse
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("event_id")]
        public string EventId { get; set; }

        //TODO2 add all events from openai
        public static BaseResponse Parse(JObject json) {

            BaseResponse rtn = null;
            var type = json["type"]?.ToString();
            switch (type)
            {
                case "session.created":
                    rtn =  json.ToObject<SessionCreated>();
                    break;
                default:
                    break;
            }


            return rtn;
        }
    }
}
