using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navbot.RealtimeApi.Dotnet.SDK.Core.Events
{
    public class TranscriptEventArgs : EventArgs
    {
        public string Transcript { get; private set; }

        public TranscriptEventArgs(string transcript)
        {
            Transcript = transcript;
        }

    }
}
