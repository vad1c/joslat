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

        private WaveInEvent speechWaveIn;
        
        private WasapiLoopbackCapture speakerCapture;
        private BufferedWaveProvider speakerWaveProvider;

        public RealtimeApiWpfControl()
        {
            InitializeComponent();

            RealtimeApiSdk = new RealtimeApiSdk();
            //RealtimeApiSdk.PlaybackAudioReceived += RealtimeApiSdk_PlaybackAudioReceived;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //RealtimeApiSdk.TransactionOccurred += RealtimeApiSdk_TransactionOccurred;



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

            speakerCapture.DataAvailable += SpeakerCapture_DataAvailable; ;


            voiceVisualEffect = VoiceVisualEffect;
            PlayVisualVoiceEffect(false);
            //RippleEffect.Visibility = Visibility.Hidden;
            //WaveformContainer.Visibility = Visibility.Hidden;
            //waveformCanvas.Visibility = Visibility.Hidden;

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

    }
}
