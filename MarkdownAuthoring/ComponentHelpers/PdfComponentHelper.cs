using PdfSharp;
using PdfSharp.Pdf;
using System.Text;
using System.Windows.Controls;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace MarkdownAuthoring.ComponentHelpers
{
    public class PdfComponentHelper
    {
        ComboBox PageSizeBox;
        public void PopulatePageSizes(ComboBox PageSizes)
        {
            foreach (PageSize pageSize in Enum.GetValues(typeof(PageSize)))
            {
                if (pageSize != PageSize.Undefined)
                {
                    PageSizes.Items.Add(pageSize);
                }
            }
            PageSizes.SelectedItem = PageSize.Letter;
            PageSizeBox=PageSizes;
        }
        public string SaveAsPdf(string htmlContent,string fileName= "output.pdf")
        {
            return SaveAsPdf(htmlContent, GetPageSize(),fileName);
        }
        public string SaveAsPdf(string htmlContent, PageSize selectedPageSize,string fileName = "output.pdf")
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                PdfDocument pdf = PdfGenerator.GeneratePdf(htmlContent, selectedPageSize);
                pdf.Save(fileName);
                pdf.Close();
            }
            catch (Exception ex) { return String.Empty; }
            return "*";
        }
        public PageSize GetPageSize()
        {
            return PageSizeBox != null && !PageSizeBox.SelectedItem.Equals(PageSize.Undefined) ? (PageSize)PageSizeBox.SelectedItem : PageSize.Letter;
        }
    }
}
