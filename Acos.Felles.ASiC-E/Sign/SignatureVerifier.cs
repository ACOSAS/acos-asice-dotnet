using System;
using System.IO;
using System.Linq;
using Acos.Felles.ASiCE.Xsd;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.X509;
using Serilog;

namespace Acos.Felles.ASiCE.Sign
{
    public class SignatureVerifier : ISignatureVerifier
    {

        public certificate Validate(byte[] data, byte[] signature)
        {
            using (var dataStream = new MemoryStream(data))
            using (var signatureStream = new MemoryStream(signature))
            {
                var cmsTypedStream = new CmsTypedStream(dataStream);
                var cmsSignedDataParser = new CmsSignedDataParser(cmsTypedStream, signatureStream);

                var store = cmsSignedDataParser.GetCertificates("Collection");
                var signerInfos = cmsSignedDataParser.GetSignerInfos();
                var signerInfo = signerInfos.GetSigners().OfType<SignerInformation>().First();
                var certificate = store?.GetMatches(signerInfo?.SignerID)
                    .OfType<X509Certificate>().FirstOrDefault();

                if (certificate == null)
                {
                    return null;
                }

                Log.Debug(
                    "Certificate found in signature {dn}",
                    certificate.SubjectDN.ToString());
                var now = DateTime.Now;
                if (now < certificate.NotBefore || now > certificate.NotAfter)
                {
                    Log.Information(
                        "The certificate is not valid right now as it is only valid between {startTime}-{endTime}",
                        certificate.NotBefore,
                        certificate.NotAfter);
                }

                return new certificate
                {
                    subject = certificate.SubjectDN.ToString(),
                    certificate1 = certificate.GetEncoded()
                };
            }
        }
    }
}