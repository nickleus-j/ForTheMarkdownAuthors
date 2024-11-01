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
using Cake.Asciidoctor;

namespace WriteInAsciiDoc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void PreviewButton_Click(object sender, RoutedEventArgs e)
        {
            string asciidocContent = AsciiDocTextBox.Text;
            string htmlContent = ConvertAsciiDocToHtml(asciidocContent);
            PreviewBrowser.NavigateToString(htmlContent);
        }

        private string ConvertAsciiDocToHtml(string asciidocContent)
        {
            var asciidoctor = new Ascii();
            return asciidoctor.Convert(asciidocContent, new { safe = "safe" });
        }
    }
}