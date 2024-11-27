using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Realtime.API.Dotnet.SDK.Core.Model
{
    public class SessionUpdateRequest
    {
        public string Type { get; set; }
        public Session Session { get; set; }
    }

    public class Session
    {
        public string Instructions { get; set; }
        public TurnDetection TurnDetection { get; set; }
        public string Voice { get; set; }
        public double Temperature { get; set; }
        public int MaxResponseOutputTokens { get; set; }
        public List<string> Modalities { get; set; }
        public string InputAudioFormat { get; set; }
        public string OutputAudioFormat { get; set; }
        public AudioTranscription InputAudioTranscription { get; set; }
        public string ToolChoice { get; set; }
        public JArray Tools { get; set; }
    }

    public class TurnDetection
    {
        public string Type { get; set; }
        public double Threshold { get; set; }
        public int PrefixPaddingMs { get; set; }
        public int SilenceDurationMs { get; set; }
    }

    public class AudioTranscription
    {
        public string Model { get; set; }
    }
}
