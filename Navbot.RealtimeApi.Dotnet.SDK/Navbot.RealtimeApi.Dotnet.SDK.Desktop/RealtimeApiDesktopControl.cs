using NAudio.Wave;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Navbot.RealtimeApi.Dotnet.SDK.Core;
using Navbot.RealtimeApi.Dotnet.SDK.Core.Events;
using Navbot.RealtimeApi.Dotnet.SDK.Core.Model.Function;
using Navbot.RealtimeApi.Dotnet.SDK.Core.Model.Response;
using Navbot.RealtimeApi.Dotnet.SDK.Desktop;
using Timer = System.Windows.Forms.Timer;

namespace Navbot.RealtimeApi.Dotnet.SDK.Desktop
{
    public partial class RealtimeApiDesktopControl : UserControl
    {
        private const string apiKey = "";
        private VisualEffect voiceVisualEffect = VisualEffect.Cycle; // 默认绘制圆形
        private Timer waveformUpdateTimer;

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

        public RealtimeApiDesktopControl()
        {
            InitializeComponent();

            RealtimeApiSdk = new RealtimeApiSdk();

            this.DoubleBuffered = true; // 防止重绘时闪烁
            this.Resize += (s, e) => this.Invalidate(); // 监听大小改变事件，触发重绘

            this.Dock = DockStyle.Fill;

            // 设置定时器更新波形图
            waveformUpdateTimer = new Timer();
            waveformUpdateTimer.Interval = 100; // 100ms更新一次
            waveformUpdateTimer.Tick += (s, e) =>
            {
                // 触发绘制波形
                this.Invalidate();
            };
            waveformUpdateTimer.Start();
        }

        public void SetVisualEffect(VisualEffect effect)
        {
            voiceVisualEffect = effect;
            this.Invalidate(); // 改变效果后重新绘制
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

        public void StartSpeechRecognition()
        {
            string errorMsg = RealtimeApiSdk.ValidateLicense();
            if (!string.IsNullOrWhiteSpace(errorMsg))
            {
                MessageBox.Show(errorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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
                case VisualEffect.Cycle:
                    HandleCycleVisualVoiceEffect(enable);
                    break;
                case VisualEffect.SoundWave:
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
            }
        }



        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // 获取控件的宽度和高度
            int width = this.ClientSize.Width;
            int height = this.ClientSize.Height;

            switch (voiceVisualEffect)
            {
                case VisualEffect.Cycle:
                    DrawCircle(e.Graphics, width, height);
                    break;

                case VisualEffect.SoundWave:
                    DrawLine(e.Graphics, width, height);
                    break;
            }
        }

        private void DrawCircle(Graphics g, int width, int height)
        {
            // 计算圆的直径（取宽高最小值并减去边距）
            int diameter = Math.Min(width, height) - 10; // 10 是边距

            if (diameter > 0)
            {
                // 计算圆的位置，使其居中
                int x = (width - diameter) / 2;
                int y = (height - diameter) / 2;

                // 创建画笔
                using (Pen pen = new Pen(Color.Red, 3)) // 红色边框，宽度为 3
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // 平滑边缘
                    g.DrawEllipse(pen, x, y, diameter, diameter); // 绘制圆
                }
            }
        }

        private void DrawLine(Graphics g, int width, int height)
        {
            // 计算线段的起始和结束点
            Point startPoint = new Point(10, height / 2); // 左侧中间
            Point endPoint = new Point(width - 10, height / 2); // 右侧中间

            using (Pen pen = new Pen(Color.Blue, 3)) // 蓝色边框，宽度为 3
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // 平滑边缘
                g.DrawLine(pen, startPoint, endPoint); // 绘制直线
            }
        }

        private void RealtimeApiDesktopControl_Load(object sender, EventArgs e)
        {
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

            //DrawDefaultVisualEffect(voiceVisualEffect);
        }

        #region Event

        public void RegisterFunctionCall(FunctionCallSetting functionCallSetting, Func<FuncationCallArgument, JObject> functionCallback)
        {
            RealtimeApiSdk.RegisterFunctionCall(functionCallSetting, functionCallback);
        }
        private void RealtimeApiSdk_PlaybackEnded(object? sender, EventArgs e)
        {
            OnPlaybackEnded(e);
        }

        protected virtual void OnPlaybackEnded(EventArgs e)
        {
            PlaybackEnded?.Invoke(this, e);
        }
        private void RealtimeApiSdk_PlaybackTextAvailable(object? sender, TranscriptEventArgs e)
        {
            OnPlaybackTextAvailable(e);
        }

        protected virtual void OnPlaybackTextAvailable(TranscriptEventArgs e)
        {
            PlaybackTextAvailable?.Invoke(this, e);
        }
        private void RealtimeApiSdk_PlaybackDataAvailable(object? sender, AudioEventArgs e)
        {
            OnPlaybackDataAvailable(e);
        }
        protected virtual void OnPlaybackDataAvailable(AudioEventArgs e)
        {
            PlaybackDataAvailable?.Invoke(this, e);
        }
        private void RealtimeApiSdk_PlaybackStarted(object? sender, EventArgs e)
        {
            OnPlaybackStarted(e);
        }

        protected virtual void OnPlaybackStarted(EventArgs e)
        {
            PlaybackStarted?.Invoke(this, e);
        }
        private void RealtimeApiSdk_SpeechEnded(object? sender, AudioEventArgs e)
        {
            OnSpeechEnded(e);
        }

        protected virtual void OnSpeechEnded(AudioEventArgs e)
        {
            SpeechEnded?.Invoke(this, e);
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

        private void RealtimeApiSdk_WebSocketResponse(object? sender, WebSocketResponseEventArgs e)
        {
            OnWebSocketResponse(e);
        }

        protected virtual void OnWebSocketResponse(WebSocketResponseEventArgs e)
        {
            WebSocketResponse?.Invoke(this, e);
        }
        private void RealtimeApiSdk_SpeechStarted(object? sender, EventArgs e)
        {
            OnSpeechStarted(e);
        }

        protected virtual void OnSpeechStarted(EventArgs e)
        {
            SpeechStarted?.Invoke(this, e);
        }
        #endregion


        private void SpeechWaveIn_DataAvailable(object? sender, WaveInEventArgs e)
        {
            List<float> audioBuffer = new List<float>();
            for (int i = 0; i < e.BytesRecorded; i += 2)
            {
                short value = BitConverter.ToInt16(e.Buffer, i);
                float normalized = value / 32768f;
                audioBuffer.Add(normalized);
            }
            // 调用绘制方法
            DrawWaveform(audioBuffer.ToArray());
            //try
            //{
            //    switch (this.voiceVisualEffect)
            //    {
            //        case VisualEffect.Cycle:
            //            //Dispatcher.Invoke(() => cycleWaveformCanvas.UpdateAudioData(audioBuffer.ToArray()));
            //            break;
            //        case VisualEffect.SoundWave:
            //            //Dispatcher.Invoke(() => DrawWaveform(audioBuffer.ToArray()));
            //            break;
            //        default:
            //            break;
            //    }

            //}
            //catch (Exception ex) { }
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
            // 调用绘制方法
            DrawWaveform(audioBuffer);
            //try
            //{
            //    switch (this.voiceVisualEffect)
            //    {
            //        case VisualEffect.Cycle:
            //            //Dispatcher.Invoke((Delegate)(() => cycleWaveformCanvas.UpdateAudioData(audioBuffer)));
            //            break;
            //        case VisualEffect.SoundWave:
            //            //Dispatcher.Invoke((Delegate)(() => DrawWaveform(audioBuffer)));
            //            break;
            //        default:
            //            break;
            //    }

            //}
            //catch (Exception ex) { }
        }


        private void DrawWaveform(float[] audioBuffer)
        {
            if (audioBuffer == null || audioBuffer.Length == 0)
            {
                // 如果音频缓冲区为空，跳过绘制或执行其他逻辑
                return;
            }

            // 获取当前控件的 Graphics 对象
            Graphics g = this.CreateGraphics();

            // 清除之前的绘制
            g.Clear(Color.Black);

            // 设置画笔和线条宽度
            using (Pen pen = new Pen(Color.Green, 1))
            {
                // 设置抗锯齿模式
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // 绘制波形图
                int width = this.ClientSize.Width;
                int height = this.ClientSize.Height;

                // 将音频数据映射到控件的宽度和高度
                int middle = height / 2; // 中间水平线
                int step = width / audioBuffer.Length;

                // 绘制波形
                for (int i = 0; i < audioBuffer.Length; i++)
                {
                    int x = i * step;
                    int y = middle + (int)(audioBuffer[i] * middle);
                    g.DrawLine(pen, x, middle, x, y);
                }
            }
        }





    }
}
