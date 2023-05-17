using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace PSExampleApp.Forms.Resources
{
    public class ResourceHelper
    {
        public static byte[] GetImageAsByteArray(string resourceName)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }
    }
}
