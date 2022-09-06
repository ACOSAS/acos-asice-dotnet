using Acos.Felles.ASiCE.Sign;
using FluentAssertions;
using Xunit;

namespace Acos.Felles.ASiCE.Test.Sign
{
    public class SignatureVerifierTest
    {
        [Fact(DisplayName = "Verify signature")]
        public void VerifySignature()
        {
            var signature = TestdataLoader.ReadFromResource("signature.p7s");
            var signedData = TestdataLoader.ReadFromResource("signedData.xml");
            var signatureVerifier = new SignatureVerifier();
            var certificate = signatureVerifier.Validate(signedData, signature);
            AssertionExtensions.Should((object) certificate).NotBeNull();
        }
    }
}