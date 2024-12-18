using NAudio.Wave;
using Newtonsoft.Json;
using Realtime.API.Dotnet.SDK.Core;
using Realtime.API.Dotnet.SDK.Core.Events;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Realtime.API.Dotnet.SDK.Desktop
{
    public partial class RealtimeApiDesktopControl : UserControl
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

        public RealtimeApiDesktopControl()
        {
            InitializeComponent();

            RealtimeApiSdk = new RealtimeApiSdk();

            this.DoubleBuffered = true; // 防止重绘时闪烁
            this.Resize += (s, e) => this.Invalidate(); // 监听大小改变事件，触发重绘

            this.Dock = DockStyle.Fill;
        }

        public RealtimeApiSdk RealtimeApiSdk { get; private set; }

        public string OpenAiApiKey
        {
            get { return RealtimeApiSdk.ApiKey; }
            set { RealtimeApiSdk.ApiKey = value; }
        }

        public void StartSpeechRecognition()
        {
            if (!ValidateLicense())
                return;

            if (!RealtimeApiSdk.IsRunning)
            {
                // Start ripple effect.
                //PlayVisualVoiceEffect(true);

                // Start voice recognition;
                RealtimeApiSdk.StartSpeechRecognitionAsync();
            }
        }

        public void StopSpeechRecognition()
        {
            if (RealtimeApiSdk.IsRunning)
            {
                // Stop the ripple effect.
                //PlayVisualVoiceEffect(false);

                // Stop voice recognition;
                RealtimeApiSdk.StopSpeechRecognitionAsync();
            }
        }



        private bool ValidateLicense()
        {
            try
            {
                // Retrieve the current directory, assuming public_key.xml and license.json are in the same directory
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string publicKeyPath = System.IO.Path.Combine(currentDirectory, "public_key.xml");
                string licensePath = System.IO.Path.Combine(currentDirectory, "license.json");

                if (!File.Exists(publicKeyPath))
                {
                    MessageBox.Show("The public key file public_key.xml does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (!File.Exists(licensePath))
                {
                    MessageBox.Show("The License file license.json does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // Read public key
                string publicKey = File.ReadAllText(publicKeyPath);

                // Read License file
                string licenseContent = File.ReadAllText(licensePath);
                dynamic licenseFile = JsonConvert.DeserializeObject(licenseContent);

                // Extract License data and signature
                string licenseJson = JsonConvert.SerializeObject(licenseFile.Data);
                byte[] dataBytes = Encoding.UTF8.GetBytes(licenseJson);
                byte[] signatureBytes = Convert.FromBase64String((string)licenseFile.Signature);

                // verify signature
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(publicKey);
                    bool isValid = rsa.VerifyData(dataBytes, CryptoConfig.MapNameToOID("SHA256"), signatureBytes);

                    if (isValid)
                    {
                        MessageBox.Show("License verification successful!", "Verification successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("License verification failed! The data may be tampered with.", "Validation failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during the verification process:{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // 获取控件的宽度和高度
            int width = this.ClientSize.Width;
            int height = this.ClientSize.Height;

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
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // 平滑边缘
                    e.Graphics.DrawEllipse(pen, x, y, diameter, diameter); // 绘制圆
                }
            }
        }
    }
}
