using PdfSharpCore.Fonts;
using PSHeavyMetal.Core.Services;
using System.IO;
using System.Reflection;

namespace PSHeavyMetal.Core.Resolvers
{
    public class FileFontResolver : IFontResolver // FontResolverBase
    {
        public string DefaultFontName => "OpenSans";

        public byte[] GetFont(string faceName)
        {
            if (faceName.Contains(DefaultFontName))
            {
                var assembly = typeof(ShareService).GetTypeInfo().Assembly;
                var stream = assembly.GetManifestResourceStream($"PSHeavyMetal.Core.Fonts.{faceName}.ttf");

                using (var reader = new StreamReader(stream))
                {
                    var bytes = default(byte[]);

                    using (var ms = new MemoryStream())
                    {
                        reader.BaseStream.CopyTo(ms);
                        bytes = ms.ToArray();
                    }

                    return bytes;
                }
            }
            else
                return GetFont(DefaultFontName);
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            var fontName = string.Empty;

            switch (familyName)
            {
                case "Open Sans":
                case "OpenSans":
                    fontName = "OpenSans";

                    if (isBold && isItalic)
                        fontName = $"{fontName}-BoldItalic";
                    else if (isBold)
                        fontName = $"{fontName}-Bold";
                    else if (isItalic)
                        fontName = $"{fontName}-Italic";
                    else
                        fontName = $"{fontName}-Regular";

                    return new FontResolverInfo(fontName);

                default:
                    break;
            }

            return null;
        }
    }
}