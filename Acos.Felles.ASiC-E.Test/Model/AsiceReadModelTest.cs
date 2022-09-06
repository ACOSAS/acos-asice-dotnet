using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Acos.Felles.ASiCE.Model;
using FluentAssertions;
using Xunit;

namespace Acos.Felles.ASiCE.Test.Model
{
    public class AsiceReadModelTest
    {
        [Fact(DisplayName = "Read empty Zip")]
        public void ReadEmptyAsicE()
        {
            using (var zipOutStream = new MemoryStream())
            {
                var createdArchive = new ZipArchive(zipOutStream, ZipArchiveMode.Create);
                createdArchive.Dispose();
                using (var zipInStream = new MemoryStream(zipOutStream.ToArray()))
                {
                    using (var readArchive = new ZipArchive(zipInStream, ZipArchiveMode.Read))
                    {
                        Action createAction = () => AsiceReadModel.Create(readArchive);
                        createAction.Should().Throw<ArgumentException>().And.ParamName.Should().Be("zipArchive");
                    }
                }
            }
        }

        [Fact(DisplayName = "Read non-empty Zip that is missing the required first entry")]
        public void ReadAsicEWithoutRequiredFirstEntry()
        {
            using (var zipOutStream = new MemoryStream())
            {
                using (var createdArchive = new ZipArchive(zipOutStream, ZipArchiveMode.Create))
                {
                    var newEntry = createdArchive.CreateEntry("file.txt");
                    using (var entryStream = newEntry.Open())
                    {
                        entryStream.Write(Encoding.UTF8.GetBytes("Lorem Ipsum"));
                    }
                }

                using (var zipInStream = new MemoryStream(zipOutStream.ToArray()))
                {
                    using (var readArchive = new ZipArchive(zipInStream, ZipArchiveMode.Read))
                    {
                        Action createAction = () => AsiceReadModel.Create(readArchive);
                        createAction.Should().Throw<ArgumentException>().And.ParamName.Should().Be("zipArchive");
                    }
                }
            }
        }

        [Fact(DisplayName = "Read non-empty Zip that is missing manifest")]
        public void ReadAsicEWithoutManifestAndSignature()
        {
            using (var zipOutStream = new MemoryStream())
            {
                using (var createdArchive = new ZipArchive(zipOutStream, ZipArchiveMode.Create))
                {
                    var newEntry = createdArchive.CreateEntry(AsiceConstants.FileNameMimeType);
                    using (var entryStream = newEntry.Open())
                    {
                        entryStream.Write(Encoding.UTF8.GetBytes((string)AsiceConstants.ContentTypeASiCe));
                    }
                }

                using (var zipInStream = new MemoryStream(zipOutStream.ToArray()))
                {
                    using (var readArchive = new ZipArchive(zipInStream, ZipArchiveMode.Read))
                    {
                        var asiceArchive = AsiceReadModel.Create(readArchive);
                        AssertionExtensions.Should((object)asiceArchive).NotBeNull();
                        Enumerable.Count(asiceArchive.Entries).Should().Be(0);
                        AssertionExtensions.Should((object)asiceArchive.CadesManifest).BeNull();
                    }
                }
            }
        }

        [Fact(DisplayName = "Read ASiC-E package from resource")]
        public void ReadAsiceResource()
        {
            using (var asicStream = TestDataUtil.ReadValidAsiceCadesFromResource())
            {
                using (var zip = new ZipArchive(asicStream, ZipArchiveMode.Read))
                using (var asice = AsiceReadModel.Create(zip))
                {
                    AssertionExtensions.Should((object)asice.CadesManifest).NotBeNull();
                    AssertionExtensions.Should((object)asice.Signatures).NotBeNull();
                    foreach (var asiceReadEntry in asice.Entries)
                    {
                        using (var entryStream = asiceReadEntry.OpenStream())
                        using (var bufferStream = new MemoryStream())
                        {
                            entryStream.CopyTo(bufferStream);
                            bufferStream.Position.Should().BeGreaterThan(0);
                        }
                    }

                    AssertionExtensions.Should((bool)asice.DigestVerifier.Verification().AllValid).BeTrue();
                }
            }
        }

        [Fact(DisplayName = "Read valid AsicE archive")]
        public void ReadAsiceWithCadesManifestAndSignature()
        {
            var signingCertificates = TestdataLoader.ReadCertificatesForTest();
            const string contentFile = "filename.txt";
            const string content = "Lorem ipsum";

            using (var outputStream = new MemoryStream())
            {
                using (var textFileStream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
                using (var asiceBuilder = AsiceBuilder.Create(outputStream, MessageDigestAlgorithm.SHA256, signingCertificates))
                {
                    asiceBuilder.AddFile(textFileStream, contentFile, MimeType.ForString("text/plain"));
                    asiceBuilder.Build().Should().NotBeNull();
                }

                using (var readStream = new MemoryStream(outputStream.ToArray()))
                using (var zip = new ZipArchive(readStream))
                {
                    var asicePackage = AsiceReadModel.Create(zip);
                    var entries = asicePackage.Entries;
                    Enumerable.Count(entries).Should().Be(1);
                    var cadesManifest = asicePackage.CadesManifest;
                    AssertionExtensions.Should((object)cadesManifest).NotBeNull();

                    AssertionExtensions.Should((int)cadesManifest.Digests.Count).Be(1);
                    AssertionExtensions.Should((object)asicePackage.DigestVerifier).NotBeNull();
                    AssertionExtensions.Should((string)cadesManifest.SignatureFileName).NotBeNull();
                    AssertionExtensions.Should((object)asicePackage.Signatures).NotBeNull();
                    Enumerable.Count(asicePackage.Signatures.Containers).Should().Be(1);

                    var firstEntry = Enumerable.First(entries);
                    using (var entryStream = firstEntry.OpenStream())
                    using (var memoryStream = new MemoryStream())
                    {
                        entryStream.CopyTo(memoryStream);
                        Encoding.UTF8.GetString(memoryStream.ToArray()).Should().Be(content);
                    }

                    var verificationResult = asicePackage.DigestVerifier.Verification();
                    AssertionExtensions.Should((object)verificationResult).NotBeNull();
                    AssertionExtensions.Should((bool)verificationResult.AllValid).BeTrue();
                }
            }
        }
    }
}