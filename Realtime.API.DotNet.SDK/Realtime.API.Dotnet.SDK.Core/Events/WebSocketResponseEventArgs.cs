

using Newtonsoft.Json.Linq;
using Realtime.API.Dotnet.SDK.Core;
using System.Net.WebSockets;

namespace Realtime.API.Dotnet.SDK
{
    public class WebSocketResponseEventArgs : EventArgs
    {
        public WebSocketResponseEventArgs(JObject responseJson, ClientWebSocket clientWebSocket)
        {
            ResponseJson = responseJson;
            ClientWebSocket = clientWebSocket;
        }

        public JObject ResponseJson { get; set; }

        public ClientWebSocket ClientWebSocket { get; }
    }
} 