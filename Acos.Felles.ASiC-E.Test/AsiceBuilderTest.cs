using System;
using System.IO;
using System.IO.Compression;
using Acos.Felles.ASiCE.Crypto;
using Acos.Felles.ASiCE.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace Acos.Felles.ASiCE.Test
{
    public class AsiceBuilderTest
    {

        //private readonly LogFixture logFixture;

        public AsiceBuilderTest()
        {
        }

        [Fact(DisplayName = "Try to create builder using non-writable stream")]
        public void TestNotWritableStream()
        {
            var zipStream = new Mock<Stream>();
            var certificate = new Mock<ICertificateHolder>();
            Action createFunction = () =>
                AsiceBuilder.Create(zipStream.Object, MessageDigestAlgorithm.SHA512, certificate.Object);
            createFunction.Should().Throw<ArgumentException>();
            zipStream.VerifyGet(s => s.CanWrite);
            zipStream.VerifyNoOtherCalls();
        }

        [Fact]
        public void TestAddFileStream()
        {
            byte[] zippedBytes;

            var signingCertificates = TestdataLoader.ReadCertificatesForTest();
            using (var zipStream = new MemoryStream())
            using (var fileStream = File.OpenRead("small.pdf"))
            {
                using (var asiceBuilder =
                    AsiceBuilder.Create(zipStream, MessageDigestAlgorithm.SHA256, signingCertificates))
                {
                    asiceBuilder.Should().NotBeNull();

                    asiceBuilder.AddFile(fileStream).Should().NotBeNull().And.BeOfType<AsiceBuilder>();

                    var asiceArchive = asiceBuilder.Build();
                    asiceArchive.Should().NotBeNull();
                }

                zippedBytes = zipStream.ToArray();
            }

            //logFixture.GetLog<AsiceBuilderTest>().Info($"Created zip containing {zippedBytes.Length} bytes");
            zippedBytes.Should().HaveCountGreaterThan(0);

            using (var zipStream = new MemoryStream(zippedBytes))
            using (var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read))
            {
                zipArchive.Entries.Count.Should().Be(4);
            }
        }
    }
}