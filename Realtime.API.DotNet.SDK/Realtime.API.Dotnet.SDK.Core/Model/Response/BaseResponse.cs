using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtime.API.Dotnet.SDK.Core.Model.Response
{
    public class BaseResponse
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("event_id")]
        public string EventId { get; set; }

        //TODO2 add all events from openai
        public static BaseResponse Parse(JObject json) {

            BaseResponse baseResponse = null;
            var type = json["type"]?.ToString();
            switch (type)
            {
                case "session.created":
                    baseResponse =  json.ToObject<SessionCreated>();
                    break;
                case "session.updated":
                    baseResponse = json.ToObject<SessionUpdate>();
                    break;
                case "input_audio_buffer.speech_started":
                    baseResponse = json.ToObject<SpeechStarted>();
                    break;
                case "input_audio_buffer.speech_stopped":
                    baseResponse = json.ToObject<SpeechStopped>();
                    break;
                case "response.audio_transcript.delta":
                    baseResponse = json.ToObject<ResponseDelta>();
                    break;
                case "conversation.item.input_audio_transcription.completed":
                    baseResponse = json.ToObject<TranscriptionCompleted>();
                    break;
                case "response.audio_transcript.done":
                    baseResponse = json.ToObject<ResponseAudioTranscriptDone>();
                    break;
                case "response.audio.delta":
                    baseResponse = json.ToObject<ResponseDelta>();
                    break;
                case "response.audio.done":
                    baseResponse = json.ToObject<ResponseDelta>();
                    break;
                case "conversation.item.created":
                    baseResponse = json.ToObject<ConversationItemCreated>();
                    break;
                case "input_audio_buffer.committed":
                    baseResponse = json.ToObject<BufferCommitted>();
                    break;
                case "response.created":
                    baseResponse = json.ToObject<ResponseCreated>();
                    break;
                case "response.function_call_arguments.done":
                    baseResponse = json.ToObject<FuncationCallArgument>();
                    break;
                default:
                    break;
            }

            return baseResponse;
        }
    }
}
