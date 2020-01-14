using System;
using Acos.Felles.ASiCE.Model;
using Xunit;

namespace Acos.Felles.ASiCE.Test.Model
{
    public class UriAndAlgorithmCollection : TheoryData<Uri, MessageDigestAlgorithm>
    {
        public UriAndAlgorithmCollection()
        {
            Add(MessageDigestAlgorithm.UriSHA256XmlEnc, MessageDigestAlgorithm.SHA256);
            Add(MessageDigestAlgorithm.UriSHA384XmlEnc, MessageDigestAlgorithm.SHA384);
            Add(MessageDigestAlgorithm.UriSHA512XmlEnc, MessageDigestAlgorithm.SHA512);
            Add(MessageDigestAlgorithm.UriSHA256XmlDsig, MessageDigestAlgorithm.SHA256);
            Add(MessageDigestAlgorithm.UriSHA384XmlDsig, MessageDigestAlgorithm.SHA384);
            Add(MessageDigestAlgorithm.UriSHA512XmlDsig, MessageDigestAlgorithm.SHA512);
            Add(new Uri("http://localhost"), null);
            Add(null, null);
        }
    }
}