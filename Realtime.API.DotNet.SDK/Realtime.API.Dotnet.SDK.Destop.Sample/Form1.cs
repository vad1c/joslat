using log4net;
using Microsoft.VisualBasic.Logging;
using Realtime.API.Dotnet.SDK.Core.Model.Function;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Realtime.API.Dotnet.SDK.Desktop.Sample
{
    public partial class MainFrom : Form
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(MainFrom));
        public MainFrom()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            this.Size = new Size(800, 500);

            // TableLayoutPanel 初始化
            tableLayoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 2,
                BackColor = Color.Green,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
            };
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));

            // 圆形 Panel 初始化
            circlePanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            // 右侧 Panel 初始化
            rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.AliceBlue
            };

            // 底部 Panel 初始化
            bottomPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.DarkGoldenrod
            };

            // 按钮初始化
            //var btnStart = new Button
            //{
            //    Text = "Start",
            //    Size = new Size(100, 40),
            //    Anchor = AnchorStyles.None
            //};
            //var btnEnd = new Button
            //{
            //    Text = "End",
            //    Size = new Size(100, 40),
            //    Anchor = AnchorStyles.None
            //};

            //// 将按钮添加到底部 Panel
            //bottomPanel.Controls.Add(btnStart);
            //bottomPanel.Controls.Add(btnEnd);

            // 调整按钮位置
            //btnStart.Location = new Point(200, 50);
            //btnEnd.Location = new Point(400, 50);

            // 将控件添加到 TableLayoutPanel
            tableLayoutPanel.Controls.Add(circlePanel, 0, 0);
            tableLayoutPanel.SetRowSpan(rightPanel, 2);
            tableLayoutPanel.Controls.Add(rightPanel, 1, 0);

            tableLayoutPanel.Controls.Add(bottomPanel, 0, 1);

            // 将 TableLayoutPanel 添加到窗体
            this.Controls.Add(tableLayoutPanel);
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
