using System;

namespace Acos.Felles.ASiCE.Model
{
    public class AsicePackageEntry
    {
        public string FileName { get; }

        public MimeType Type { get; }

        public MessageDigestAlgorithm MessageDigestAlgorithm { get; }

        public DigestContainer Digest { get; set; }

        public AsicePackageEntry(string fileName, MimeType type, MessageDigestAlgorithm messageDigestAlgorithm)
        {
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            Type = type ?? throw new ArgumentNullException(nameof(type));
            MessageDigestAlgorithm = messageDigestAlgorithm ??
                                     throw new ArgumentNullException(nameof(messageDigestAlgorithm));
        }
    }
}