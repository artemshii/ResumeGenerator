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
    public class ClassicPdfGenerator : IPdfGenerator
    {
        public CompleteData _changedData { get; set; }

        public int designNumber { get; } = 1;
        public ClassicPdfGenerator(CompleteData changedData)
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

            gfx.DrawString(completeData.Name ?? "Example Name", headerFont, XBrushes.Black,
                        new XRect(0, 0, page.Width.Point, y), XStringFormats.Center);



            gfx.DrawString(completeData.JobTitle ?? "Example JobTitle", jobTitleFont, XBrushes.Black,
                            new XRect(0, y, page.Width.Point, y + jobTitleFont.Height + 5), XStringFormats.TopCenter);

            y += jobTitleFont.Height + 5;


            gfx.DrawString($"{completeData.PhoneNumber}  |  {completeData.Email}  |  {completeData.LinkedIn}", 
                        subHeaderFont, XBrushes.Black,
                        new XRect(0, y, page.Width.Point, y + subHeaderFont.Height + 5), XStringFormats.TopCenter);

            y += 25;

            gfx.DrawString("Work Experience", sectionTitleFont, XBrushes.Black, 
                          new XRect(30, y, page.Width.Point, y + sectionTitleFont.Height + 5), XStringFormats.TopLeft);

            y += sectionTitleFont.Height + 5;

            gfx.DrawLine(XPens.Black, 30, y, (page.Width.Point - 60), y+ 0.01);

            XTextFormatter tf = new XTextFormatter(gfx);

            XStringFormat rightAlign = new XStringFormat
            {
                Alignment = XStringAlignment.Far,          // horizontal = right
                LineAlignment = XLineAlignment.Near        // vertical = top
            };

            if(completeData.ProffesionalExperience == null)
            {
                throw new NotFoundException("Please fill in the Professional Experience");
            }

            completeData.ProffesionalExperience.Reverse();

            foreach (var item in completeData.ProffesionalExperience)
            {
                if(item.CompanyDescription == null)
                {
                    throw new NotFoundException("Please fill in the description");
                }
                gfx.DrawString(item.CompanyName ?? "Example Company Name", contentFontHeader, XBrushes.Black, new XRect(30, y, page.Width.Point - 60, y + contentFont.Height), XStringFormats.TopLeft);
                gfx.DrawString($"{item.Location}", contentFontLocation, XBrushes.Black, new XRect(30, y, page.Width.Point - 60, y + contentFont.Height), rightAlign);
                y += contentFont.Height;
                gfx.DrawString(item.Position ?? "Example Position", contentFontSubHeader, XBrushes.Black, new XRect(30, y, page.Width.Point - 60, y + contentFont.Height), XStringFormats.TopLeft);
                gfx.DrawString($"{item.YearStart} -  {item.YearEnd}", contentFontYear, XBrushes.Black, new XRect(30, y, page.Width.Point - 60, y + contentFont.Height), rightAlign);
                y += contentFont.Height;

                tf.DrawString($"• {item.CompanyDescription}", contentFont, XBrushes.Black, new XRect(40, y, page.Width.Point - 80, y + 100), XStringFormats.TopLeft);



                if (item.CompanyDescription.Length / 90 < 1)
                {
                    y += contentFont.Height + 10 ;
                }
                else
                {
                    y += contentFont.Height * (item.CompanyDescription.Length / 90) + 10;
                }

            }

            y += 15;

            gfx.DrawString("Education", sectionTitleFont, XBrushes.Black,
                          new XRect(30, y, page.Width.Point, y + sectionTitleFont.Height + 5), XStringFormats.TopLeft);

            y += sectionTitleFont.Height + 5;

            gfx.DrawLine(XPens.Black, 30, y, page.Width.Point - 60, y + 0.01);
            if (completeData.Education == null)
            {
                throw new NotFoundException("Please fill in the Professional Experience");
            }

            completeData.Education.Reverse();

            foreach (var item in completeData.Education)
            {
                if (item.Description == null)
                {
                    throw new NotFoundException("Please fill in the description");
                }
                gfx.DrawString(item.InstitutionName ?? "Example Name", contentFontHeader, XBrushes.Black, new XRect(30, y, page.Width.Point - 60, y + contentFont.Height), XStringFormats.TopLeft);
                gfx.DrawString($"{item.Location}", contentFontLocation, XBrushes.Black, new XRect(30, y, page.Width.Point - 60, y + contentFont.Height), rightAlign);
                y += contentFont.Height;
                gfx.DrawString(item.Degree ?? "Example Degree", contentFontSubHeader, XBrushes.Black, new XRect(30, y, page.Width.Point - 60, y + contentFont.Height), XStringFormats.TopLeft);
                gfx.DrawString($"{item.YearStart} -  {item.YearEnd}", contentFontYear, XBrushes.Black, new XRect(30, y, page.Width.Point - 60, y + contentFont.Height), rightAlign);
                y += contentFont.Height;

                tf.DrawString($"• {item.Description}", contentFont, XBrushes.Black, new XRect(40, y, page.Width.Point - 80, y + 100), XStringFormats.TopLeft);

                if (item.Description.Length / 90 < 1)
                {
                    y += contentFont.Height + 10;
                }
                else
                {
                    y += contentFont.Height * (item.Description.Length / 90) + 10;
                }

            }

            y += 15;

            gfx.DrawString("Skills", sectionTitleFont, XBrushes.Black,
                          new XRect(30, y, page.Width.Point, y + sectionTitleFont.Height + 5), XStringFormats.TopLeft);

            y += sectionTitleFont.Height + 5;



            gfx.DrawLine(XPens.Black, 30, y, page.Width.Point - 60, y + 0.01);

            y += 3;

            if(completeData.Skills == null)
            {
                throw new PdfException("Please fill in at least one skill");
            }

            string skills = string.Join(", ", completeData.Skills);


            tf.DrawString($"Skills: {skills}", contentFont, XBrushes.Black,
                          new XRect(35, y, page.Width.Point - 70, y + 100), XStringFormats.TopLeft);

            if (skills.Length / 90 < 1)
            {
                y += contentFont.Height + 5;
            }
            else
            {
                y += contentFont.Height * (skills.Length / 90) + 5;
            }

            if (completeData.Languages == null)
            {
                throw new PdfException("Please fill in at least one language");
            }
            string languages = string.Join(", ", completeData.Languages);

            tf.DrawString($"Languages: {languages}", contentFont, XBrushes.Black,
                          new XRect(35, y, page.Width.Point - 70, y + 100), XStringFormats.TopLeft);









            if(y >= page.Height.Point)
            {
                throw new PdfException("Data doesnt fit in 1 page");
            }




            // 2. Save to MemoryStream
            using var stream = new MemoryStream();
            

            return pdfDocument;

            
        }


        

    }
}
