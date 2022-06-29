using PdfSharpCore.Drawing;
using PdfSharpCore.Fonts;
using PdfSharpCore.Pdf;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Resolvers;
using System.Diagnostics;

namespace PSHeavyMetal.Core.Services
{
    public class ShareService : IShareService
    {
        public void CreatePdfFile(HeavyMetalMeasurement measurement, string filepath)
        {
            var document = new PdfDocument();
            document.Info.Title = measurement.Name;
            document.Info.Subject = measurement.Configuration.AnalyteName;

            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            GlobalFontSettings.FontResolver = new FileFontResolver();

            try
            {
                XFont font = new XFont("OpenSans", 10, XFontStyle.Bold);

                gfx.DrawString(measurement.Name, font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);

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