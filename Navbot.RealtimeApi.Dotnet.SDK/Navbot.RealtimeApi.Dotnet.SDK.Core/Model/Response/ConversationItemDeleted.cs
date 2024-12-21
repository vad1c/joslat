using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navbot.RealtimeApi.Dotnet.SDK.Core.Model.Response
{
    /// <summary>
    /// conversation.item.deleted
    /// </summary>
    public class ConversationItemDeleted : BaseResponse
    {
        [JsonProperty("item_id")]
        public string ItemId { get; set; }
    }
}
