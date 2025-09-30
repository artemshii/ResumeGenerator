using System.Reflection.Metadata;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using ResumeGenerator.Data.Interfaces;
using ResumeGenerator.Data.Models;
using ResumeGenerator.Data.Models.Exceptions;
using static System.Net.Mime.MediaTypeNames;

namespace ResumeGenerator.Data.ResumeStyles
{
    public class ColoredPdfGenerator : IPdfGenerator
    {
        public CompleteData _changedData { get; set; }

        public int designNumber { get; } = 3;
        public ColoredPdfGenerator(CompleteData changedData)
        {
            _changedData = changedData;

        }

        public PdfDocument StyleGenerator_1(CompleteData completeData)
        {
            PdfDocument pdfDocument = new PdfDocument();
            PdfPage page = pdfDocument.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont headerFont = new XFont("Arial", 20, XFontStyleEx.Bold);
            XFont jobTitleFont = new XFont("Arial", 12, XFontStyleEx.Bold);
            XFont subHeaderFont = new XFont("Arial", 8, XFontStyleEx.Italic);
            XFont sectionTitleFont = new XFont("Arial", 14, XFontStyleEx.Bold);
            XFont contentFont = new XFont("Arial", 10, XFontStyleEx.Regular);
            XFont contentFontLocation = new XFont("Arial", 10, XFontStyleEx.Bold);
            XFont contentFontYear = new XFont("Arial", 10, XFontStyleEx.Italic);
            XFont contentFontHeader = new XFont("Arial", 10, XFontStyleEx.Bold);
            XFont contentFontSubHeader = new XFont("Arial", 10, XFontStyleEx.Italic);

            double y = headerFont.Height + 10; // initial vertical position

            //TODO








            if (y >= page.Height.Point)
            {
                throw new PdfException("Data doesnt fit in 1 page");
            }





            using var stream = new MemoryStream();


            return pdfDocument;


        }




    }
}
