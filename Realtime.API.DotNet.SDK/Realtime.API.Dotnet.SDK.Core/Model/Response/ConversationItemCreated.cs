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
    /// conversation.item.created
    /// </summary>
    public class ConversationItemCreated
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("previous_item_id")]
        public string PreviousItemId { get; set; }

        [JsonProperty("item")]
        public Item Item { get; set; }
    }
}
