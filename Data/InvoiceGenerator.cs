using System.Drawing.Configuration;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using ResumeGenerator.Data.Interfaces;

namespace ResumeGenerator.Data
{
    public class InvoiceGenerator : IInvoiceGenerator
    {

        async public Task<PdfDocument> Generate(string price, string sessionId, string email)
        {
            PdfDocument pdfDocument = new PdfDocument();
            PdfPage page = pdfDocument.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont headerFont = new XFont("Arial", 20, XFontStyleEx.Regular);
            XFont contentFont = new XFont("Arial", 10, XFontStyleEx.Regular);

            

            double y = 20;

            gfx.DrawLine(XPens.Black, 30, y, page.Width.Point - 100, y + 0.01);
            gfx.DrawString("Invoice", headerFont, XBrushes.Black,
                          new XRect(page.Width.Point - 95, y-10, page.Width.Point, y+5), XStringFormats.TopLeft);

            y += 100;

            gfx.DrawString("Issued to", contentFont, XBrushes.Black,
                        new XRect(40, y, page.Width.Point, y + 10), XStringFormats.TopLeft);
            gfx.DrawString("Product", contentFont, XBrushes.Black,
                        new XRect(190, y, page.Width.Point, y + 10), XStringFormats.TopLeft);
            gfx.DrawString("Amount", contentFont, XBrushes.Black,
                        new XRect(340, y, page.Width.Point, y + 10), XStringFormats.TopLeft);
            gfx.DrawString("Price", contentFont, XBrushes.Black,
                        new XRect(490, y, page.Width.Point, y + 10), XStringFormats.TopLeft);

            gfx.DrawLine(XPens.Black, 40, y + 11, 530, y + 11.01);

            y += 50;

            gfx.DrawString(email, contentFont, XBrushes.Black,
                        new XRect(40, y, page.Width.Point, y + 10), XStringFormats.TopLeft);
            gfx.DrawString("Usage", contentFont, XBrushes.Black,
                        new XRect(190, y, page.Width.Point, y + 10), XStringFormats.TopLeft);
            gfx.DrawString("10", contentFont, XBrushes.Black,
                        new XRect(340, y, page.Width.Point, y + 10), XStringFormats.TopLeft);
            gfx.DrawString($"{price} Euro", contentFont, XBrushes.Black,
                        new XRect(490, y, page.Width.Point, y + 10), XStringFormats.TopLeft);

            y += 10;

            gfx.DrawLine(XPens.Black, 40, y + 11, 530, y + 11.01);


            y += 50;

            gfx.DrawString($"Session Id: {sessionId}", contentFont, XBrushes.Black,
                            new XRect(35, y, page.Width.Point, y+20), XStringFormats.TopLeft);

            using var stream = new MemoryStream();



            return pdfDocument;
        }
    }
}
