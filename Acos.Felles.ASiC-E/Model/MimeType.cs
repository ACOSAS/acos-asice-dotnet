using System;

namespace Acos.Felles.ASiCE.Model
{
    public class MimeType
    {
        private string Type { get; }

        private MimeType(string type)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        public static MimeType ForString(string type)
        {
            return new MimeType(type ?? throw new ArgumentNullException(nameof(type)));
        }

        public override string ToString()
        {
            return Type;
        }
    }
}