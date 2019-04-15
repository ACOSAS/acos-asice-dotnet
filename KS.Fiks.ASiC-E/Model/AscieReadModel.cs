using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using KS.Fiks.ASiC_E.Crypto;
using KS.Fiks.ASiC_E.Manifest;

namespace KS.Fiks.ASiC_E.Model
{
    public class AscieReadModel : IDisposable
    {
        private readonly ZipArchive _zipArchive;
        private readonly DigestVerifier _digestVerifier;

        private AscieReadModel(ZipArchive zipArchive)
        {
            this._zipArchive = zipArchive;

            CadesManifest = GetCadesManifest();

            Entries = GetAsiceEntries();

            Signatures = ExtractSignaturesFromManifest();

            var declaredDigests = Manifest?.getDeclaredDigests();
            if (declaredDigests != null)
            {
                this._digestVerifier = Crypto.DigestVerifier.Create(declaredDigests);
            }

        }

        public IEnumerable<AsiceReadEntry> Entries { get; }

        public CadesManifest CadesManifest { get; }

        private AbstractManifest Manifest => CadesManifest;

        public Signatures Signatures { get; }

        public IDigestVerifier DigestVerifier => this._digestVerifier;

        public static AscieReadModel Create(ZipArchive zipArchive)
        {
            var asicArchive = zipArchive ?? throw new ArgumentNullException(nameof(zipArchive));
            if (zipArchive.Mode != ZipArchiveMode.Read)
            {
                throw new ArgumentException("The provided ZipArchive should be in READ mode", nameof(zipArchive));
            }

            var firstEntry = asicArchive.Entries.FirstOrDefault();
            if (firstEntry == null || firstEntry.FullName != AsiceConstants.FileNameMimeType)
            {
                throw new ArgumentException(
                    $"Archive is not a valid ASiC-E archive as the first entry is not '{AsiceConstants.FileNameMimeType}'",
                    nameof(zipArchive));
            }

            return new AscieReadModel(asicArchive);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool dispose)
        {
            // TODO: verify signature
            this._zipArchive.Dispose();
        }

        private IEnumerable<AsiceReadEntry> GetAsiceEntries()
        {
            return this._zipArchive.Entries.Where(entry => entry.Name != AsiceConstants.FileNameMimeType)
                .Where(entry => !entry.FullName.StartsWith("META-INF/", StringComparison.OrdinalIgnoreCase))
                .Select(entry => new AsiceReadEntry(entry, LookupMessageDigestAlgorithmForEntry(entry.FullName),
                    this._digestVerifier));
        }

        private MessageDigestAlgorithm LookupMessageDigestAlgorithmForEntry(string fullEntryName)
        {
            var declaredDigest = CadesManifest?.Digests[fullEntryName];
            if (declaredDigest == null)
            {
                throw new DigestVerificationException(
                    $"Could not find declared digest method for entry '{fullEntryName}'");
            }

            return declaredDigest.MessageDigestAlgorithm;
        }

        private CadesManifest GetCadesManifest()
        {
            var cadesManifestEntry = GetEntry(AsiceConstants.CadesManifestFilename);
            if (cadesManifestEntry == null)
            {
                return null;
            }

            using (var entryStream = cadesManifestEntry.Open())
            {
                return new CadesManifestReader().FromStream(entryStream);
            }
        }

        private ZipArchiveEntry GetEntry(string fullEntryName)
        {
            return _zipArchive.Entries.SingleOrDefault(entry =>
                entry.FullName.Equals(fullEntryName, StringComparison.CurrentCultureIgnoreCase));
        }

        private byte[] GetDataForEntry(string fullEntryName)
        {
            var zipArchiveEntry = GetEntry(fullEntryName);
            if (zipArchiveEntry == null)
            {
                return null;
            }

            using (var zipStream = zipArchiveEntry.Open())
            using (var bufferStream = new MemoryStream())
            {
                zipStream.CopyTo(bufferStream);
                return bufferStream.ToArray();
            }
        }

        private Signatures ExtractSignaturesFromManifest()
        {
            var signatures = Manifest?.getSignatureRefs()
                .Select(s => new SignatureFileContainer(s, GetDataForEntry(s.FileName)));
            return signatures == null ? null : new Signatures(signatures);
        }
    }
}