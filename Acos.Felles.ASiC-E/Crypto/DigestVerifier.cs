using System;
using System.Collections.Generic;
using System.Linq;
using Acos.Felles.ASiCE.Model;
using Serilog;

namespace Acos.Felles.ASiCE.Crypto
{
    public class DigestVerifier : IDigestReceiver, IDigestVerifier
    {
        private readonly IDictionary<string, DeclaredDigestFile> _declaredDigests;
        private readonly Queue<string> _validFiles = new Queue<string>();
        private readonly Queue<string> _invalidFiles = new Queue<string>();

        private DigestVerifier(IDictionary<string, DeclaredDigestFile> declaredDigests)
        {
            _declaredDigests = declaredDigests ?? throw new ArgumentNullException(nameof(declaredDigests));
        }

        public static DigestVerifier Create(IDictionary<string, DeclaredDigestFile> declaredDigests)
        {
            var dd = declaredDigests ?? throw new ArgumentNullException(nameof(declaredDigests));
            return new DigestVerifier(dd);
        }

        public void ReceiveDigest(string fileName, byte[] digest)
        {
            Log.Debug("Got digest for file {fileName}", fileName);
            var declaredDigest = _declaredDigests[fileName];
            if (declaredDigest != null)
            {
                if (declaredDigest.Digest.SequenceEqual(digest))
                {
                    Log.Debug("Digest verified for file {fileName}", fileName);
                    _validFiles.Enqueue(fileName);
                }
                else
                {
                    Log.Debug("Digest did not match the declared digest for file {fileName}", fileName);
                    _invalidFiles.Enqueue(fileName);
                }
            }
            else
            {
                Log.Debug("No file named {fileName} has been declared. It will be deemed invalid", fileName);
                _invalidFiles.Enqueue(fileName);
            }
        }

        public DigestVerificationResult Verification()
        {
            var totalFilesProcessedCount = _invalidFiles.Count + _validFiles.Count;
            if (totalFilesProcessedCount != _declaredDigests.Count)
            {
                throw new DigestVerificationException($"Total number of files processed by the verifier was {totalFilesProcessedCount}, but {_declaredDigests.Count} files was declared");
            }

            return new DigestVerificationResult(_validFiles, _invalidFiles);
        }
    }
}