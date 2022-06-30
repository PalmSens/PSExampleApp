using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Fonts;
using PdfSharpCore.Pdf;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Resolvers;
using System;
using System.Diagnostics;
using System.IO;

namespace PSHeavyMetal.Core.Services
{
    public class ShareService : IShareService
    {
        public ShareService()
        {
            GlobalFontSettings.FontResolver = new FileFontResolver();
        }

        public void CreatePdfFile(HeavyMetalMeasurement measurement, string filepath)
        {
            var document = new PdfDocument();
            document.Info.Title = measurement.Name;
            document.Info.Subject = measurement.Configuration.AnalyteName;

            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var tf = new XTextFormatter(gfx);

            var measurementTextInfo = $"Measurement is conducted at {measurement.MeasurementDate}\n" +
                $"Sample name: {measurement.Name}\n" +
                $"Sample Notes: {measurement.Configuration.Description}\n\n" +
                $"Analyte measured: {measurement.Configuration.AnalyteName}\n" +
                $"Measurement result: {measurement.Concentration} {measurement.Configuration.ConcentrationUnit}";

            try
            {
                var titleFont = new XFont("OpenSans", 25, XFontStyle.Bold);
                var normalFont = new XFont("OpenSans", 20, XFontStyle.Bold);

                gfx.DrawString("PS HeavyMetal Measurement report", titleFont, XBrushes.Black, new XRect(0, 20, page.Width, page.Height), XStringFormats.TopCenter);

                var backGroundRect = new XRect(0, 100, page.Width, 230);
                gfx.DrawRectangle(XBrushes.SkyBlue, backGroundRect);

                var textrect = new XRect(20, 120, page.Width, 200);
                gfx.DrawRectangle(XBrushes.Transparent, textrect);
                tf.Alignment = XParagraphAlignment.Left;
                tf.DrawString(measurementTextInfo, normalFont, XBrushes.Black, textrect, XStringFormats.TopLeft);

                for (int i = 0; i < Math.Min(measurement.MeasurementImages.Count, 3); i++)
                {
                    using (var stream = new MemoryStream(measurement.MeasurementImages[i]))
                    {
                        var pdfImage = XImage.FromStream(() => stream);

                        switch (i)
                        {
                            //Here the x and y magic numbers are the coordinates of the image being drawn. Bit ugly but there is no option just to
                            case 0:
                                gfx.DrawImage(pdfImage, 50, 350, 200, 200);
                                break;

                            case 1:
                                gfx.DrawImage(pdfImage, 350, 350, 200, 200);
                                break;

                            case 2:
                                gfx.DrawImage(pdfImage, 50, 575, 200, 200);
                                break;

                            case 3:
                                gfx.DrawImage(pdfImage, 350, 575, 200, 200);
                                break;

                            default:
                                break;
                        }
                    }
                }

                document.Save(filepath);
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }
    }
}