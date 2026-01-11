using System;
using PdfSharp; // PdfSharp.PageSize
using Aspose.Words; // Aspose.Words.PaperSize (adjust if using Aspose.Pdf)

namespace MarkdownAuthoring.ComponentHelpers
{
    public static class PageSizeMapper
    {
        public static PaperSize MapPdfSharpToAspose(PdfSharp.PageSize pdfSharpSize)
        {
            switch (pdfSharpSize)
            {
                case PageSize.A4: return PaperSize.A4;
                case PdfSharp.PageSize.A3: return PaperSize.A3;
                case PdfSharp.PageSize.A5: return PaperSize.A5;
                case PdfSharp.PageSize.Letter: return PaperSize.Letter;
                case PdfSharp.PageSize.Legal: return PaperSize.Legal;
                case PdfSharp.PageSize.Tabloid: return PaperSize.Tabloid;
                case PdfSharp.PageSize.Executive: return PaperSize.Executive;
                default: // Fallback if no direct mapping exists
                    return PaperSize.A4;
            }
        }
    }
}
