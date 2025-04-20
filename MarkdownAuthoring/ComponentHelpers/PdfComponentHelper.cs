using PdfSharp;
using PdfSharp.Pdf;
using System.Text;
using System.Threading.Tasks;
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
        public void SaveAsPdf(string htmlContent,string fileName= "output.pdf")
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            PdfDocument pdf = PdfGenerator.GeneratePdf(htmlContent, GetPageSize());
            pdf.Save(fileName);
            pdf.Close();
        }
        public PageSize GetPageSize()
        {
            return PageSizeBox != null && !PageSizeBox.SelectedItem.Equals(PageSize.Undefined) ? (PageSize)PageSizeBox.SelectedItem : PageSize.Letter;
        }
    }
}
