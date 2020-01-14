using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Acos.Felles.ASiCE.Model;
using Acos.Felles.ASiCE.Xsd;
using FluentAssertions;
using Xunit;

namespace Acos.Felles.ASiCE.Test.Model
{
    public class CadesManifestTest
    {
        [Fact(DisplayName = "Instantiate using null value")]
        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", Justification = "The whole point is to test that instance creation fails", Scope = "method")]
        public void ProvideNull()
        {
            Action creator = () => new CadesManifest(null);
            creator.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("asiCManifestType");
        }

        [Fact(DisplayName = "Instantiate using a quite empty manifest")]
        public void ProvideWithNoSignatureRef()
        {
            var manifestType = new ASiCManifestType();
            var cadesManifest = new CadesManifest(manifestType);
            AssertionExtensions.Should(cadesManifest).NotBeNull();
            AssertionExtensions.Should(cadesManifest.Spec).Be(ManifestSpec.Cades);
            cadesManifest.Digests.Should().BeNull();
            AssertionExtensions.Should(cadesManifest.SignatureFileName).BeNull();
            AssertionExtensions.Should(cadesManifest.SignatureFileRef).BeNull();
        }

        [Fact(DisplayName = "Instantiate with data objects")]
        public void ProvideWithReferences()
        {
            const string FileName = "filename.txt";
            var digestAlgorithm = MessageDigestAlgorithm.SHA256;
            var manifestType = new ASiCManifestType
            {
                DataObjectReference = new[]
                {
                    new DataObjectReferenceType
                    {
                        Rootfile = false,
                        MimeType = "text/plain",
                        URI = FileName,
                        DigestMethod = new DigestMethodType
                        {
                            Algorithm = digestAlgorithm.Uri.ToString()
                        },
                        DigestValue = new byte[] { 0, 1, 0, 1 }
                    }
                }
            };
            var cadesManifest = new CadesManifest(manifestType);
            AssertionExtensions.Should(cadesManifest).NotBeNull();
            var digests = cadesManifest.Digests;
            digests.Should().NotBeNull();
            AssertionExtensions.Should(digests.Count).Be(1);
            AssertionExtensions.Should(Enumerable.First(digests).Value.MessageDigestAlgorithm).BeEquivalentTo(digestAlgorithm);
            AssertionExtensions.Should(cadesManifest.SignatureFileRef).BeNull();
        }

        [Fact(DisplayName = "Instantiate with signature ref")]
        public void ProvideWithSignatureFile()
        {
            const string SignatureFileName = "my.p7";
            var manifestType = new ASiCManifestType
            {
                SigReference = new SigReferenceType
                {
                    MimeType = AsiceConstants.ContentTypeSignature,
                    URI = SignatureFileName
                }
            };
            var cadesManifest = new CadesManifest(manifestType);
            AssertionExtensions.Should(cadesManifest).NotBeNull();
            AssertionExtensions.Should(cadesManifest.SignatureFileName).NotBeNull();
            AssertionExtensions.Should(cadesManifest.SignatureFileName).Be(SignatureFileName);
            AssertionExtensions.Should(cadesManifest.SignatureFileRef).NotBeNull();
            AssertionExtensions.Should(cadesManifest.SignatureFileRef.FileName).Be(SignatureFileName);
            AssertionExtensions.Should(cadesManifest.SignatureFileRef.MimeType).Be(AsiceConstants.MimeTypeCadesSignature);
        }
    }
}