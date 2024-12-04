

using Newtonsoft.Json.Linq;

namespace Realtime.API.Dotnet.SDK
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
