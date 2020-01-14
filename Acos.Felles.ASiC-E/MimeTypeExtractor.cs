using System;
using Acos.Felles.ASiCE.Model;

namespace Acos.Felles.ASiCE
{
    public static class MimeTypeExtractor
    {
        public static MimeType ExtractMimeType(string fileName)
        {
            var file = fileName ?? throw new ArgumentNullException(nameof(fileName));
            return MimeType.ForString(ExtractType(file));
        }

        private static string ExtractType(string fileName)
        {
            return MimeTypes.GetMimeType(fileName);
        }
    }
}