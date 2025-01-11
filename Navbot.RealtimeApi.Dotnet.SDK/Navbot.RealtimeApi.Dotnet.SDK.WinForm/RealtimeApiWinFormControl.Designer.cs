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
            audioVisualizer = new AudioVisualizer.WinForm.AudioVisualizerView();
            SuspendLayout();
            // 
            // audioVisualizer1
            // 
            audioVisualizer.AudioSampleRate = 8192;
            audioVisualizer.BackColor = Color.Black;
            audioVisualizer.Dock = DockStyle.Fill;
            audioVisualizer.Location = new Point(0, 0);
            audioVisualizer.Name = "audioVisualizer1";
            audioVisualizer.RenderInterval = 50;
            audioVisualizer.Scale = 1F;
            audioVisualizer.Size = new Size(200, 200);
            audioVisualizer.TabIndex = 0;
            // 
            // RealtimeApiWinFormControl
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlText;
            Controls.Add(audioVisualizer);
            Name = "RealtimeApiWinFormControl";
            Size = new Size(200, 200);
            Load += RealtimeApiDesktopControl_Load;
            ResumeLayout(false);
        }

        #endregion

        private AudioVisualizer.WinForm.AudioVisualizerView audioVisualizer;
    }
}
