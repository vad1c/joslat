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
        public static BaseResponse Parse(JObject json)
        {
            BaseResponse baseResponse = null;
            var type = json["type"]?.ToString();

            var typeMapping = new Dictionary<string, Func<JObject, BaseResponse>>
            {
                { "session.created", j => j.ToObject<SessionCreated>() },
                { "session.updated", j => j.ToObject<SessionUpdate>() },
                { "input_audio_buffer.speech_started", j => j.ToObject<SpeechStarted>() },
                { "input_audio_buffer.speech_stopped", j => j.ToObject<SpeechStopped>() },
                { "conversation.item.input_audio_transcription.completed", j => j.ToObject<TranscriptionCompleted>() },
                { "response.audio_transcript.done", j => j.ToObject<ResponseAudioTranscriptDone>() },
                { "conversation.item.created", j => j.ToObject<ConversationItemCreated>() },
                { "input_audio_buffer.committed", j => j.ToObject<BufferCommitted>() },
                { "response.created", j => j.ToObject<ResponseCreated>() },
                { "response.function_call_arguments.done", j => j.ToObject<FuncationCallArgument>() }
            };

            if (typeMapping.ContainsKey(type))
            {
                baseResponse = typeMapping[type](json);
            }
            else if (type == "response.audio_transcript.delta" || type == "response.audio.delta" || type == "response.audio.done")
            {
                var responseDelta = json.ToObject<ResponseDelta>();

                if (type == "response.audio_transcript.delta")
                    responseDelta.ResponseDeltaType = ResponseDeltaType.AudioTranscriptDelta;
                else if (type == "response.audio.delta")
                    responseDelta.ResponseDeltaType = ResponseDeltaType.AudioDelta;
                else if (type == "response.audio.done")
                    responseDelta.ResponseDeltaType = ResponseDeltaType.AudioDone;

                baseResponse = responseDelta;
            }

            return baseResponse;
        }
    }
}
