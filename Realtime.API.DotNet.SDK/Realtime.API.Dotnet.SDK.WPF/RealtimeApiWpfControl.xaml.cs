using Realtime.API.Dotnet.SDK.Core;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Realtime.API.Dotnet.SDK.WPF
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class RealtimeApiWpfControl : UserControl
    {
        private const string apiKey = "";
        private VisualEffect voiceVisualEffect;

        public RealtimeApiWpfControl()
        {
            InitializeComponent();

            RealtimeApiSdk = new RealtimeApiSdk();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //RealtimeApiSdk.TransactionOccurred += RealtimeApiSdk_TransactionOccurred;

            voiceVisualEffect = WPF.VisualEffect.SoundWave;
            RippleEffect.Visibility = Visibility.Hidden;
            WaveformContainer.Visibility = Visibility.Hidden;
        }
 
        public RealtimeApiSdk RealtimeApiSdk { get; private set; }

        public VisualEffect VoiceVisualEffect
        {
            get { return voiceVisualEffect; }
            set { voiceVisualEffect = value; }
        }

        public string OpenAiApiKey
        {
            get { return RealtimeApiSdk.ApiKey; }
            set { RealtimeApiSdk.ApiKey = value; }
        }

        private void RealtimeApiSdk_TransactionOccurred(object? sender, TransactionOccurredEventArgs e)
        {
            Console.WriteLine(e.Message);
            //Dispatcher.Invoke(() => chatMessages.Add(e.Message));
        }

        public void StartSpeechRecognition()
        {
            if (!RealtimeApiSdk.IsRunning)
            {
                // Start ripple effect.
                PlayVisualVoiceEffect(true);

                // Start voice recognition;
                RealtimeApiSdk.StartSpeechRecognitionAsync();
            }
        }


        public void StopSpeechRecognition()
        {
            if (RealtimeApiSdk.IsRunning)
            {
                // Stop the ripple effect.
                PlayVisualVoiceEffect(false);

                // Stop voice recognition;
                RealtimeApiSdk.StopSpeechRecognitionAsync();
            }
        }

        private void PlayVisualVoiceEffect(bool enable)
        {
            switch (voiceVisualEffect)
            {
                case WPF.VisualEffect.Cycle:
                    HandleCycleVisualVoiceEffect(enable);
                    break;
                case WPF.VisualEffect.SoundWave:
                    HandleWaveVisualVoiceEffect(enable);
                    break;
                default:
                    break;
            }
        }


        private void HandleCycleVisualVoiceEffect(bool enable)
        {
            if (enable)
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
            else
            {
                RippleEffect.Visibility = Visibility.Collapsed;

                // Remove the animation.
                RippleScale.BeginAnimation(ScaleTransform.ScaleXProperty, null);
                RippleScale.BeginAnimation(ScaleTransform.ScaleYProperty, null);
            }
        }

        private void HandleWaveVisualVoiceEffect(bool enable)
        {
            if (enable)
            {
                WaveformContainer.Visibility = Visibility.Visible;
                foreach (var child in WaveformContainer.Children)
                {
                    if (child is Rectangle rect)
                    {
                        AnimateRectangleHeight(rect);
                    }
                }
            }
            else 
            {
                WaveformContainer.Visibility = Visibility.Collapsed;
                foreach (var child in WaveformContainer.Children)
                {
                    if (child is Rectangle rect)
                    {
                        rect.BeginAnimation(Rectangle.HeightProperty, null);
                    }
                }
            }
        }

        private void AnimateRectangleHeight(Rectangle rect)
        {
             Random _random = new Random();
            DoubleAnimation heightAnimation = new DoubleAnimation
            {
                From = 10,
                To = _random.Next(50, 150), 
                Duration = TimeSpan.FromMilliseconds(300),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };

            rect.BeginAnimation(Rectangle.HeightProperty, heightAnimation);
        }
    }
}
