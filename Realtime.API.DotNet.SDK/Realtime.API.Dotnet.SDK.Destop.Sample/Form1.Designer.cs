namespace Realtime.API.Dotnet.SDK.Desktop.Sample
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tableLayoutPanel = new TableLayoutPanel();
            circlePanel = new Panel();
            realtimeApiDesktopControl1 = new RealtimeApiDesktopControl();
            rightPanel = new Panel();
            bottomPanel = new Panel();
            btnEnd = new Button();
            btnStart = new Button();
            tableLayoutPanel.SuspendLayout();
            circlePanel.SuspendLayout();
            bottomPanel.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.BackColor = SystemColors.ActiveCaption;
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68.70229F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31.29771F));
            tableLayoutPanel.Controls.Add(circlePanel, 0, 0);
            tableLayoutPanel.Controls.Add(rightPanel, 1, 0);
            tableLayoutPanel.Controls.Add(bottomPanel, 0, 1);
            tableLayoutPanel.Location = new Point(12, 12);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 2;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel.Size = new Size(786, 435);
            tableLayoutPanel.TabIndex = 0;
            // 
            // circlePanel
            // 
            circlePanel.Controls.Add(realtimeApiDesktopControl1);
            circlePanel.Location = new Point(3, 3);
            circlePanel.Name = "circlePanel";
            circlePanel.Size = new Size(387, 211);
            circlePanel.TabIndex = 0;
            // 
            // realtimeApiDesktopControl1
            // 
            realtimeApiDesktopControl1.Dock = DockStyle.Fill;
            realtimeApiDesktopControl1.Location = new Point(0, 0);
            realtimeApiDesktopControl1.Name = "realtimeApiDesktopControl1";
            realtimeApiDesktopControl1.OpenAiApiKey = "";
            realtimeApiDesktopControl1.Size = new Size(387, 211);
            realtimeApiDesktopControl1.TabIndex = 0;
            // 
            // rightPanel
            // 
            rightPanel.Location = new Point(543, 3);
            rightPanel.Name = "rightPanel";
            rightPanel.Size = new Size(240, 211);
            rightPanel.TabIndex = 1;
            // 
            // bottomPanel
            // 
            bottomPanel.Controls.Add(btnEnd);
            bottomPanel.Controls.Add(btnStart);
            bottomPanel.Location = new Point(3, 220);
            bottomPanel.Name = "bottomPanel";
            bottomPanel.Size = new Size(534, 212);
            bottomPanel.TabIndex = 2;
            // 
            // btnEnd
            // 
            btnEnd.Location = new Point(341, 90);
            btnEnd.Name = "btnEnd";
            btnEnd.Size = new Size(88, 35);
            btnEnd.TabIndex = 1;
            btnEnd.Text = "End";
            btnEnd.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(79, 90);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(88, 35);
            btnStart.TabIndex = 0;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel);
            Name = "Form1";
            Text = "Form1";
            tableLayoutPanel.ResumeLayout(false);
            circlePanel.ResumeLayout(false);
            bottomPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel;
        private Panel circlePanel;
        private RealtimeApiDesktopControl realtimeApiDesktopControl1;
        private Panel rightPanel;
        private Panel bottomPanel;
        private Button btnEnd;
        private Button btnStart;
    }
}