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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
                BackColor = Color.Red,
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
                BackColor = Color.White
             };

            // 底部 Panel 初始化
             bottomPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.LightGray
            };

            // 按钮初始化
            var btnStart = new Button
            {
                Text = "Start",
                Size = new Size(100, 40),
                Anchor = AnchorStyles.None
            };
            var btnEnd = new Button
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
            tableLayoutPanel.Controls.Add(circlePanel, 0, 0);
            tableLayoutPanel.Controls.Add(rightPanel, 1, 0);
            tableLayoutPanel.SetColumnSpan(bottomPanel, 2); // 底部区域跨两列
            tableLayoutPanel.Controls.Add(bottomPanel, 0, 1);

            // 将 TableLayoutPanel 添加到窗体
            this.Controls.Add(tableLayoutPanel);
        }

        
    }
}
