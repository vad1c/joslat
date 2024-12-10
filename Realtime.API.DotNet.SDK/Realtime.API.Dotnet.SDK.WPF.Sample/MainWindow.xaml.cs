using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Realtime.API.Dotnet.SDK.Core;
using Realtime.API.Dotnet.SDK.Core.Model.Function;
using Realtime.API.Dotnet.SDK.Core.Model.Response;
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
        private static readonly ILog log = LogManager.GetLogger(typeof(MainWindow));
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
            //realtimeApiWpfControl.RealtimeApiSdk.TransactionOccurred += RealtimeApiSdk_TransactionOccurred;

            realtimeApiWpfControl.SpeechTextAvailable += RealtimeApiSdk_SpeechTextAvailable;
            realtimeApiWpfControl.PlaybackTextAvailable += RealtimeApiSdk_PlaybackTextAvailable;

            //realtimeApiWpfControl.VoiceVisualEffect = WPF.VisualEffect.Cycle;
            realtimeApiWpfControl.VoiceVisualEffect = WPF.VisualEffect.SoundWave;

            RegisterWeatherFunctionCall();
            RegisterNotepadFunctionCall();

            log.Info("App Start...");
        }

        private void RealtimeApiSdk_PlaybackTextAvailable(object? sender, Core.Events.TranscriptEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                ChatOutput.AppendText($"AI: {e.Transcript}\n"); // Display the received playback text
            });
        }

        private void RealtimeApiSdk_SpeechTextAvailable(object? sender, Core.Events.TranscriptEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                ChatOutput.AppendText($"User: {e.Transcript}"); // Display the received speech text
            });
        }

        // TODO delete
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
            BaseResponse resBase = e.BaseResponse;

            if (resBase is SessionCreated)
            {
                SessionCreated created = (SessionCreated)resBase;
                string s = created.EventId;
            }

            var type = e.ResponseJson["type"]?.ToString();
            switch (type)
            {
                //case "response.function_call_arguments.done":
                //    string functionName = e.ResponseJson["name"]?.ToString();
                //    switch (functionName)
                //    {
                //        case "get_weather":
                //            HandleWeatherFunctionCall(e.ResponseJson, e.ClientWebSocket);
                //            break;
                //        case "write_notepad":
                //            HandleNotepadFunctionCall(e.ResponseJson, e.ClientWebSocket);
                //            break;

                //        default:
                //            Console.WriteLine("Unknown function call received.");
                //            break;
                //    }
                //    break;
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
            var weatherFunctionCallSetting = new FunctionCallSetting
            {
                Name = "get_weather",
                Description = "Get current weather for a specified city",
                Parameter = new FunctionParameter
                {
                    Properties = new Dictionary<string, FunctionProperty>
                    {
                        {
                            "city", new FunctionProperty
                            {
                                Description = "The name of the city for which to fetch the weather."
                            }
                        }
                    },
                    Required = new List<string> { "city" }
                }
            };
            //string jsonString = JsonConvert.SerializeObject(weatherFunctionCall);
            //JObject jObject = JObject.Parse(jsonString);
            //realtimeApiWpfControl.RealtimeApiSdk.RegisterFunctionCall(jObject);

            realtimeApiWpfControl.RegisterFunctionCall(weatherFunctionCallSetting, HandleWeatherFunctionCall);
        }

        private void RegisterNotepadFunctionCall()
        {
            var notepadFunctionCallSetting = new FunctionCallSetting
            {
                Name = "write_notepad",
                Description = "Open a text editor and write the time, for example, 2024-10-29 16:19. Then, write the content, which should include my questions along with your answers.",
                Parameter = new FunctionParameter
                {
                    Properties = new Dictionary<string, FunctionProperty>
                    {
                        {
                            "content", new FunctionProperty
                            {
                                Description = "The content consists of my questions along with the answers you provide."
                            }
                        },
                        {
                            "date", new FunctionProperty
                            {
                                Description = "The time, for example, 2024-10-29 16:19."
                            }
                        }
                    },
                    Required = new List<string> { "content", "date" }
                }
            };
            //string jsonString = JsonConvert.SerializeObject(notepadFunctionCall);
            //JObject jObject = JObject.Parse(jsonString);

            //realtimeApiWpfControl.RealtimeApiSdk.RegisterFunctionCall(jObject);

            realtimeApiWpfControl.RegisterFunctionCall(notepadFunctionCallSetting, HandleNotepadFunctionCall);
        }


        #region Function Call

        private bool HandleWeatherFunctionCall(FuncationCallArgument argument, ClientWebSocket clientWebSocket)
        {
            try
            {
                var name = argument.Name;
                var arguments = argument.Arguments;
                if (!string.IsNullOrEmpty(arguments))
                {
                    var weatherArgument = JsonConvert.DeserializeObject<WeatherArgument>(arguments);
                    var city = weatherArgument?.City;
                    if (!string.IsNullOrEmpty(city))
                    {
                        var weatherResult = GetWeatherFake(city);
                        SendFunctionCallResult(weatherResult, argument.CallId, clientWebSocket);
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

            return true;
        }

        private bool HandleNotepadFunctionCall(FuncationCallArgument argument, ClientWebSocket clientWebSocket)
        {
            try
            {
                var name = argument.Name;
                var callId = argument.CallId;
                var arguments = argument.Arguments;
                if (!string.IsNullOrEmpty(arguments))
                {
                    var noteArgument = JsonConvert.DeserializeObject<NoteArgument>(arguments);
                    var content = noteArgument?.Content;
                    var date = noteArgument?.Date;
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
            return true;
        }

        private string GetWeatherFake(string city)
        {
            var weatherResponse = new WeatherResponse
            {
                City = city,
                Temperature = "30°C"
            };

            return JsonConvert.SerializeObject(weatherResponse);
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
            var functionCallResult = new FunctionCallResult
            {
                Type = "conversation.item.create",
                Item = new FunctionCallItem
                {
                    Type = "function_call_output",
                    Output = result,
                    CallId = callId
                }
            };

            string resultJsonString = JsonConvert.SerializeObject(functionCallResult);

            webSocketClient.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(resultJsonString)), WebSocketMessageType.Text, true, CancellationToken.None);
            Console.WriteLine("Sent function call result: " + resultJsonString);

            // TODO create model
            var responseJson = new ResponseJson
            {
                Type = "response.create"
            };
            string rpJsonString = JsonConvert.SerializeObject(responseJson);

            webSocketClient.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(rpJsonString)), WebSocketMessageType.Text, true, CancellationToken.None);
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