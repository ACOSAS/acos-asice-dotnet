using System;
using System.IO;
using Acos.Felles.ASiCE.Model;
using Org.BouncyCastle.Crypto.IO;
using Org.BouncyCastle.Security;

namespace Acos.Felles.ASiCE.Crypto
{
    public class DigestReadStream : DigestStream
    {

        private readonly string _fileName;
        private readonly IDigestReceiver _digestReceiver;

        public DigestReadStream(Stream stream, string fileName, MessageDigestAlgorithm messageDigestAlgorithm, IDigestReceiver digestReceiver)
            : base(
                stream,
                messageDigestAlgorithm.Digest,
                null)
        {
            _fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            _digestReceiver = digestReceiver ?? throw new ArgumentNullException(nameof(digestReceiver));
        }

        public override void Close()
        {
            _digestReceiver.ReceiveDigest(_fileName, DigestUtilities.DoFinal(ReadDigest()));
            base.Close();
        }
    }
}