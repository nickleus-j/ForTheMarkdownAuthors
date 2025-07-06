using System.Configuration;
using System.Data;
using System.Windows;

namespace MarkdownAuthoring
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string FileLocation { get; set; }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // If a file path is passed as an argument (e.g., dragged onto EXE)
            if (e.Args.Length > 0)
            {
                FileLocation = e.Args[0];
            }

        }
    }

}
