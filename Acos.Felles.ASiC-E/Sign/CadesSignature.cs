using System;
using Acos.Felles.ASiCE.Model;

namespace Acos.Felles.ASiCE.Sign
{
    public static class CadesSignature
    {
        public static SignatureFileRef CreateSignatureRef()
        {
            var uuid = Guid.NewGuid().ToString();
            return new SignatureFileRef($"META-INF/signature-{uuid}.p7s");
        }
    }
}