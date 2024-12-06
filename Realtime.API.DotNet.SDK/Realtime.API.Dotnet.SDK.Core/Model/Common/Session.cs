using Newtonsoft.Json;
using Realtime.API.Dotnet.SDK.Core.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtime.API.Dotnet.SDK.Core.Model.Common
{
    public class Session
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("expires_at")]
        public long ExpiresAt { get; set; }

        [JsonProperty("modalities")]
        public List<string> Modalities { get; set; }

        [JsonProperty("instructions")]
        public string Instructions { get; set; }

        [JsonProperty("voice")]
        public string Voice { get; set; }

        [JsonProperty("turn_detection")]
        public TurnDetection TurnDetection { get; set; }

        [JsonProperty("input_audio_format")]
        public string InputAudioFormat { get; set; }

        [JsonProperty("output_audio_format")]
        public string OutputAudioFormat { get; set; }

        [JsonProperty("input_audio_transcription")]
        public AudioTranscription InputAudioTranscription { get; set; }

        [JsonProperty("tool_choice")]
        public string ToolChoice { get; set; }

        [JsonProperty("temperature")]
        public double Temperature { get; set; }

        [JsonProperty("max_response_output_tokens")]
        public string MaxResponseOutputTokens { get; set; }

        [JsonProperty("tools")]
        public List<Tool> Tools { get; set; }
    }

    public class AudioTranscription
    {
        [JsonProperty("model")]
        public string Model { get; set; }
    }

    public class Tool
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("parameters")]
        public ToolParameters Parameters { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class ToolParameters
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("properties")]
        public Dictionary<string, FunctionProperty> properties { get; set; }

        [JsonProperty("required")]
        public List<string> Required { get; set; }
    }

    
}
