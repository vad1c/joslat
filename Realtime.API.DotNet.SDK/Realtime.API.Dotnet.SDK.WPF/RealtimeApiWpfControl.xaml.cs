using Microsoft.VisualBasic;
using NAudio.Wave;
using Newtonsoft.Json.Linq;
using Realtime.API.Dotnet.SDK.Core;

using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Realtime.API.Dotnet.SDK.WPF
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class RealtimeApiWpfControl : UserControl
    {
        public ObservableCollection<string> chatMessages = new ObservableCollection<string>();
        private const string apiKey = "";

        public RealtimeApiWpfControl()
        {
            InitializeComponent();

            RealtimeApiSdk = new RealtimeApiSdk();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            RealtimeApiSdk.TransactionOccurred += RealtimeApiSdk_TransactionOccurred1;

            ChatListBox.ItemsSource = chatMessages;

            this.StartSpeechRecognition.Click += StartSpeechRecognition_Click;
            this.StopSpeechRecognition.Click += StopSpeechRecognition_Click;
        }

        public RealtimeApiSdk RealtimeApiSdk { get; private set; }

        public string OpenAiApiKey
        {
            get { return RealtimeApiSdk.ApiKey; }
            set { RealtimeApiSdk.ApiKey = value; }
        }

        private void RealtimeApiSdk_TransactionOccurred1(object? sender, TransactionOccurredEventArgs e)
        {
            Dispatcher.Invoke(() => chatMessages.Add(e.Message));
        }

        private void StartSpeechRecognition_Click(object sender, RoutedEventArgs e)
        {
            // Start ripple effect.
            StartRippleEffect();

            // Start voice recognition;
            RealtimeApiSdk.StartSpeechRecognitionAsync();
        }


        private void StopSpeechRecognition_Click(object sender, RoutedEventArgs e)
        {
            // Stop the ripple effect.
            StopRippleEffect();

            // Stop voice recognition;
            RealtimeApiSdk.StopSpeechRecognitionAsync();
        }

        private void StartRippleEffect()
        {
            // Ensure the ripples are visible.
            RippleEffect.Visibility = Visibility.Visible;

            // Clear previous animations before each launch.
            RippleScale.BeginAnimation(ScaleTransform.ScaleXProperty, null);
            RippleScale.BeginAnimation(ScaleTransform.ScaleYProperty, null);

            // Create an animation object.
            DoubleAnimation scaleAnimation = new DoubleAnimation
            {
                From = 1,
                To = 3,
                Duration = new Duration(TimeSpan.FromSeconds(1)),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };

            // Start animation.
            RippleScale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            RippleScale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }

        private void StopRippleEffect()
        {
            RippleEffect.Visibility = Visibility.Collapsed;

            // Remove the animation.
            RippleScale.BeginAnimation(ScaleTransform.ScaleXProperty, null);
            RippleScale.BeginAnimation(ScaleTransform.ScaleYProperty, null);
        }
    }
}
