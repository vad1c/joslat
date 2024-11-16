using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtime.API.Dotnet.SDK.Core.Events
{
    public class AudioEventArgs : EventArgs
    {
        public byte[] AudioBuffer { get; private set; }

        public AudioEventArgs()
            : this(new byte[0])
        {
        }

        public AudioEventArgs(byte[] audioBuffer)
        {
            AudioBuffer = audioBuffer;
        }
    }
}
