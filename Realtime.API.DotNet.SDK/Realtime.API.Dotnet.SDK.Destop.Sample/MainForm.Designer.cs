namespace Realtime.API.Dotnet.SDK.Desktop.Sample
{
    partial class MainForm
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
            tlpMain = new TableLayoutPanel();
            rightPanel = new Panel();
            bottomPanel = new Panel();
            btnEnd = new Button();
            btnStart = new Button();
            circlePanel = new Panel();
            tlpMain.SuspendLayout();
            bottomPanel.SuspendLayout();
            SuspendLayout();
            // 
            // tlpMain
            // 
            tlpMain.ColumnCount = 2;
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpMain.Controls.Add(rightPanel, 1, 0);
            tlpMain.Controls.Add(bottomPanel, 0, 1);
            tlpMain.Controls.Add(circlePanel, 0, 0);
            tlpMain.Location = new Point(3, 0);
            tlpMain.Name = "tlpMain";
            tlpMain.RowCount = 2;
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpMain.Size = new Size(799, 452);
            tlpMain.TabIndex = 0;
            tlpMain.Visible = false;
            // 
            // rightPanel
            // 
            rightPanel.Location = new Point(402, 3);
            rightPanel.Name = "rightPanel";
            rightPanel.Size = new Size(200, 100);
            rightPanel.TabIndex = 1;
            // 
            // bottomPanel
            // 
            bottomPanel.Controls.Add(btnEnd);
            bottomPanel.Controls.Add(btnStart);
            bottomPanel.Location = new Point(3, 229);
            bottomPanel.Name = "bottomPanel";
            bottomPanel.Size = new Size(393, 209);
            bottomPanel.TabIndex = 2;
            // 
            // btnEnd
            // 
            btnEnd.Location = new Point(194, 88);
            btnEnd.Name = "btnEnd";
            btnEnd.Size = new Size(81, 35);
            btnEnd.TabIndex = 1;
            btnEnd.Text = "End";
            btnEnd.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(74, 88);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(81, 35);
            btnStart.TabIndex = 0;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = true;
            // 
            // circlePanel
            // 
            circlePanel.Location = new Point(3, 3);
            circlePanel.Name = "circlePanel";
            circlePanel.Size = new Size(200, 100);
            circlePanel.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tlpMain);
            Name = "MainForm";
            Text = "MainForm";
            tlpMain.ResumeLayout(false);
            bottomPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpMain;
        private Panel circlePanel;
        private Panel rightPanel;
        private Panel panel2;
        private Panel bottomPanel;
        private Button btnEnd;
        private Button btnStart;
    }
}