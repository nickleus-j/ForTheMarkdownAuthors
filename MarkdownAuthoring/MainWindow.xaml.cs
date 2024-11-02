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
using PdfSharp;

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
            PopulatePageSizes();
        }
        private void PopulatePageSizes()
        {
            foreach (PageSize pageSize in Enum.GetValues(typeof(PageSize)))
            {
                if (pageSize != PageSize.Undefined)
                {
                    PageSizes.Items.Add(pageSize);
                }
            }
            PageSizes.SelectedIndex = 0; // Select the first item by default
        }

        // Event handler for Markdown TextBox text changes
        private void MarkdownTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UpdatePreview();
        }

        //Update preview by converting Markdown to HTML
        private void UpdatePreview()
        {
            string markdownText = MarkdownTextBox.Text;
            string htmlContent = Markdown.ToHtml(markdownText); // Convert markdown to HTML using Markdig
            
            PreviewBrowser.NavigateToString($"<html><body>{htmlContent}</body></html>");
        }

        //// Helper method to insert text at cursor position in TextBox
        private void InsertTextAtCursor(string text)
        {
            var selectionStart = MarkdownTextBox.SelectionStart;
            MarkdownTextBox.Text = MarkdownTextBox.Text.Insert(selectionStart, text);
            MarkdownTextBox.SelectionStart = selectionStart + text.Length;
            UpdatePreview();
        }

        //Toolbar button click events
        private void Bold_Click(object sender, RoutedEventArgs e) => InsertTextAtCursor("**Bold Text**");
        private void Italic_Click(object sender, RoutedEventArgs e) => InsertTextAtCursor("_Italic Text_");
        private void Heading_Click(object sender, RoutedEventArgs e) => InsertTextAtCursor("# Heading");
        private void Link_Click(object sender, RoutedEventArgs e) => InsertTextAtCursor("[Link Text](http://bing.com)");
        private void List_Click(object sender, RoutedEventArgs e) => InsertTextAtCursor("- List item");
    }
}
