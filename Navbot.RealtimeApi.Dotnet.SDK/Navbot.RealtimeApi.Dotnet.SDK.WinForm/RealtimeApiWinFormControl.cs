using NAudio.Wave;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Navbot.RealtimeApi.Dotnet.SDK.Core;
using Navbot.RealtimeApi.Dotnet.SDK.Core.Events;
using Navbot.RealtimeApi.Dotnet.SDK.Core.Model.Function;
using Navbot.RealtimeApi.Dotnet.SDK.Core.Model.Response;
using Timer = System.Windows.Forms.Timer;
using Navbot.RealtimeApi.Dotnet.SDK.WinForm;

namespace Navbot.RealtimeApi.Dotnet.SDK.WinForm
{
    public partial class RealtimeApiWinFormControl : UserControl
    {
        private const string apiKey = "";
        private VisualEffect voiceVisualEffect = VisualEffect.Cycle; // 默认绘制圆形
        private Timer waveformUpdateTimer;

        private float[] _waveformData; // 波形数据
        private Timer _refreshTimer;  // 定时器用于刷新界面
        private Random _random;       // 模拟随机波形数据（可替换为实际音频数据）

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

        public RealtimeApiWinFormControl()
        {
            InitializeComponent();
            RealtimeApiSdk = new RealtimeApiSdk();

            Init();
            this.Resize += (s, e) => this.Invalidate();
        }

        public void Init() 
        {
            this.DoubleBuffered = true; // 启用双缓冲，减少闪烁
            _waveformData = new float[100]; // 初始化波形数据
        }

        // 设置波形数据（实际使用时调用此方法更新数据）
        public void UpdateWaveform(float[] waveformData)
        {
            _waveformData = waveformData;
            this.Invalidate(); // 触发重绘
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

            // 清空之前的绘制内容
            e.Graphics.Clear(this.BackColor);

            if (_waveformData == null || _waveformData.Length == 0)
                return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit; // 抗锯齿文本渲染
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality; // 高质量像素偏移
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality; // 高质量合成

            // 获取控件当前宽高并添加内边距
            int margin = 10; // 设定内边距
            int width = this.ClientSize.Width - margin * 2; // 宽度减去左右边距
            int height = this.ClientSize.Height - margin * 2; // 高度减去上下边距
            float step = (float)width / _waveformData.Length; // 每个数据点的间隔
            float centerY = height / 2 + margin; // 波形中心位置向下偏移 margin

            // 创建渐变画刷
            using (var gradientBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                       new PointF(margin, 0),
                       new PointF(width + margin, 0),
                       Color.Purple,
                       Color.Yellow))
            using (var pen = new Pen(gradientBrush, 2)) // 波形线条
            {
                // 绘制波形
                for (int i = 1; i < _waveformData.Length; i++)
                {
                    float x1 = margin + (i - 1) * step; // 左边增加 margin
                    float y1 = centerY - (_waveformData[i - 1] * (height / 2));
                    float x2 = margin + i * step; // 右边增加 margin
                    float y2 = centerY - (_waveformData[i] * (height / 2));
                    g.DrawLine(pen, x1, y1, x2, y2);
                }
            }
        }

        private void DrawCircle(Graphics g, int width, int height)
        {
            // Calculate the diameter of a circle (taking the minimum width and height and subtracting the margin)
            int diameter = Math.Min(width, height) - 10; // 10 is the margin

            if (diameter > 0)
            {
                // Calculate the position of the circle to center it
                int x = (width - diameter) / 2;
                int y = (height - diameter) / 2;

                // Create a paintbrush
                using (Pen pen = new Pen(Color.Red, 3)) // Red border, width 3
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // Smooth edges
                    g.DrawEllipse(pen, x, y, diameter, diameter); // Draw a circle
                }
            }
        }

        private void DrawLine(Graphics g, int width, int height)
        {
            // Calculate the starting and ending points of a line segment
            Point startPoint = new Point(10, height / 2); // Left middle
            Point endPoint = new Point(width - 10, height / 2); // Right middle

            using (Pen pen = new Pen(Color.Blue, 3)) // Blue border, width 3
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // Smooth edges
                g.DrawLine(pen, startPoint, endPoint); // draw a straight line
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
            try
            {
                switch (this.voiceVisualEffect)
                {
                    case VisualEffect.Cycle:
                        UpdateWaveform(audioBuffer.ToArray());
                        break;
                    case VisualEffect.SoundWave:
                        UpdateWaveform(audioBuffer.ToArray());
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
                    case VisualEffect.Cycle:
                        UpdateWaveform(audioBuffer.ToArray());
                        break;
                    case VisualEffect.SoundWave:
                        UpdateWaveform(audioBuffer.ToArray());
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex) { }
        }


    }
}
