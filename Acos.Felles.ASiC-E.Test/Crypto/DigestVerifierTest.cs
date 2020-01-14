using System;
using System.Collections.Immutable;
using System.Linq;
using Acos.Felles.ASiCE.Crypto;
using Acos.Felles.ASiCE.Model;
using FluentAssertions;
using Xunit;

namespace Acos.Felles.ASiCE.Test.Crypto
{
    public class DigestVerifierTest
    {
        private static readonly MimeType MimeType = MimeType.ForString("text/plain");

        private static readonly DeclaredDigestFile FileOne = new DeclaredDigestFile(
            new byte[] { 0, 1, 1 },
            MessageDigestAlgorithm.SHA256,
            "fileOne.txt",
            MimeType);

        private static readonly DeclaredDigestFile FileTwo = new DeclaredDigestFile(
            new byte[] { 1, 1, 1 },
            MessageDigestAlgorithm.SHA256,
            "fileTwo.txt",
            MimeType);

        [Fact(DisplayName = "Verify no digests")]
        public void VerifyNothing()
        {
            var digestVerifier = DigestVerifier.Create(ImmutableDictionary<string, DeclaredDigestFile>.Empty);
            var verification = digestVerifier.Verification();
            AssertionExtensions.Should(verification).NotBeNull();
            AssertionExtensions.Should(verification.AllValid).BeTrue();
        }

        [Fact(DisplayName = "Verify digest, multiple declared none received")]
        public void VerifyNoneChecked()
        {
            var digestVerifier = CreateDigestVerifierForTest();

            Func<DigestVerificationResult> action = () => digestVerifier.Verification();

            AssertionExtensions.Should(action.Should().Throw<DigestVerificationException>().And.Message)
                .Be("Total number of files processed by the verifier was 0, but 2 files was declared");
        }

        [Fact(DisplayName = "Verify digest, two declared but only one checked")]
        public void VerifyOneCheckedTwoDeclared()
        {
            var digestVerifier = CreateDigestVerifierForTest();
            digestVerifier.ReceiveDigest(FileOne.Name, Enumerable.ToArray<byte>(FileOne.Digest));
            Func<DigestVerificationResult> action = () => digestVerifier.Verification();
            AssertionExtensions.Should(action.Should().Throw<DigestVerificationException>().And.Message)
                .Be("Total number of files processed by the verifier was 1, but 2 files was declared");
        }

        [Fact(DisplayName = "Verify digests, two declared and two checked but with non-matching digests")]
        public void VerifyAllButWrongDigest()
        {
            var digestVerifier = CreateDigestVerifierForTest();
            digestVerifier.ReceiveDigest(FileOne.Name, Enumerable.ToArray<byte>(FileTwo.Digest));
            digestVerifier.ReceiveDigest(FileTwo.Name, Enumerable.ToArray<byte>(FileOne.Digest));
            var result = digestVerifier.Verification();
            AssertionExtensions.Should(result.AllValid).BeFalse();
            Enumerable.Count<string>(result.InvalidElements).Should().Be(2);
        }

        [Fact(DisplayName = "Verify digests, all checked and valid")]
        public void VerifyAllSucceed()
        {
            var digestVerifier = CreateDigestVerifierForTest();
            digestVerifier.ReceiveDigest(FileOne.Name, Enumerable.ToArray<byte>(FileOne.Digest));
            digestVerifier.ReceiveDigest(FileTwo.Name, Enumerable.ToArray<byte>(FileTwo.Digest));
            var result = digestVerifier.Verification();
            AssertionExtensions.Should(result.AllValid).BeTrue();
            Enumerable.Count<string>(result.ValidElements).Should().Be(2);
            result.ValidElements.Should().Contain(FileOne.Name, FileTwo.Name);
        }

        private static DigestVerifier CreateDigestVerifierForTest()
        {
            var declaredDigests = ImmutableList.Create(FileOne, FileTwo).ToImmutableDictionary(d => d.Name, d => d);
            return DigestVerifier.Create(declaredDigests);
        }
    }
}