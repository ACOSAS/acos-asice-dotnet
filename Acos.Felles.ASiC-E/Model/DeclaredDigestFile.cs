using System;
using System.Collections.Generic;
using System.Linq;

namespace Acos.Felles.ASiCE.Model
{
    public class DeclaredDigestFile
    {
        private readonly byte[] _digest;

        public MessageDigestAlgorithm MessageDigestAlgorithm { get; }

        public string Name { get; }

        public MimeType MimeType { get; }

        public IEnumerable<byte> Digest => _digest;

        public DeclaredDigestFile(
            byte[] digest,
            MessageDigestAlgorithm messageDigestAlgorithm,
            string fileName,
            MimeType mimeType)
        {
            _digest = digest ?? throw new ArgumentNullException(nameof(digest));
            MessageDigestAlgorithm = messageDigestAlgorithm ??
                                     throw new ArgumentNullException(nameof(messageDigestAlgorithm));
            Name = fileName ?? throw new ArgumentNullException(nameof(fileName));
            MimeType = mimeType ?? throw new ArgumentNullException(nameof(mimeType));
        }

        public bool Verify(DeclaredDigestFile declaredDigestFile)
        {
            return MessageDigestAlgorithm == declaredDigestFile.MessageDigestAlgorithm &&
                   DigestEquals(declaredDigestFile.Digest);
        }

        private bool DigestEquals(IEnumerable<byte> digest)
        {
            return _digest.SequenceEqual(digest);
        }
    }
}