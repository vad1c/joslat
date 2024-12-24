using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navbot.RealtimeApi.Dotnet.SDK.Core.Model.Response
{
    /// <summary>
    /// conversation.item.created
    /// </summary>
    public class ConversationItemCreated : BaseResponse
    {
        [JsonProperty("previous_item_id")]
        public string PreviousItemId { get; set; }

        [JsonProperty("item")]
        public Item Item { get; set; }
    }
    public class Item
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("role")]
        public string Role { get; set; }
        [JsonProperty("content")]
        public List<Content> Content { get; set; }
    }
    public class Content
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("transcript")]
        public string Transcript { get; set; }
        [JsonProperty("audio")]
        public string Audio { get; set; }
    }
}
