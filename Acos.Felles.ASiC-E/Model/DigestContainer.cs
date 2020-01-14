using System;

namespace Acos.Felles.ASiCE.Model
{
    public class DigestContainer
    {
        private readonly byte[] _digest;
        private readonly MessageDigestAlgorithm _messageDigestAlgorithm;

        public DigestContainer(byte[] digest, MessageDigestAlgorithm messageDigestAlgorithm)
        {
            _digest = digest ?? throw new ArgumentNullException(nameof(digest));
            _messageDigestAlgorithm = messageDigestAlgorithm ??
                                           throw new ArgumentNullException(nameof(messageDigestAlgorithm));
        }

        public byte[] GetDigest()
        {
            return _digest;
        }

        public MessageDigestAlgorithm GetMessageDigestAlgorithm()
        {
            return _messageDigestAlgorithm;
        }
    }
}