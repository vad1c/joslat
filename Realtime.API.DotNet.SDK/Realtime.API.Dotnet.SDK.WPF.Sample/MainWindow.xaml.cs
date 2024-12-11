using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Realtime.API.Dotnet.SDK.Core;
using Realtime.API.Dotnet.SDK.Core.Model.Function;
using Realtime.API.Dotnet.SDK.Core.Model.Request;
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

            RegisterWeatherFunctionCall();
            RegisterNotepadFunctionCall();

            log.Info("App Start...");
        }

        private void realtimeApiWpfControl_SpeechTextAvailable(object sender, Core.Events.TranscriptEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                ChatOutput.AppendText($"User: {e.Transcript}"); // Display the received speech text
            });

        }

        private void realtimeApiWpfControl_PlaybackTextAvailable(object sender, Core.Events.TranscriptEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                ChatOutput.AppendText($"AI: {e.Transcript}\n"); // Display the received playback text
            });

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

            FucationCall fucationCall = new FucationCall();
            realtimeApiWpfControl.RegisterFunctionCall(weatherFunctionCallSetting, fucationCall.HandleWeatherFunctionCall);
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

            FucationCall fucationCall = new FucationCall();
            realtimeApiWpfControl.RegisterFunctionCall(notepadFunctionCallSetting, fucationCall.HandleNotepadFunctionCall);
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

      
     
    }
}