using System;
using Acos.Felles.ASiCE.Model;
using FluentAssertions;
using Xunit;

namespace Acos.Felles.ASiCE.Test.Model
{
    public class MessageDigestAlgorithmFromUriTest
    {
        [Theory]
        [ClassData(typeof(UriAndAlgorithmCollection))]
        public void TestFromUri(Uri uri, MessageDigestAlgorithm expectedAlgorithm)
        {
            AssertionExtensions.Should(MessageDigestAlgorithm.FromUri(uri)).Be(expectedAlgorithm);
        }
    }
}