using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtime.API.Dotnet.SDK.Core.Events
{
    public class AudioSentEventArgs : EventArgs
    {
        public byte[] AudioBuffer { get; private set; }

        public AudioSentEventArgs(byte[] audioBuffer)
        {
            AudioBuffer = audioBuffer;
        }
    }
}
