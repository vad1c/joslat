using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Realtime.API.Dotnet.SDK.WPF
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class WpfUserControl : UserControl
    {
        public WpfUserControl()
        {
            InitializeComponent();

            this.StartSpeechRecognition.Click += StartSpeechRecognition_Click;
            this.StopSpeechRecognition.Click += StopSpeechRecognition_Click;
            
        }

        private void StopSpeechRecognition_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void StartSpeechRecognition_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }

}
