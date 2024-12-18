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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Init();
        }



        public void Init() 
        {
            this.Size = new Size(800, 500);

            // TableLayoutPanel 初始化
            tlpMain = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 2,
                BackColor = Color.LightGray
            };
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));

            // 圆形 Panel 初始化
            circlePanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            circlePanel.Paint += CirclePanel_Paint; // 添加绘制事件

            // 右侧 Panel 初始化
            rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.WhiteSmoke
            };

            // 底部 Panel 初始化
            bottomPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.LightGray
            };

            // 按钮初始化
            btnStart = new Button
            {
                Text = "Start",
                Size = new Size(100, 40),
                Anchor = AnchorStyles.None
            };
            btnEnd = new Button
            {
                Text = "End",
                Size = new Size(100, 40),
                Anchor = AnchorStyles.None
            };

            // 将按钮添加到底部 Panel
            bottomPanel.Controls.Add(btnStart);
            bottomPanel.Controls.Add(btnEnd);

            // 调整按钮位置
            btnStart.Location = new Point(200, 50);
            btnEnd.Location = new Point(400, 50);

            // 将控件添加到 TableLayoutPanel
            tlpMain.Controls.Add(circlePanel, 0, 0);
            tlpMain.Controls.Add(rightPanel, 1, 0);
            tlpMain.SetColumnSpan(bottomPanel, 2); // 底部区域跨两列
            tlpMain.Controls.Add(bottomPanel, 0, 1);

            // 将 TableLayoutPanel 添加到窗体
            this.Controls.Add(tlpMain);
        }

        private void CirclePanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Red, 3);
            int radius = Math.Min(circlePanel.Width, circlePanel.Height) - 20;
            g.DrawEllipse(pen, 10, 10, radius, radius);
        }
    }
}
