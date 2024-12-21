using log4net;
using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows;

namespace Navbot.RealtimeApi.Dotnet.SDK.WPF.Sample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public App()
        {
            string logDirectory = "logs";
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo("log4net.config"));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            log.Info("Application started.");
        }
    }

}
