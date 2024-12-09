using NAudio.Wave;
using Newtonsoft.Json.Linq;
using Realtime.API.Dotnet.SDK.Core;
using Realtime.API.Dotnet.SDK.Core.Events;
using Realtime.API.Dotnet.SDK.Core.Model.Function;
using System;
using System.Net.WebSockets;
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

        //TODO Move into Api Sdk
        private WaveInEvent speechWaveIn;

        private WasapiLoopbackCapture speakerCapture;
        private BufferedWaveProvider speakerWaveProvider;

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
            //RealtimeApiSdk.PlaybackAudioReceived += RealtimeApiSdk_PlaybackAudioReceived;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //RealtimeApiSdk.TransactionOccurred += RealtimeApiSdk_TransactionOccurred;


            // TODO connect to sdk event
            speechWaveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(44100, 1)
            };

            speechWaveIn.DataAvailable += SpeechWaveIn_DataAvailable;
            //waveIn.StopRecording();


            speakerCapture = new WasapiLoopbackCapture();
            speakerWaveProvider = new BufferedWaveProvider(speakerCapture.WaveFormat)
            {
                BufferLength = 1024 * 1024, // 1 MB buffer (adjust based on your needs)
                DiscardOnBufferOverflow = true // Optional: discard data when buffer is full}
            };

            speakerCapture.DataAvailable += SpeakerCapture_DataAvailable;

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
            //RippleEffect.Visibility = Visibility.Hidden;
            //WaveformContainer.Visibility = Visibility.Hidden;
            //waveformCanvas.Visibility = Visibility.Hidden;

            DrawDefaultVisualEffect(voiceVisualEffect);
        }

        private void RealtimeApiSdk_PlaybackDataAvailable(object? sender, AudioEventArgs e)
        {
            OnPlaybackDataAvailable(e);
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

        public void RegisterFunctionCall(FunctionCallSetting functionCallSetting, Func<JObject, ClientWebSocket, bool> functionCallback)
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
                //RealtimeApiSdk.SpeechAudioReceived += RealtimeApiSdk_SpeechAudioReceived;
                //RealtimeApiSdk.PlaybackAudioReceived += RealtimeApiSdk_PlaybackAudioReceived;
                speakerCapture.StartRecording();
                //StartAudioCapture();
                speechWaveIn.StartRecording();

            }
            else
            {
                //RealtimeApiSdk.AudioReceived -= RealtimeApiSdk_SpeechAudioReceived;
                //RealtimeApiSdk.PlaybackAudioReceived -= RealtimeApiSdk_PlaybackAudioReceived;
                speakerCapture.StopRecording();

                //StopAudioCapture();
                speechWaveIn.StopRecording();
                WaveCanvas.Children.Clear();
                //DrawWaveform(new float[] { 0f});
            }

        }

        private void HandleWaveVisualVoiceEffect(bool enable)
        {
            if (enable)
            {
                //RealtimeApiSdk.SpeechAudioReceived += RealtimeApiSdk_SpeechAudioReceived;
                //RealtimeApiSdk.PlaybackAudioReceived += RealtimeApiSdk_PlaybackAudioReceived;
                speakerCapture.StartRecording();
                //StartAudioCapture();
                speechWaveIn.StartRecording();
            }
            else
            {
                //RealtimeApiSdk.AudioReceived -= RealtimeApiSdk_SpeechAudioReceived;
                //RealtimeApiSdk.PlaybackAudioReceived -= RealtimeApiSdk_PlaybackAudioReceived;
                speakerCapture.StopRecording();

                //StopAudioCapture();
                speechWaveIn.StopRecording();
                cycleWaveformCanvas.Children.Clear();
                //cycleWaveformCanvas.UpdateAudioData( new float[] { 0});
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
                //audioBuffer.Add(BitConverter.ToSingle(e.Buffer, i));
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
            //List<float> audioBuffer = new List<float>();
            //for (int i = 0; i < e.BytesRecorded; i += 2)
            //{
            //    short value = BitConverter.ToInt16(e.Buffer, i);
            //    float normalized = value / 32768f;
            //    audioBuffer.Add(normalized);
            //    //audioBuffer.Add(BitConverter.ToSingle(e.Buffer, i));
            //}

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
        private void DrawCircle(double sizeFactor = 0.9)
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
                X1 = 0,
                Y1 = canvasHeight / 2,
                X2 = canvasWidth,
                Y2 = canvasHeight / 2,
                Stroke = Brushes.LimeGreen,
                StrokeThickness = 2
            };

            WaveCanvas.Children.Add(line);
        }


    }
}
