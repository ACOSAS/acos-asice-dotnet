using System;
using System.IO;
using System.IO.Compression;
using Acos.Felles.ASiCE.Crypto;

namespace Acos.Felles.ASiCE.Model
{
    public class AsiceReadEntry
    {
        private readonly ZipArchiveEntry _zipArchiveEntry;
        private readonly MessageDigestAlgorithm _digestAlgorithm;
        private readonly IDigestReceiver _digestReceiver;

        public AsiceReadEntry(ZipArchiveEntry zipArchiveEntry, MessageDigestAlgorithm digestAlgorithm, IDigestReceiver digestReceiver)
        {
            _zipArchiveEntry = zipArchiveEntry ?? throw new ArgumentNullException(nameof(zipArchiveEntry));
            _digestAlgorithm = digestAlgorithm ?? throw new ArgumentNullException(nameof(digestAlgorithm));
            _digestReceiver = digestReceiver;
        }

        public string FileName => _zipArchiveEntry.Name;

        public Stream OpenStream()
        {
            return new DigestReadStream(_zipArchiveEntry.Open(), FileName, _digestAlgorithm, _digestReceiver);
        }
    }
}