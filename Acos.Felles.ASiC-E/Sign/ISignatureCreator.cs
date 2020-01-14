using System.Collections.Generic;
using Acos.Felles.ASiCE.Model;

namespace Acos.Felles.ASiCE.Sign
{
    public interface ISignatureCreator
    {
        SignatureFileContainer CreateSignatureFile(IEnumerable<AsicePackageEntry> asicPackageEntries);

        SignatureFileContainer CreateCadesSignatureFile(ManifestContainer manifestContainer);
    }
}