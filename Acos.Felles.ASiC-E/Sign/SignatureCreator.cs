using System;
using System.Collections.Generic;
using System.Linq;
using Acos.Felles.ASiCE.Crypto;
using Acos.Felles.ASiCE.Model;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.X509.Store;

namespace Acos.Felles.ASiCE.Sign
{
    public class SignatureCreator : ISignatureCreator
    {
        private const string X509StoreType = "Certificate/Collection";
        private readonly ICertificateHolder certificateHolder;

        private SignatureCreator(ICertificateHolder certificateHolder)
        {
            this.certificateHolder = certificateHolder ?? throw new ArgumentNullException(nameof(certificateHolder));
        }

        public static SignatureCreator Create(ICertificateHolder certificateHolder)
        {
            return new SignatureCreator(certificateHolder);
        }

        public SignatureFileContainer CreateSignatureFile(IEnumerable<AsicePackageEntry> asicPackageEntries)
        {
            throw new NotImplementedException();
        }

        public SignatureFileContainer CreateCadesSignatureFile(ManifestContainer manifestContainer)
        {
            var signedDataGenerator = new CmsSignedDataGenerator();
            signedDataGenerator.AddSigner(
                certificateHolder.GetPrivateKey(),
                certificateHolder.GetPublicCertificate(),
                CmsSignedGenerator.DigestSha256);
            signedDataGenerator.AddSignerInfoGenerator(CreateSignerInfoGenerator());
            signedDataGenerator.AddCertificates(CreateX509Store());
            var signedData =
                signedDataGenerator.Generate(new CmsProcessableByteArray(manifestContainer.Data.ToArray()));
            return new SignatureFileContainer(manifestContainer.SignatureFileRef, signedData.GetEncoded());
        }

        private IX509Store CreateX509Store()
        {
            return X509StoreFactory.Create(
                X509StoreType,
                new X509CollectionStoreParameters(new[] { certificateHolder.GetPublicCertificate() }));
        }

        private SignerInfoGenerator CreateSignerInfoGenerator()
        {
            return new SignerInfoGeneratorBuilder().Build(
                CreateContentSigner(),
                certificateHolder.GetPublicCertificate());
        }

        private Asn1SignatureFactory CreateContentSigner()
        {
            return new Asn1SignatureFactory(AsiceConstants.SignatureAlgorithm, certificateHolder.GetPrivateKey());
        }
    }
}