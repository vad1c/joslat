using log4net;
using Navbot.RealtimeApi.Dotnet.SDK.Core.Model.Function;
using Navbot.RealtimeApi.Dotnet.SDK.WinForm;

namespace Navbot.RealtimeApi.Dotnet.SDK.Desktop.Sample
{
    public partial class MainForm : Form
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(MainForm));
        RealtimeApiWinFormControl realtimeApiDesktopControl = new RealtimeApiWinFormControl();
        private RichTextBox chatOutput;
        public MainForm()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            realtimeApiDesktopControl.SpeechTextAvailable += RealtimeApiDesktopControl_SpeechTextAvailable;
            realtimeApiDesktopControl.PlaybackTextAvailable += RealtimeApiDesktopControl_PlaybackTextAvailable;

            var tableLayoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 2,
                BackColor = Color.White,
            };
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));

            var circlePanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            var rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ColorTranslator.FromHtml("#322723")
            };

            chatOutput = new RichTextBox
            {
                Dock = DockStyle.Fill,
                BackColor = ColorTranslator.FromHtml("#322723"),
                ForeColor = Color.LightGreen,
                ReadOnly = true,
                Font = new Font("Arial", 12)
            };

            var bottomPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ColorTranslator.FromHtml("#2a2a2a")
            };

            var btnStart = new Button
            {
                Text = "Start",
                Size = new Size(150, 50),
                Anchor = AnchorStyles.None,
                BackColor = Color.Green,
                ForeColor = Color.White,
            };
            btnStart.Click += btnStart_Click;

            var btnEnd = new Button
            {
                Text = "End",
                Size = new Size(150, 50),
                Anchor = AnchorStyles.None,
                BackColor = Color.Red,
                ForeColor = Color.White
            };
            btnEnd.Click += btnEnd_Click;

            bottomPanel.Controls.Add(btnStart);
            bottomPanel.Controls.Add(btnEnd);

            btnStart.Location = new Point(-100, 30);
            btnEnd.Location = new Point(150, 30);



            circlePanel.Controls.Add(realtimeApiDesktopControl);
            rightPanel.Controls.Add(chatOutput);
            bottomPanel.Controls.Add(btnStart);
            bottomPanel.Controls.Add(btnEnd);
            

            tableLayoutPanel.Controls.Add(circlePanel, 0, 0);
            tableLayoutPanel.SetRowSpan(rightPanel, 2);
            tableLayoutPanel.Controls.Add(rightPanel, 1, 0);

            tableLayoutPanel.Controls.Add(bottomPanel, 0, 1);

            this.Controls.Add(tableLayoutPanel);
        }

        private void RealtimeApiDesktopControl_PlaybackTextAvailable(object? sender, Core.Events.TranscriptEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                chatOutput.AppendText($"AI: {e.Transcript}\n"); 
                chatOutput.ScrollToCaret(); 
            });
        }

        private void RealtimeApiDesktopControl_SpeechTextAvailable(object? sender, Core.Events.TranscriptEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                chatOutput.AppendText($"User: {e.Transcript}\n"); 
                chatOutput.ScrollToCaret(); 
            });
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            realtimeApiDesktopControl.StartSpeechRecognition();
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            realtimeApiDesktopControl.StopSpeechRecognition();
        }

        private void MainFrom_Load(object sender, EventArgs e)
        {
            string openAiApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? "";

            realtimeApiDesktopControl.OpenAiApiKey = openAiApiKey;

            RegisterWeatherFunctionCall();
            RegisterNotepadFunctionCall();

            log.Info("App Start...");
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

            realtimeApiDesktopControl.RegisterFunctionCall(weatherFunctionCallSetting, FucationCallHelper.HandleWeatherFunctionCall);
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

            realtimeApiDesktopControl.RegisterFunctionCall(notepadFunctionCallSetting, FucationCallHelper.HandleNotepadFunctionCall);
        }
    }
}
