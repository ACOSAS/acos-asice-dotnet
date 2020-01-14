using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Acos.Felles.ASiCE.Manifest;
using Acos.Felles.ASiCE.Model;
using Acos.Felles.ASiCE.Xsd;
using FluentAssertions;
using Xunit;

namespace Acos.Felles.ASiCE.Test.Manifest
{
    public class CadesManifestCreatorTest
    {
        [Fact(DisplayName = "Create CAdES manifest without signature")]
        public void CreateCadesManifest()
        {
            var cadesManifestCreator = CadesManifestCreator.CreateWithoutSignatureFile();
            var digestAlgorithm = MessageDigestAlgorithm.SHA256;
            var fileEntry = new AsicePackageEntry("my.pdf", MimeType.ForString("application/pdf"), digestAlgorithm);
            fileEntry.Digest = new DigestContainer(new byte[] { 0, 0, 1 }, digestAlgorithm);
            var entries = new[] { fileEntry };
            var manifest = cadesManifestCreator.CreateManifest(entries);
            AssertionExtensions.Should(manifest).NotBeNull()
                .And
                .BeOfType<ManifestContainer>();
            manifest.Data.Should().NotBeNull();
            AssertionExtensions.Should(manifest.FileName).Be(AsiceConstants.CadesManifestFilename);

            var xmlManifest = DeserializeManifest(Enumerable.ToArray<byte>(manifest.Data));
            xmlManifest.Should().NotBeNull();
            xmlManifest.SigReference.Should().BeNull();
            xmlManifest.DataObjectReference.Should().HaveCount(1);
            var dataObjectRef = xmlManifest.DataObjectReference[0];
            dataObjectRef.Should().NotBeNull();
            dataObjectRef.MimeType.Should().Be(fileEntry.Type.ToString());
            dataObjectRef.DigestValue.Should().Equal(fileEntry.Digest.GetDigest());
            dataObjectRef.URI.Should().Be(fileEntry.FileName);
        }

        [Fact(DisplayName = "Create CAdES manifest with signature")]
        public void CreateCadesManifestIncludingSignature()
        {
            var cadesManifestCreator = CadesManifestCreator.CreateWithSignatureFile();
            var fileEntry = new AsicePackageEntry("my.pdf", MimeType.ForString("application/pdf"), MessageDigestAlgorithm.SHA256);
            fileEntry.Digest = new DigestContainer(new byte[] { 0, 0, 1 }, MessageDigestAlgorithm.SHA256);
            var manifest = cadesManifestCreator.CreateManifest(new[] { fileEntry });
            AssertionExtensions.Should(manifest).NotBeNull()
                .And
                .BeOfType<ManifestContainer>();
            AssertionExtensions.Should(manifest.FileName).Be(AsiceConstants.CadesManifestFilename);
            var xmlManifest = DeserializeManifest(Enumerable.ToArray<byte>(manifest.Data));
            xmlManifest.Should().NotBeNull();
            xmlManifest.SigReference.Should().NotBeNull();
            xmlManifest.SigReference.MimeType.Should().Be(AsiceConstants.ContentTypeSignature);
            xmlManifest.DataObjectReference.Should().HaveCount(1);
        }

        private static ASiCManifestType DeserializeManifest(byte[] data)
        {
            using (var xmlStream = new MemoryStream(data))
            {
                var xmlSerializer = new XmlSerializer(typeof(ASiCManifestType));
                var xmlObj = xmlSerializer.Deserialize(xmlStream);
                return (ASiCManifestType)xmlObj;
            }
        }
    }
}