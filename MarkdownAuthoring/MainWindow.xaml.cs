﻿using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
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
        private string DefaultResultText = "Result Seen  below";
        public MainWindow()
        {
            InitializeComponent();
            ResultLabel.Text = DefaultResultText;
            SetUpPdfHelpers();
            SetupStyleContent();

            if (!String.IsNullOrEmpty(App.FileLocation))
            {
                LoadToBrowser(App.FileLocation);
            }
        }
        private void SetUpPdfHelpers()
        {
            pdfComponentHelper = new PdfComponentHelper();
            pdfComponentHelper.PopulatePageSizes(PageSizes);
        }
        private void SetupStyleContent()
        {
            fontBox.ItemsSource = Fonts.SystemFontFamilies.OrderBy(x=>x.Source);
            fontBox.SelectedValue = Fonts.SystemFontFamilies.SingleOrDefault(x => x.Source == "Verdana");
        }
        // Event handler for Markdown TextBox text changes
        private void MarkdownTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UpdatePreview();
        }
        private void fontBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                string result=pdfComponentHelper.SaveAsPdf(MarkdownTextToHtml(), selectedPageSize, saveFileDialog.FileName);
                PdfStatusLbl.Text = String.IsNullOrEmpty(result) ? "Error In generating Pdf" : String.Empty;
            }
        }
        private async void LoadToBrowser(string fileName)
        {
            await Task.Delay(500);
            string? fileContent = FileParser.ReturnTextContent(fileName);
            LoadContent(fileContent);
            
        }
        private void LoadFile(string fileName)
        {
            string? fileContent = FileParser.ReturnTextContent(fileName);
            LoadContent(fileContent);
        }
        private void LoadContent(string? fileContent)
        {
            if (String.IsNullOrEmpty(fileContent))
            {
                FileStatusLbl.Text = "Error in Opening File";
            }
            else
            {
                MarkdownTextBox.Text = fileContent;
                FileStatusLbl.Text = String.Empty;
            }
        }
        private void OpenTextButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog{ Filter = "Text file (*.txt)|*.txt|Markdown file(*.md,*.mkd)|*.md;*.mkd", DefaultExt = ".txt" };
            if (openFile.ShowDialog() == true)
            {
                LoadFile(openFile.FileName);
            }
        }
        private void SaveTextButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog {
                Filter = "Text files (*.txt)|*.txt|Markdown files(*.md,*.mkd)|*.md;*.mkd",
                DefaultExt = ".txt",
                Title = "Save text file"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                FileParser.WriteTextToFile(saveFileDialog.FileName, MarkdownTextBox.Text);
            }
        }
        
        public string GetCssCode()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("body{font-family: '");
            sb.Append(fontBox.SelectedValue.ToString());
            sb.Append("'}");
            sb.Append("a{color: '");
            sb.Append(Linkcolor.ColorName);
            sb.Append("'}");
            sb.Append("ul{list-style: '");
            sb.Append(defaultBullet.ResultStyle);
            sb.Append("'}");
            return sb.ToString();
        }
        public string MarkdownTextToHtml()
        {
            string markdownText = MarkdownTextBox.Text;
            string htmlContent = Markdown.ToHtml(markdownText); // Convert markdown to HTML using Markdig
            return $"<html><head><style>{GetCssCode()}</style></head><body>{htmlContent}</body></html>";
        }
        /// <summary>
        /// Update preview by converting Markdown to HTML
        /// </summary>
        private void UpdatePreview()
        {
            string markdownText = MarkdownTextBox.Text;
            string htmlContent = Markdown.ToHtml(markdownText); // Convert markdown to HTML using Markdig
            
            PreviewBrowser.NavigateToString(MarkdownTextToHtml());
            if (IsValidHtmlContent(htmlContent))
            {
                ResultLabel.Text = DefaultResultText;
            }
            else
            {
                ResultLabel.Text = "Invalid markup Content";
            }
        }

        /// <summary>
        /// Helper method to insert text at cursor position in TextBox
        /// </summary>
        /// <param name="text"></param>
        private void InsertTextAtCursor(string text)
        {
            var selectionStart = MarkdownTextBox.SelectionStart;
            MarkdownTextBox.Text = MarkdownTextBox.Text.Insert(selectionStart, text);
            MarkdownTextBox.SelectionStart = selectionStart + text.Length;
        }

        //Toolbar button click events
        private void Bold_Click(object sender, RoutedEventArgs e) => InsertTextAtCursor("**Bold Text**");
        private void Italic_Click(object sender, RoutedEventArgs e) => InsertTextAtCursor("_Italic Text_");
        private void Heading_Click(object sender, RoutedEventArgs e) => InsertTextAtCursor("# Headline");
        private void Link_Click(object sender, RoutedEventArgs e) => InsertTextAtCursor("[Link Text](http://bing.com)");
        private void Image_Click(object sender, RoutedEventArgs e) => InsertTextAtCursor("\n![Alt Text](https://en.wikipedia.org/static/images/icons/wikipedia.png)");
        private void List_Click(object sender, RoutedEventArgs e) => InsertTextAtCursor("- List item");
        private void Blockquotes_Click(object sender, RoutedEventArgs e) => InsertTextAtCursor(">>Blockquotes\n>> ");

        private void Linkcolor_ChangeColor(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        private void defaultBullet_ChangeStyle(object sender, EventArgs e)
        {
            UpdatePreview();
        }
        public bool IsValidHtmlContent(string htmlOutput)
        {
            if (string.IsNullOrWhiteSpace(htmlOutput)) return false;

            try
            {
                // If conversion succeeds, assume valid Markdown
                return !string.IsNullOrWhiteSpace(htmlOutput);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
