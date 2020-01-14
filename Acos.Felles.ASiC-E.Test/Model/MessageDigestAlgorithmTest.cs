using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Acos.Felles.ASiCE.Model;
using FluentAssertions;
using Xunit;

namespace Acos.Felles.ASiCE.Test.Model
{
    public class MessageDigestAlgorithmTest
    {
        [SuppressMessage("Microsoft.Usage", "CA2211", Justification = "Needs to be public to support XUnit")]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Needs to be public to support XUnit")]
        public static IEnumerable<object[]> Algorithms = new[]
        {
            new object[] { MessageDigestAlgorithm.SHA256 },
            new object[] { MessageDigestAlgorithm.SHA384 },
            new object[] { MessageDigestAlgorithm.SHA512 }
        };

        [Theory]
        [MemberData(nameof(Algorithms))]
        public void TestStaticProperties(MessageDigestAlgorithm messageDigestAlgorithm)
        {
            AssertionExtensions.Should(messageDigestAlgorithm.Name).NotBeNull();
            AssertionExtensions.Should(messageDigestAlgorithm.Uri).NotBeNull();
            AssertionExtensions.Should(messageDigestAlgorithm.Digest).NotBeNull();
        }

        [Fact(DisplayName = "Test that the static field UriSHA256 is initialized")]
        public void Uri256()
        {
            AssertionExtensions.Should(MessageDigestAlgorithm.UriSHA256XmlEnc).NotBeNull();
        }

        [Fact(DisplayName = "Test that the static field UriSHA384 is initialized")]
        public void Uri384()
        {
            AssertionExtensions.Should(MessageDigestAlgorithm.UriSHA384XmlEnc).NotBeNull();
        }

        [Fact(DisplayName = "Test that the static field UriSHA512 is initialized")]
        public void Uri512()
        {
            AssertionExtensions.Should(MessageDigestAlgorithm.UriSHA512XmlEnc).NotBeNull();
        }
    }
}