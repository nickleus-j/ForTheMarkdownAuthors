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
using Markdig;

namespace MarkdownAuthoring
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
            string markdownContent = MarkdownTextBox.Text;
            string htmlContent = ConvertMarkdownToHtml(markdownContent);
            PreviewBrowser.NavigateToString(htmlContent);
        }

        private string ConvertMarkdownToHtml(string markdownContent)
        {
            return Markdown.ToHtml(markdownContent);
        }
    }
}