using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtime.API.Dotnet.SDK.Core.Model.Response
{
    /// <summary>
    /// conversation.item.truncate
    /// </summary>
    public class ConversationItemTruncate : BaseResponse
    {
        [JsonProperty("item_id")]
        public string ItemId { get; set; }

        [JsonProperty("content_index")]
        public string ContentIndex { get; set; }

        [JsonProperty("audio_end_ms")]
        public string AudioEndMs { get; set; }
    }
}
