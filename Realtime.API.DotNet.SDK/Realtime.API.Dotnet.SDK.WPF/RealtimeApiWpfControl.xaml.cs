using NAudio.Wave;
using Realtime.API.Dotnet.SDK.Core;
using Realtime.API.Dotnet.SDK.Core.Events;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Realtime.API.Dotnet.SDK.WPF
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class RealtimeApiWpfControl : UserControl
    {
        private const string apiKey = "";
        private VisualEffect voiceVisualEffect;
        private WaveInEvent waveIn; 
        private List<float> audioBuffer = new List<float>();

        public RealtimeApiWpfControl()
        {
            InitializeComponent();

            RealtimeApiSdk = new RealtimeApiSdk();
            RealtimeApiSdk.PlaybackAudioReceived += RealtimeApiSdk_PlaybackAudioReceived;

            RealtimeApiSdk.SpeechStarted += (s, e) =>
            {
                Dispatcher.Invoke(() =>
                {
                    HandleCycleVisualVoiceEffect(true);
                });
            };

            RealtimeApiSdk.SpeechEnded += (s, e) =>
            {
                Dispatcher.Invoke(() =>
                {
                    HandleCycleVisualVoiceEffect(false);
                });
            };

        }

        private void RealtimeApiSdk_PlaybackAudioReceived(object? sender, AudioEventArgs e)
        {
            if (e.Waveform != null)
            {
                Dispatcher.Invoke(() => DrawWaveform(e.Waveform));
            }
        }
        private void DrawWaveform(float[] waveform)
        {
            WaveCanvas.Children.Clear();

            double canvasWidth = WaveCanvas.ActualWidth;
            double canvasHeight = WaveCanvas.ActualHeight;

            if (canvasWidth == 0 || canvasHeight == 0 || waveform == null)
                return;

            Polyline polyline = new Polyline
            {
                Stroke = Brushes.LimeGreen,
                StrokeThickness = 2
            };

            double step = canvasWidth / waveform.Length;
            double centerY = canvasHeight / 2;

            for (int i = 0; i < waveform.Length; i++)
            {
                double x = i * step;
                double y = centerY - (waveform[i] * centerY);
                polyline.Points.Add(new Point(x, y));
            }

            WaveCanvas.Children.Add(polyline);
        }

        private void FadeOutWaveform()
        {
            if (WaveCanvas.Children.Count > 0)
            {
                var fadeOutAnimation = new DoubleAnimation
                {
                    From = 1.0,
                    To = 0.0,
                    Duration = TimeSpan.FromSeconds(1),
                    FillBehavior = FillBehavior.Stop
                };

                fadeOutAnimation.Completed += (s, e) =>
                {
                    WaveCanvas.Children.Clear();
                };

                foreach (var child in WaveCanvas.Children)
                {
                    if (child is UIElement element)
                    {
                        element.BeginAnimation(UIElement.OpacityProperty, fadeOutAnimation);
                    }
                }
            }
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //RealtimeApiSdk.TransactionOccurred += RealtimeApiSdk_TransactionOccurred;

            voiceVisualEffect = VoiceVisualEffect;
            RippleEffect.Visibility = Visibility.Hidden;
            WaveformContainer.Visibility = Visibility.Hidden;
        }
 
        public RealtimeApiSdk RealtimeApiSdk { get; private set; }

        public VisualEffect VoiceVisualEffect
        {
            get { return voiceVisualEffect; }
            set { voiceVisualEffect = value; }
        }

        public string OpenAiApiKey
        {
            get { return RealtimeApiSdk.ApiKey; }
            set { RealtimeApiSdk.ApiKey = value; }
        }

        private void RealtimeApiSdk_TransactionOccurred(object? sender, TransactionOccurredEventArgs e)
        {
            Console.WriteLine(e.Message);
            //Dispatcher.Invoke(() => chatMessages.Add(e.Message));
        }

        public void StartSpeechRecognition()
        {
            if (!RealtimeApiSdk.IsRunning)
            {
                // Start ripple effect.
                PlayVisualVoiceEffect(true);

                // Start voice recognition;
                RealtimeApiSdk.StartSpeechRecognitionAsync();
            }
        }


        public void StopSpeechRecognition()
        {
            if (RealtimeApiSdk.IsRunning)
            {
                // Stop the ripple effect.
                PlayVisualVoiceEffect(false);

                // Stop voice recognition;
                RealtimeApiSdk.StopSpeechRecognitionAsync();
            }
        }

        private void PlayVisualVoiceEffect(bool enable)
        {
            switch (voiceVisualEffect)
            {
                case WPF.VisualEffect.Cycle:
                    HandleCycleVisualVoiceEffect(enable);
                    break;
                case WPF.VisualEffect.SoundWave:
                    //HandleWaveVisualVoiceEffect(enable);
                    if (enable)
                        StartAudioCapture();
                    else
                        StopAudioCapture();
                    break;
                default:
                    break;
            }
        }


        private void HandleCycleVisualVoiceEffect(bool enable)
        {
            if (voiceVisualEffect == WPF.VisualEffect.SoundWave) 
            {
                return;
            }

            if (enable)
            {
                // Ensure the ripples are visible.
                RippleEffect.Visibility = Visibility.Visible;

                // Clear previous animations before each launch.
                RippleScale.BeginAnimation(ScaleTransform.ScaleXProperty, null);
                RippleScale.BeginAnimation(ScaleTransform.ScaleYProperty, null);

                // Create an animation object.
                DoubleAnimation scaleAnimation = new DoubleAnimation
                {
                    From = 1,
                    To = 3,
                    Duration = new Duration(TimeSpan.FromSeconds(1)),
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever
                };

                // Start animation.
                RippleScale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
                RippleScale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
            }
            else
            {
                RippleEffect.Visibility = Visibility.Collapsed;

                // Remove the animation.
                RippleScale.BeginAnimation(ScaleTransform.ScaleXProperty, null);
                RippleScale.BeginAnimation(ScaleTransform.ScaleYProperty, null);
            }
        }

        private void HandleWaveVisualVoiceEffect(bool enable)
        {
            if (enable)
            {
                WaveformContainer.Visibility = Visibility.Visible;
                foreach (var child in WaveformContainer.Children)
                {
                    if (child is Rectangle rect)
                    {
                        AnimateRectangleHeight(rect);
                    }
                }
            }
            else 
            {
                WaveformContainer.Visibility = Visibility.Collapsed;
                foreach (var child in WaveformContainer.Children)
                {
                    if (child is Rectangle rect)
                    {
                        rect.BeginAnimation(Rectangle.HeightProperty, null);
                    }
                }
            }
        }

        private void AnimateRectangleHeight(Rectangle rect)
        {
             Random _random = new Random();
            DoubleAnimation heightAnimation = new DoubleAnimation
            {
                From = 10,
                To = _random.Next(50, 150), 
                Duration = TimeSpan.FromMilliseconds(300),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };

            rect.BeginAnimation(Rectangle.HeightProperty, heightAnimation);
        }


        private void StartAudioCapture()
        {
            WaveCanvas.Visibility = Visibility.Visible;
            waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(44100, 1)
            };

            waveIn.DataAvailable += OnDataAvailable;
            waveIn.StartRecording();
        }

        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            for (int i = 0; i < e.BytesRecorded; i += 2)
            {
                short value = BitConverter.ToInt16(e.Buffer, i);
                float normalized = value / 32768f; 
                audioBuffer.Add(normalized);
            }

            Dispatcher.Invoke(() => DrawWaveform());
        }

        private void DrawWaveform()
        {
            WaveCanvas.Children.Clear();

            if (audioBuffer.Count == 0)
                return;

            double canvasWidth = WaveCanvas.ActualWidth;
            double canvasHeight = WaveCanvas.ActualHeight;

            double centerY = canvasHeight / 2;
            double step = canvasWidth / audioBuffer.Count;

            Polyline waveform = new Polyline
            {
                Stroke = Brushes.LimeGreen,
                StrokeThickness = 2
            };

            for (int i = 0; i < audioBuffer.Count; i++)
            {
                double x = i * step;
                double y = centerY - (audioBuffer[i] * centerY);
                waveform.Points.Add(new Point(x, y));
            }

            WaveCanvas.Children.Add(waveform);

            // 清空旧的音频缓冲，保留最近一部分数据
            if (audioBuffer.Count > 1000)
                audioBuffer.RemoveRange(0, audioBuffer.Count - 1000);
        }

        private void StopAudioCapture()
        {
            if (waveIn != null)
            {
                waveIn.StopRecording();
                waveIn.Dispose();
                waveIn = null; 
            }

            WaveformContainer.Visibility = Visibility.Hidden; 
            WaveCanvas.Children.Clear(); 
        }
    }
}
