

using OpenAI.RealtimeApi.Dotnet.SDK.Core.Model.Response;
using System.Net.WebSockets;

namespace OpenAI.RealtimeApi.Dotnet.SDK.Core.Events
{
    public class WebSocketResponseEventArgs : EventArgs
    {
        public WebSocketResponseEventArgs(BaseResponse baseResponse, ClientWebSocket clientWebSocket) {
            BaseResponse = baseResponse;
            ClientWebSocket = clientWebSocket;
        }

        public BaseResponse BaseResponse { get; set; }

        public ClientWebSocket ClientWebSocket { get; }
    }
} 