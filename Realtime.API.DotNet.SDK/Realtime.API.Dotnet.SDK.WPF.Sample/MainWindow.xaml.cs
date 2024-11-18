using Newtonsoft.Json.Linq;
using Realtime.API.Dotnet.SDK.Core;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Realtime.API.Dotnet.SDK.WPF.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<string> chatMessages = new ObservableCollection<string>();
        private bool isPlaying = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string openAiApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? "";

            realtimeApiWpfControl.OpenAiApiKey = openAiApiKey;
            realtimeApiWpfControl.RealtimeApiSdk.WebSocketResponse += RealtimeApiSdk_WebSocketResponse;
            realtimeApiWpfControl.RealtimeApiSdk.TransactionOccurred += RealtimeApiSdk_TransactionOccurred;

            realtimeApiWpfControl.VoiceVisualEffect = WPF.VisualEffect.Cycle;
            //realtimeApiWpfControl.VoiceVisualEffect = WPF.VisualEffect.SoundWave;

            RegisterWeatherFunctionCall();
            RegisterNotepadFunctionCall();
        }

        private void RealtimeApiSdk_TransactionOccurred(object? sender, TransactionOccurredEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        private void StartSpeechRecognition_Click(object sender, RoutedEventArgs e)
        {
            realtimeApiWpfControl.StartSpeechRecognition();
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            var playIcon = (System.Windows.Shapes.Path)PlayPauseButton.Template.FindName("PlayIcon", PlayPauseButton);
            var pauseIcon = (System.Windows.Shapes.Path)PlayPauseButton.Template.FindName("PauseIcon", PlayPauseButton);

            if (isPlaying)
            {
                playIcon.Visibility = Visibility.Visible;
                pauseIcon.Visibility = Visibility.Collapsed;

                realtimeApiWpfControl.StopSpeechRecognition();
            }
            else
            {
                playIcon.Visibility = Visibility.Collapsed;
                pauseIcon.Visibility = Visibility.Visible;

                realtimeApiWpfControl.StartSpeechRecognition();
            }

            isPlaying = !isPlaying;
        }


        private void RealtimeApiSdk_WebSocketResponse(object? sender, WebSocketResponseEventArgs e)
        {
            var type = e.ResponseJson["type"]?.ToString();
            switch (type)
            {
                case "response.function_call_arguments.done":
                    string functionName = e.ResponseJson["name"]?.ToString();
                    switch (functionName)
                    {
                        case "get_weather":
                            HandleWeatherFunctionCall(e.ResponseJson, e.ClientWebSocket);
                            break;
                        case "write_notepad":
                            HandleNotepadFunctionCall(e.ResponseJson, e.ClientWebSocket);
                            break;

                        default:
                            Console.WriteLine("Unknown function call received.");
                            break;
                    }
                    break;
                //case "conversation.item.input_audio_transcription.completed":
                //    var text = e.ResponseJson["transcript"]?.ToString();

                //    WriteToTextFile(text);
                //    break;
                default:
                    Console.WriteLine("Unhandled command type");
                    break;
            }
        }

        private void RegisterWeatherFunctionCall()
        {
            JObject weatherFunctionCallSettings = new JObject
            {
                ["type"] = "function",
                ["name"] = "get_weather",
                ["description"] = "Get current weather for a specified city",
                ["parameters"] = new JObject
                {
                    ["type"] = "object",
                    ["properties"] = new JObject
                    {
                        ["city"] = new JObject
                        {
                            ["type"] = "string",
                            ["description"] = "The name of the city for which to fetch the weather."
                        }
                    },
                    ["required"] = new JArray("city")
                }
            };

            realtimeApiWpfControl.RealtimeApiSdk.RegisterFunctionCall(weatherFunctionCallSettings);
        }

        private void RegisterNotepadFunctionCall()
        {
            JObject notepadFunctionCallSettings = new JObject
            {
                ["type"] = "function",
                ["name"] = "write_notepad",
                ["description"] = "Open a text editor and write the time, for example, 2024-10-29 16:19. Then, write the content, which should include my questions along with your answers.",
                ["parameters"] = new JObject
                {
                    ["type"] = "object",
                    ["properties"] = new JObject
                    {
                        ["content"] = new JObject
                        {
                            ["type"] = "string",
                            ["description"] = "The content consists of my questions along with the answers you provide."
                        },
                        ["date"] = new JObject
                        {
                            ["type"] = "string",
                            ["description"] = "the time, for example, 2024-10-29 16:19."
                        },
                    },
                    ["required"] = new JArray("content", "date")
                }
            };

            realtimeApiWpfControl.RealtimeApiSdk.RegisterFunctionCall(notepadFunctionCallSettings);
        }

        #region Function Call

        private void HandleWeatherFunctionCall(JObject json, ClientWebSocket clientWebSocket)
        {
            try
            {
                var name = json["name"]?.ToString();
                var callId = json["call_id"]?.ToString();
                var arguments = json["arguments"]?.ToString();
                if (!string.IsNullOrEmpty(arguments))
                {
                    var functionCallArgs = JObject.Parse(arguments);

                    var city = functionCallArgs["city"]?.ToString();
                    if (!string.IsNullOrEmpty(city))
                    {
                        var weatherResult = GetWeatherFake(city);
                        SendFunctionCallResult(weatherResult, callId, clientWebSocket);
                    }
                    else
                    {
                        Console.WriteLine("City not provided for get_weather function.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid arguments.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing function call arguments: {ex.Message}");
            }
        }

        private void HandleNotepadFunctionCall(JObject json, ClientWebSocket clientWebSocket)
        {
            try
            {
                var name = json["name"]?.ToString();
                var callId = json["call_id"]?.ToString();
                var arguments = json["arguments"]?.ToString();
                if (!string.IsNullOrEmpty(arguments))
                {
                    var functionCallArgs = JObject.Parse(arguments);
                    var content = functionCallArgs["content"]?.ToString();
                    var date = functionCallArgs["date"]?.ToString();
                    if (!string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(date))
                    {
                        WriteToNotepad(date, content);
                        SendFunctionCallResult("Write to notepad successful.", callId, clientWebSocket);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid arguments.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing function call arguments: {ex.Message}");
            }
        }

        private string GetWeatherFake(string city)
        {
            return $@"{{
                ""city"": ""{city}"",
                ""temperature"": ""30°C""
            }}";
        }

        private void WriteToNotepad(string date, string content)
        {
            try
            {
                string filePath = System.IO.Path.Combine(Environment.CurrentDirectory, "temp.txt");

                // Write the date and content to a text file
                File.AppendAllText(filePath, $"Date: {date}\nContent: {content}\n\n");

                // Open the text file in Notepad
                Process.Start("notepad.exe", filePath);
                Console.WriteLine("Content written to Notepad.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to Notepad: {ex.Message}");
            }
        }

        private void SendFunctionCallResult(string result, string callId, ClientWebSocket webSocketClient)
        {
            var resultJson = new JObject
            {
                ["type"] = "conversation.item.create",
                ["item"] = new JObject
                {
                    ["type"] = "function_call_output",
                    ["output"] = result,
                    ["call_id"] = callId
                }
            };

            webSocketClient.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(resultJson.ToString())), WebSocketMessageType.Text, true, CancellationToken.None);
            Console.WriteLine("Sent function call result: " + resultJson);

            var rpJson = new JObject
            {
                ["type"] = "response.create"
            };

            webSocketClient.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(rpJson.ToString())), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private void WriteToTextFile(string text)
        {
            var filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Transcription.txt");
            File.AppendAllText(filePath, text + Environment.NewLine);
            Console.WriteLine($"Text written to {filePath}");
        }
        #endregion


    }
}