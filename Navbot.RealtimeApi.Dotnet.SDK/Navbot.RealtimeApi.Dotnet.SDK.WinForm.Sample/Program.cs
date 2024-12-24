using Navbot.RealtimeApi.Dotnet.SDK.Desktop.Sample;
using Navbot.RealtimeApi.Dotnet.SDK.WinForm.Sample;

namespace Navbot.RealtimeApi.Dotnet.SDK.Destop.Sample
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new RealtimeForm());
        }
    }
}