using NAudio.Wave;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Realtime.API.Dotnet.SDK.Core;
using Realtime.API.Dotnet.SDK.Core.Events;
using Realtime.API.Dotnet.SDK.Core.Model.Function;
using Realtime.API.Dotnet.SDK.Core.Model.Response;
using System;
using System.ComponentModel;
using System.IO;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
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

        //TODO2 Move into Api Sdk
        private WaveInEvent speechWaveIn;

        // TODO2
        private WasapiLoopbackCapture speakerCapture;
        private BufferedWaveProvider speakerWaveProvider;

        public event EventHandler<WaveInEventArgs> WaveInDataAvailable;
        public event EventHandler<WebSocketResponseEventArgs> WebSocketResponse;

        public event EventHandler<EventArgs> SpeechStarted;
        public event EventHandler<AudioEventArgs> SpeechDataAvailable;
        public event EventHandler<TranscriptEventArgs> SpeechTextAvailable;
        public event EventHandler<AudioEventArgs> SpeechEnded;

        public event EventHandler<EventArgs> PlaybackStarted;
        public event EventHandler<AudioEventArgs> PlaybackDataAvailable;
        public event EventHandler<TranscriptEventArgs> PlaybackTextAvailable;
        public event EventHandler<EventArgs> PlaybackEnded;

        public RealtimeApiWpfControl()
        {
            InitializeComponent();
            
            RealtimeApiSdk = new RealtimeApiSdk();
            Loaded += RealtimeApiWpfControl_Loaded;
            WaveCanvas.SizeChanged += WaveCanvas_SizeChanged;
        }

        public RealtimeApiSdk RealtimeApiSdk { get; private set; }
        public string OpenAiApiKey
        {
            get { return RealtimeApiSdk.ApiKey; }
            set { RealtimeApiSdk.ApiKey = value; }
        }

        public VisualEffect VoiceVisualEffect
        {
            get { return voiceVisualEffect; }
            set { voiceVisualEffect = value; }
        }

        private void RealtimeApiWpfControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Parent is FrameworkElement parent)
            {
                parent.SizeChanged += Parent_SizeChanged;
                UpdateControlSize(parent);
            }
        }

        private void Parent_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is FrameworkElement parent)
            {
                UpdateControlSize(parent);
            }
        }

        private void UpdateControlSize(FrameworkElement parent)
        {
            this.Width = parent.ActualWidth;
            this.Height = parent.ActualHeight;
            this.UpdateLayout();
        }

        protected virtual void OnPlaybackDataAvailable(AudioEventArgs e)
        {
            PlaybackDataAvailable?.Invoke(this, e);
        }

        private void RealtimeApiSdk_PlaybackEnded(object? sender, EventArgs e)
        {
            OnPlaybackEnded(e);
        }

        protected virtual void OnPlaybackEnded(EventArgs e)
        {
            PlaybackEnded?.Invoke(this, e);
        }

        private void RealtimeApiSdk_PlaybackStarted(object? sender, EventArgs e)
        {
            OnPlaybackStarted(e);
        }

        protected virtual void OnPlaybackStarted(EventArgs e)
        {
            PlaybackStarted?.Invoke(this, e);
        }

        private void RealtimeApiSdk_PlaybackTextAvailable(object? sender, TranscriptEventArgs e)
        {
            OnPlaybackTextAvailable(e);
        }

        protected virtual void OnPlaybackTextAvailable(TranscriptEventArgs e)
        {
            PlaybackTextAvailable?.Invoke(this, e);
        }

        private void RealtimeApiSdk_SpeechEnded(object? sender, AudioEventArgs e)
        {
            OnSpeechEnded(e);
        }

        protected virtual void OnSpeechEnded(AudioEventArgs e)
        {
            SpeechEnded?.Invoke(this, e);
        }

        private void RealtimeApiSdk_SpeechStarted(object? sender, EventArgs e)
        {
            OnSpeechStarted(e);
        }

        protected virtual void OnSpeechStarted(EventArgs e)
        {
            SpeechStarted?.Invoke(this, e);
        }

        private void RealtimeApiSdk_SpeechTextAvailable(object? sender, TranscriptEventArgs e)
        {
            OnSpeechTextAvailable(e);
        }

        protected virtual void OnSpeechTextAvailable(TranscriptEventArgs e)
        {
            SpeechTextAvailable?.Invoke(this, e);
        }

        private void RealtimeApiSdk_SpeechDataAvailable(object? sender, AudioEventArgs e)
        {
            OnSpeechDataAvailable(e);
        }

        protected virtual void OnSpeechDataAvailable(AudioEventArgs e)
        {
            SpeechDataAvailable?.Invoke(this, e);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // TODO2 connect to sdk event
            speechWaveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(44100, 1)
            };

            speechWaveIn.DataAvailable += SpeechWaveIn_DataAvailable;

            //RealtimeApiSdk.WaveInDataAvailable += RealtimeApiSdk_WaveInDataAvailable;
            //RealtimeApiSdk.WaveInDataAvailable += SpeechWaveIn_DataAvailable;

            speakerCapture = new WasapiLoopbackCapture();
            speakerWaveProvider = new BufferedWaveProvider(speakerCapture.WaveFormat)
            {
                BufferLength = 1024 * 1024, // 1 MB buffer (adjust based on your needs)
                DiscardOnBufferOverflow = true // Optional: discard data when buffer is full}
            };

            speakerCapture.DataAvailable += SpeakerCapture_DataAvailable;
            RealtimeApiSdk.WebSocketResponse += RealtimeApiSdk_WebSocketResponse;

            RealtimeApiSdk.SpeechStarted += RealtimeApiSdk_SpeechStarted;
            RealtimeApiSdk.SpeechDataAvailable += RealtimeApiSdk_SpeechDataAvailable;
            RealtimeApiSdk.SpeechTextAvailable += RealtimeApiSdk_SpeechTextAvailable;
            RealtimeApiSdk.SpeechEnded += RealtimeApiSdk_SpeechEnded;

            RealtimeApiSdk.PlaybackStarted += RealtimeApiSdk_PlaybackStarted;
            RealtimeApiSdk.PlaybackDataAvailable += RealtimeApiSdk_PlaybackDataAvailable;
            RealtimeApiSdk.PlaybackTextAvailable += RealtimeApiSdk_PlaybackTextAvailable;
            RealtimeApiSdk.PlaybackEnded += RealtimeApiSdk_PlaybackEnded;

            voiceVisualEffect = VoiceVisualEffect;
            PlayVisualVoiceEffect(false);

            DrawDefaultVisualEffect(voiceVisualEffect);
        }

        private void RealtimeApiSdk_WaveInDataAvailable(object? sender, WaveInEventArgs e)
        {
            OnWaveInDataAvailable(e);
        }

        protected virtual void OnWaveInDataAvailable(WaveInEventArgs e)
        {
            WaveInDataAvailable?.Invoke(this, e);
        }

        private void RealtimeApiSdk_WebSocketResponse(object? sender, WebSocketResponseEventArgs e)
        {
            OnWebSocketResponse(e);
        }

        protected virtual void OnWebSocketResponse(WebSocketResponseEventArgs e)
        {
            WebSocketResponse?.Invoke(this, e);
        }

        private void RealtimeApiSdk_PlaybackDataAvailable(object? sender, AudioEventArgs e)
        {
            OnPlaybackDataAvailable(e);
        }

        public void StartSpeechRecognition()
        {
            //string errorMsg = RealtimeApiSdk.ValidateLicense();
            //if (!string.IsNullOrWhiteSpace(errorMsg)) 
            //{
            //    MessageBox.Show(errorMsg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}

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

        public void RegisterFunctionCall(FunctionCallSetting functionCallSetting, Func<FuncationCallArgument, JObject> functionCallback)
        {
            RealtimeApiSdk.RegisterFunctionCall(functionCallSetting, functionCallback);
        }

        private void PlayVisualVoiceEffect(bool enable)
        {
            WaveCanvas.Visibility = voiceVisualEffect == WPF.VisualEffect.SoundWave ? Visibility.Visible : Visibility.Collapsed;
            //RippleEffect.Visibility = voiceVisualEffect == WPF.VisualEffect.Cycle ? Visibility.Visible : Visibility.Collapsed;
            RippleEffect.Visibility = Visibility.Collapsed;
            cycleWaveformCanvas.Visibility = voiceVisualEffect == WPF.VisualEffect.Cycle ? Visibility.Visible : Visibility.Collapsed;


            switch (voiceVisualEffect)
            {
                case WPF.VisualEffect.Cycle:
                    HandleCycleVisualVoiceEffect(enable);
                    break;
                case WPF.VisualEffect.SoundWave:
                    HandleWaveVisualVoiceEffect(enable);
                    break;
                default:
                    break;
            }
        }

        private void HandleCycleVisualVoiceEffect(bool enable)
        {
            if (enable)
            {
                speakerCapture.StartRecording();
                speechWaveIn.StartRecording();

            }
            else
            {
                speakerCapture.StopRecording();

                speechWaveIn.StopRecording();
                WaveCanvas.Children.Clear();
            }

        }

        private void HandleWaveVisualVoiceEffect(bool enable)
        {
            if (enable)
            {
                speakerCapture.StartRecording();
                speechWaveIn.StartRecording();
            }
            else
            {
                speakerCapture.StopRecording();

                speechWaveIn.StopRecording();
                cycleWaveformCanvas.Children.Clear();
            }
        }

        private void SpeechWaveIn_DataAvailable(object? sender, WaveInEventArgs e)
        {
            List<float> audioBuffer = new List<float>();
            for (int i = 0; i < e.BytesRecorded; i += 2)
            {
                short value = BitConverter.ToInt16(e.Buffer, i);
                float normalized = value / 32768f;
                audioBuffer.Add(normalized);
            }

            try
            {
                switch (this.voiceVisualEffect)
                {
                    case WPF.VisualEffect.Cycle:
                        Dispatcher.Invoke(() => cycleWaveformCanvas.UpdateAudioData(audioBuffer.ToArray()));
                        break;
                    case WPF.VisualEffect.SoundWave:
                        Dispatcher.Invoke(() => DrawWaveform(audioBuffer.ToArray()));
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex) { }
        }

        private void SpeakerCapture_DataAvailable(object? sender, WaveInEventArgs e)
        {
            speakerWaveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
            var audioBuffer = new float[e.BytesRecorded / 4];
            WaveBuffer waveBuffer = new WaveBuffer(e.Buffer);
            for (int i = 0; i < audioBuffer.Length; i++)
            {
                audioBuffer[i] = waveBuffer.FloatBuffer[i];
            }

            try
            {
                switch (this.voiceVisualEffect)
                {
                    case WPF.VisualEffect.Cycle:
                        Dispatcher.Invoke((Delegate)(() => cycleWaveformCanvas.UpdateAudioData(audioBuffer)));
                        break;
                    case WPF.VisualEffect.SoundWave:
                        Dispatcher.Invoke((Delegate)(() => DrawWaveform(audioBuffer)));
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex) { }
        }

        private void RealtimeApiSdk_PlaybackAudioReceived(object? sender, AudioEventArgs e)
        {
            try
            {
                switch (this.voiceVisualEffect)
                {
                    case WPF.VisualEffect.Cycle:
                        Dispatcher.Invoke(() => cycleWaveformCanvas.UpdateAudioData(e.GetWaveBuffer()));
                        break;
                    case WPF.VisualEffect.SoundWave:
                        Dispatcher.Invoke(() => DrawWaveform(e.GetWaveBuffer()));
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex) { }
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

        private void DrawDefaultVisualEffect(VisualEffect effect)
        {
            WaveCanvas.Children.Clear();

            switch (effect)
            {
                case WPF.VisualEffect.Cycle:
                    DrawCircle();
                    break;

                case WPF.VisualEffect.SoundWave:
                    DrawLine();
                    break;

                default:
                    break;
            }
        }

        private void DrawCircle(double sizeFactor = 0.95)
        {
            double canvasWidth = cycleWaveformCanvas.ActualWidth;
            double canvasHeight = cycleWaveformCanvas.ActualHeight;
            if (canvasWidth == 0 || canvasHeight == 0)
                return;

            double radius = Math.Min(canvasWidth, canvasHeight) * sizeFactor / 2;
            double centerX = canvasWidth / 2;
            double centerY = canvasHeight / 2;

            Ellipse circle = new Ellipse
            {
                Width = radius * 2,
                Height = radius * 2,
                Stroke = Brushes.LimeGreen,
                StrokeThickness = 2
            };

            Canvas.SetLeft(circle, centerX - radius);
            Canvas.SetTop(circle, centerY - radius);

            cycleWaveformCanvas.Children.Clear();

            cycleWaveformCanvas.Children.Add(circle);
        }

        private void DrawLine()
        {
            double canvasWidth = WaveCanvas.ActualWidth;
            double canvasHeight = WaveCanvas.ActualHeight;

            if (canvasWidth == 0 || canvasHeight == 0)
                return;

            Line line = new Line
            {
                X1 = 10,
                Y1 = canvasHeight / 2,
                X2 = canvasWidth - 10,
                Y2 = canvasHeight / 2,
                Stroke = Brushes.LimeGreen,
                StrokeThickness = 2
            };


            WaveCanvas.Children.Add(line);
        }

        private void WaveCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawDefaultVisualEffect(voiceVisualEffect);
        }

        private void cycleWaveformCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawDefaultVisualEffect(voiceVisualEffect);
        }
    }
}
