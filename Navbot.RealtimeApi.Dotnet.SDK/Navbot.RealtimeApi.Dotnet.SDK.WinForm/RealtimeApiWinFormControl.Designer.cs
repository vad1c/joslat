namespace Navbot.RealtimeApi.Dotnet.SDK.WinForm
{
    partial class RealtimeApiWinFormControl
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            audioVisualizer1 = new AudioVisualizer.WinForm.AudioVisualizer();
            SuspendLayout();
            // 
            // audioVisualizer1
            // 
            audioVisualizer1.AudioSampleRate = 8192;
            audioVisualizer1.BackColor = Color.Black;
            audioVisualizer1.Dock = DockStyle.Fill;
            audioVisualizer1.Location = new Point(0, 0);
            audioVisualizer1.Name = "audioVisualizer1";
            audioVisualizer1.RenderInterval = 50;
            audioVisualizer1.Scale = 1F;
            audioVisualizer1.Size = new Size(200, 200);
            audioVisualizer1.TabIndex = 0;
            audioVisualizer1.VisualEffict = AudioVisualizer.Core.VisualEffict.Oscilloscope;
            // 
            // RealtimeApiWinFormControl
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlText;
            Controls.Add(audioVisualizer1);
            Name = "RealtimeApiWinFormControl";
            Size = new Size(200, 200);
            Load += RealtimeApiDesktopControl_Load;
            ResumeLayout(false);
        }

        #endregion

        private AudioVisualizer.WinForm.AudioVisualizer audioVisualizer1;
    }
}
