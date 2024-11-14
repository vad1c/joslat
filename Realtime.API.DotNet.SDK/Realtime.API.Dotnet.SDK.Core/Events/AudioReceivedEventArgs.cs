using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtime.API.Dotnet.SDK.Core.Events
{
    public class AudioReceivedEventArgs : EventArgs
    {
        public byte[] AudioBuffer { get; private set; }

        public AudioReceivedEventArgs(byte[] audioBuffer)
        {
            AudioBuffer = audioBuffer;
        }
    }
}
