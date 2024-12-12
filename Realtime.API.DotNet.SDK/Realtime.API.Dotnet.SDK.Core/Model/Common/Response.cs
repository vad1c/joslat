using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtime.API.Dotnet.SDK.Core.Model.Common
{
    public class Response
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("object")]
        public string Object { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("status_details")]
        public StatusDetails? StatusDetails { get; set; }
        [JsonProperty("output")]
        public List<Output> Output { get; set; }
        [JsonProperty("usage")]
        public Usage Usage { get; set; }
    }

    public class StatusDetails
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("error")]
        public Error Error { get; set; }
    }
    public class Error
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public class Output
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("object")]
        public string Object { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("role")]
        public string Role { get; set; }
        [JsonProperty("content")]
        public List<Content>  Content { get; set; }
    }
    
    public class Usage
    {
        [JsonProperty("total_tokens")]
        public int TotalTokens { get; set; }
        [JsonProperty("input_tokens")]
        public int InputTokens { get; set; }
        [JsonProperty("output_tokens")]
        public int OutputTokens { get; set; }
        [JsonProperty("input_token_details")]
        public InputTokenDetails InputTokenDetails { get; set; }
        [JsonProperty("output_token_details")]
        public OutputTokenDetails OutputTokenDetails { get; set; }
    }

    public class InputTokenDetails
    {
        [JsonProperty("cached_tokens")]
        public int CachedTokens { get; set; }
        [JsonProperty("text_tokens")]
        public int TextTokens { get; set; }
        [JsonProperty("audio_tokens")]
        public int AudioTokens { get; set; }
    }

    public class OutputTokenDetails
    {
        [JsonProperty("text_tokens")]
        public int TextTokens { get; set; }
        [JsonProperty("audio_tokens")]
        public int AudioTokens { get; set; }
    }
}
