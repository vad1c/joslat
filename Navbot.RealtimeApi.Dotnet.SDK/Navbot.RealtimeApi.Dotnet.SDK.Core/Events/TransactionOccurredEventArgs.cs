

using Newtonsoft.Json.Linq;

namespace Navbot.RealtimeApi.Dotnet.SDK.Core.Events
{
    public class TransactionOccurredEventArgs : EventArgs
    {
        public TransactionOccurredEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}
