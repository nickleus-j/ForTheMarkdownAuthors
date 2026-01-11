
using Aspose.Words;
using System;
using System.IO;
using System.Text;

namespace MarkdownAuthoring.ComponentHelpers
{
    public class DocumentGenerator
    {
        /// <summary>
        /// Generates an ODT file from HTML + CSS.
        /// </summary>
        /// <param name="html">HTML content string</param>
        /// <param name="filePath">Target ODT file path</param>
        public static string GenerateOdtFromHtmlCss(string fullHtml, PaperSize givenSize,string filePath = "output.odt")
        {
            try
            {// Create ODT file
                Document doc = new Document();
                DocumentBuilder builder = new DocumentBuilder(doc);
                builder.PageSetup.PaperSize = givenSize;

                // Insert the HTML string
                builder.InsertHtml(fullHtml);
                // Save the document to ODT format
                doc.Save(filePath, SaveFormat.Odt);
                return "*";
            }
            catch (Exception ex) { return String.Empty; }
        }
    }
}
