using log4net;
using Navbot.RealtimeApi.Dotnet.SDK.Core.Model.Function;
using System.Windows;
using System.Windows.Input;

namespace Navbot.RealtimeApi.Dotnet.SDK.WPF.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MainWindow));
        private bool isRecording = false;
        private bool isMuted = true;
            
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string openAiApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? "";

            realtimeApiWpfControl.OpenAiApiKey = openAiApiKey;

            // Register FunctionCall for weather
            realtimeApiWpfControl.RegisterFunctionCall(new FunctionCallSetting
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
            }, FunctionCallHelper.HandleWeatherFunctionCall);

            // Register FunctionCall for run application
            realtimeApiWpfControl.RegisterFunctionCall(new FunctionCallSetting
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
            }, FunctionCallHelper.HandleNotepadFunctionCall);

            log.Info("App Start...");
        }

        /// <summary>
        /// Start / Stop Speech Recognition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartStopRecognition_Click(object sender, RoutedEventArgs e)
        {
            var playIcon = (System.Windows.Shapes.Path)PlayPauseButton.Template.FindName("PlayIcon", PlayPauseButton);
            var pauseIcon = (System.Windows.Shapes.Path)PlayPauseButton.Template.FindName("PauseIcon", PlayPauseButton);

            if (isRecording)
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

            isRecording = !isRecording;
        }

        #region Talking Mode
        private void PressToTalkButton_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            EnableTalkingMode();
        }

        private void PressToTalkButton_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DisableTalkingModeWithDelay();
        }

        private CancellationTokenSource muteDelayCancellationTokenSource;
        private int millisecondsDelay = 1000;

        private async void DisableTalkingModeWithDelay()
        {
            // Cancel any previous mute delay if the button is pressed again quickly
            muteDelayCancellationTokenSource?.Cancel();
            muteDelayCancellationTokenSource = new CancellationTokenSource();

            try
            {
                // Introduce a delay before muting
                await Task.Delay(millisecondsDelay, muteDelayCancellationTokenSource.Token); // 500ms delay

                // Perform mute logic only if the task wasn't canceled
                DisableTalkingMode();
            }
            catch (TaskCanceledException)
            {
                // Ignore if the delay was canceled
            }
        }

        private void EnableTalkingMode()
        {
            muteDelayCancellationTokenSource?.Cancel(); // Cancel any pending mute
            var muteCrossIcon = (System.Windows.Shapes.Path)PressToTalkButton.Template.FindName("MuteCrossIcon", PressToTalkButton);
            isMuted = false;
            muteCrossIcon.Visibility = Visibility.Collapsed;

            // Unmute microphone
            realtimeApiWpfControl.RealtimeApiSdk.IsMuted = isMuted;
            log.Info("Microphone unmuted");
        }

        private void DisableTalkingMode()
        {
            var muteCrossIcon = (System.Windows.Shapes.Path)PressToTalkButton.Template.FindName("MuteCrossIcon", PressToTalkButton);
            isMuted = true;
            muteCrossIcon.Visibility = Visibility.Visible;

            // Mute microphone
            realtimeApiWpfControl.RealtimeApiSdk.IsMuted = isMuted;
            log.Info("Microphone muted");
        }

        #endregion Talking Mode



        /// <summary>
        /// Hook up SpeechTextAvailable event to display speech text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void realtimeApiWpfControl_SpeechTextAvailable(object sender, Core.Events.TranscriptEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                ChatOutput.AppendText($"User: {e.Transcript}"); // Display the received speech text
            });
        }

        /// <summary>
        /// Hook up PlaybackTextAvailable evnet to display OpenAI response.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void realtimeApiWpfControl_PlaybackTextAvailable(object sender, Core.Events.TranscriptEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                ChatOutput.AppendText($"AI: {e.Transcript}\n"); // Display the received playback text
            });
        }

    }
}