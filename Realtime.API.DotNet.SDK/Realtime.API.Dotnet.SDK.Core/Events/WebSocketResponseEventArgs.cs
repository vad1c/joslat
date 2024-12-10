﻿

using Newtonsoft.Json.Linq;
using Realtime.API.Dotnet.SDK.Core;
using Realtime.API.Dotnet.SDK.Core.Model.Response;
using System.Net.WebSockets;

namespace Realtime.API.Dotnet.SDK
{
    public class WebSocketResponseEventArgs : EventArgs
    {
        public WebSocketResponseEventArgs(BaseResponse baseResponse, ClientWebSocket clientWebSocket) {
            BaseResponse = baseResponse;
            ClientWebSocket = clientWebSocket;
        }

        // TODO remove this
        public WebSocketResponseEventArgs(JObject responseJson, ClientWebSocket clientWebSocket)
        {
            ResponseJson = responseJson;
            ClientWebSocket = clientWebSocket;
        }

        public BaseResponse BaseResponse { get; set; }

        public JObject ResponseJson { get; set; }

        public ClientWebSocket ClientWebSocket { get; }
    }
} 