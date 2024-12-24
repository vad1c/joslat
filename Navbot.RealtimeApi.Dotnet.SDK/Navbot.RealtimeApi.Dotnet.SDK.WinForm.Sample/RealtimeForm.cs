using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Navbot.RealtimeApi.Dotnet.SDK.WinForm.Sample
{
    public partial class RealtimeForm : Form
    {
        public RealtimeForm()
        {
            InitializeComponent();
        }

        private void RealtimeForm_Load(object sender, EventArgs e)
        {
            string openAiApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? "";
            realtimeApiWinFormControl.OpenAiApiKey = openAiApiKey;

            realtimeApiWinFormControl.SpeechTextAvailable += RealtimeApiWinFormControl_SpeechTextAvailable;
            realtimeApiWinFormControl.PlaybackTextAvailable += RealtimeApiWinFormControl_PlaybackTextAvailable;
        }

        /// <summary>
        /// Start Speech Recognition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            realtimeApiWinFormControl.StartSpeechRecognition();
        }

        /// <summary>
        /// Stop Speech Recognition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnd_Click(object sender, EventArgs e)
        {
            realtimeApiWinFormControl.StopSpeechRecognition();
        }

        /// <summary>
        /// Hook up SpeechTextAvailable event to display speech text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RealtimeApiWinFormControl_SpeechTextAvailable(object? sender, Core.Events.TranscriptEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                rtxOutputInfo.AppendText($"User: {e.Transcript}\n");
            });
        }

        /// <summary>
        /// Hook up PlaybackTextAvailable evnet to display OpenAI response.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RealtimeApiWinFormControl_PlaybackTextAvailable(object? sender, Core.Events.TranscriptEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                rtxOutputInfo.AppendText($"AI: {e.Transcript}\n");
            });
        }
    }
}
