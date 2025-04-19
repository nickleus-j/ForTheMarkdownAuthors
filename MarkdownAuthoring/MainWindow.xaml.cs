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
using MarkdownAuthoring.ComponentHelpers;
using Microsoft.Win32;
using PdfSharp;

namespace MarkdownAuthoring
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PdfComponentHelper pdfComponentHelper;
        public MainWindow()
        {
            InitializeComponent();
            SetUpPdfHelpers();
        }
        private void SetUpPdfHelpers()
        {
            pdfComponentHelper = new PdfComponentHelper();
            pdfComponentHelper.PopulatePageSizes(PageSizes);
        }

        // Event handler for Markdown TextBox text changes
        private void MarkdownTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UpdatePreview();
        }
        private void SaveAsPdfButton_Click(object sender, RoutedEventArgs e)
        {
            PageSize selectedPageSize = (PageSize)PageSizes.SelectedItem;
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                DefaultExt = ".pdf",
                Title = "Save PDF Document"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                pdfComponentHelper.SaveAsPdf(MarkdownTextToHtml(), saveFileDialog.FileName);
            }
        }
        private void OpenTextButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog{ DefaultExt = ".txt" };
            if (openFile.ShowDialog() == true)
            {
                MarkdownTextBox.Text = FileParser.ReturnTextContent(openFile.FileName);
            }
        }
        private void SaveTextButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog {
                Filter = "Text files (*.txt)|*.txt",
                DefaultExt = ".txt",
                Title = "Save text file"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                FileParser.WriteTextToFile(saveFileDialog.FileName, MarkdownTextBox.Text);
            }
        }
        public string MarkdownTextToHtml()
        {
            string markdownText = MarkdownTextBox.Text;
            string htmlContent = Markdown.ToHtml(markdownText); // Convert markdown to HTML using Markdig
            return $"<html><body>{htmlContent}</body></html>";
        }
        //Update preview by converting Markdown to HTML
        private void UpdatePreview()
        {
            string markdownText = MarkdownTextBox.Text;
            string htmlContent = Markdown.ToHtml(markdownText); // Convert markdown to HTML using Markdig
            
            PreviewBrowser.NavigateToString(MarkdownTextToHtml());
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
