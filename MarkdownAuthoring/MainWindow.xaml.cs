using System;
using System.Collections.Generic;
using System.Linq;
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
        private PdfComponentHelper _pdfComponentHelper;
        private const string DefaultPreviewText = "Preview";

        public MainWindow()
        {
            InitializeComponent();
            InitializeApplication();
        }

        private void InitializeApplication()
        {
            SetUpPdfHelpers();
            SetupStyleContent();

            if (!string.IsNullOrEmpty(App.FileLocation))
            {
                LoadToBrowser(App.FileLocation);
            }
        }

        /// <summary>
        /// Initialize PDF export helper and populate page size options
        /// </summary>
        private void SetUpPdfHelpers()
        {
            _pdfComponentHelper = new PdfComponentHelper();
            _pdfComponentHelper.PopulatePageSizes(PageSizeSelector);
        }

        /// <summary>
        /// Populate available system fonts
        /// </summary>
        private void SetupStyleContent()
        {
            FontBox.ItemsSource = Fonts.SystemFontFamilies.OrderBy(x => x.Source);
            FontBox.SelectedValue = Fonts.SystemFontFamilies.SingleOrDefault(x => x.Source == "Verdana");
        }

        /// <summary>
        /// Update preview when markdown text changes
        /// </summary>
        private void MarkdownTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePreview();
        }

        /// <summary>
        /// Update preview when font selection changes
        /// </summary>
        private void FontBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontBox.SelectedItem != null)
            {
                UpdatePreview();
            }
        }

        /// <summary>
        /// Update preview when blockquote font size changes
        /// </summary>
        private void BlockquoteFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePreview();
        }

        /// <summary>
        /// Export markdown as PDF document
        /// </summary>
        private void SaveAsPdfButton_Click(object sender, RoutedEventArgs e)
        {
            PageSize selectedPageSize = (PageSize)PageSizeSelector.SelectedItem;
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                DefaultExt = ".pdf",
                Title = "Save PDF Document"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string result = _pdfComponentHelper.SaveAsPdf(
                    MarkdownTextToHtml(),
                    selectedPageSize,
                    saveFileDialog.FileName
                );

                ExportStatusLabel.Text = string.IsNullOrEmpty(result)
                    ? "Error generating PDF"
                    : string.Empty;
            }
        }

        /// <summary>
        /// Export markdown as ODT document
        /// </summary>
        private void SaveAsOdtButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "ODT files (*.odt)|*.odt",
                DefaultExt = ".odt",
                Title = "Save ODT Document"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string result = DocumentGenerator.GenerateOdtFromHtmlCss(
                    MarkdownTextToHtml(),
                    PageSizeMapper.MapPdfSharpToAspose((PageSize)PageSizeSelector.SelectedItem),
                    saveFileDialog.FileName
                );

                ExportStatusLabel.Text = string.IsNullOrEmpty(result)
                    ? "Error generating ODT"
                    : string.Empty;
            }
        }

        /// <summary>
        /// Load markdown file from path and display in editor
        /// </summary>
        private async void LoadToBrowser(string fileName)
        {
            await Task.Delay(500);
            string fileContent = FileParser.ReturnTextContentOfFile(fileName);
            LoadContent(fileContent);
        }

        /// <summary>
        /// Load markdown file content
        /// </summary>
        private void LoadFile(string fileName)
        {
            string fileContent = FileParser.ReturnTextContentOfFile(fileName);
            LoadContent(fileContent);
        }

        /// <summary>
        /// Load content into markdown editor and update preview
        /// </summary>
        private void LoadContent(string fileContent)
        {
            if (string.IsNullOrEmpty(fileContent))
            {
                FileStatusLabel.Text = "Error opening file";
            }
            else
            {
                MarkdownTextBox.Text = fileContent;
                FileStatusLabel.Text = string.Empty;
            }
        }

        /// <summary>
        /// Open file dialog and load markdown file
        /// </summary>
        private void OpenTextButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog
            {
                Filter = "Text file (*.txt)|*.txt|Markdown file(*.md,*.mkd)|*.md;*.mkd",
                DefaultExt = ".txt"
            };

            if (openFile.ShowDialog() == true)
            {
                LoadFile(openFile.FileName);
            }
        }

        /// <summary>
        /// Save markdown content to file
        /// </summary>
        private void SaveTextButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|Markdown files(*.md,*.mkd)|*.md;*.mkd",
                DefaultExt = ".txt",
                Title = "Save text file"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                FileParser.WriteTextToFile(saveFileDialog.FileName, MarkdownTextBox.Text);
            }
        }

        /// <summary>
        /// Generate CSS styling based on user preferences
        /// </summary>
        private string GetCssCode()
        {
            StringBuilder sb = new StringBuilder();

            // Font styling
            sb.Append("body { font-family: '");
            sb.Append(FontBox.SelectedValue?.ToString() ?? "Arial");
            sb.Append("'; }");

            // Link color
            sb.Append("a { color: ");
            sb.Append(LinkColorPicker.ColorName ?? "#0066cc");
            sb.Append("; }");

            // Bullet style
            sb.Append("ul { list-style: ");
            sb.Append(DefaultBulletStyle.ResultStyle ?? "disc");
            sb.Append("; }");

            // Blockquote styling
            if (BlockquoteFontSize.SelectedItem != null)
            {
                sb.Append("blockquote { text-align: justify; font-size: ");
                sb.Append(((ComboBoxItem)BlockquoteFontSize.SelectedItem).Content);
                sb.Append("; }");
            }

            // Underline headings
            if (UnderlineHeadings.IsChecked == true)
            {
                sb.Append("h1, h2, h3, h4, h5, h6 { text-decoration: underline; }");
            }

            // Table borders
            sb.Append("table, td, th { border: 1px solid black; }");

            return sb.ToString();
        }

        /// <summary>
        /// Convert markdown text to HTML with styling
        /// </summary>
        private string MarkdownTextToHtml()
        {
            string markdownText = MarkdownTextBox.Text;
            var pipeline = new MarkdownPipelineBuilder().UsePipeTables().Build();
            string htmlContent = Markdown.ToHtml(markdownText, pipeline);
            return $"<html><head><style>{GetCssCode()}</style></head><body>{htmlContent}</body></html>";
        }

        /// <summary>
        /// Update browser preview with converted HTML
        /// </summary>
        private void UpdatePreview()
        {
            // Guard clause: ensure required controls are initialized
            if (MarkdownTextBox == null || PreviewBrowser == null || PreviewLabel == null)
            {
                return;
            }

            string markdownText = MarkdownTextBox.Text;
            string htmlContent = Markdown.ToHtml(markdownText);

            PreviewBrowser.NavigateToString(MarkdownTextToHtml());

            PreviewLabel.Text = IsValidHtmlContent(htmlContent)
                ? DefaultPreviewText
                : "Invalid markup content";
        }

        /// <summary>
        /// Insert markdown syntax at cursor position
        /// </summary>
        private void InsertTextAtCursor(string text)
        {
            int selectionStart = MarkdownTextBox.SelectionStart;
            MarkdownTextBox.Text = MarkdownTextBox.Text.Insert(selectionStart, text);
            MarkdownTextBox.SelectionStart = selectionStart + text.Length;
            MarkdownTextBox.Focus();
        }

        /// <summary>
        /// Find and replace text in markdown editor
        /// </summary>
        private void ReplaceButton_Click(object sender, RoutedEventArgs e)
        {
            string text = MarkdownTextBox.Text;
            string find = FindText.Text;
            string replace = ReplaceText.Text;

            if (string.IsNullOrEmpty(find))
            {
                ReplaceStatus.Text = "Enter text to find";
                return;
            }

            if (text.Contains(find))
            {
                MarkdownTextBox.Text = text.Replace(find, replace);
                ReplaceStatus.Text = "Replaced successfully";
                ReplaceStatus.Foreground = Brushes.Green;
            }
            else
            {
                ReplaceStatus.Text = "No matches found";
                ReplaceStatus.Foreground = Brushes.Red;
            }
        }

        // Toolbar formatting button click handlers
        private void Bold_Click(object sender, RoutedEventArgs e)
            => InsertTextAtCursor("**Bold Text**");

        private void Italic_Click(object sender, RoutedEventArgs e)
            => InsertTextAtCursor("_Italic Text_");

        private void Heading_Click(object sender, RoutedEventArgs e)
            => InsertTextAtCursor("# Heading\n");

        private void Link_Click(object sender, RoutedEventArgs e)
            => InsertTextAtCursor("[Link Text](https://example.com)");

        private void Image_Click(object sender, RoutedEventArgs e)
            => InsertTextAtCursor("\n![Alt Text](https://example.com/image.png)\n");

        private void List_Click(object sender, RoutedEventArgs e)
            => InsertTextAtCursor("- List item\n");

        private void Blockquotes_Click(object sender, RoutedEventArgs e)
            => InsertTextAtCursor("> Blockquote\n");

        // Style panel event handlers
        private void LinkColorPicker_ChangeColor(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        private void DefaultBulletStyle_ChangeStyle(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        private void UnderlineHeadings_Checked(object sender, RoutedEventArgs e)
        {
            UpdatePreview();
        }

        private void UnderlineHeadings_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdatePreview();
        }

        /// <summary>
        /// Validate HTML content
        /// </summary>
        private bool IsValidHtmlContent(string htmlOutput)
        {
            return !string.IsNullOrWhiteSpace(htmlOutput);
        }
    }
}
