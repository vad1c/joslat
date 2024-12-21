using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navbot.RealtimeApi.Dotnet.SDK.Core.Model.Response
{
    /// <summary>
    /// input_audio_buffer.committed
    /// </summary>
    public class BufferCommitted : BaseResponse
    {
        [JsonProperty("previous_item_id")]
        public string PreviousItemId { get; set; }

        [JsonProperty("item_id")]
        public string ItemId { get; set; }
    }
}
