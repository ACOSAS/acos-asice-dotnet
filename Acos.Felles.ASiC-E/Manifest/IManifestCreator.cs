using System.Collections.Generic;
using Acos.Felles.ASiCE.Model;

namespace Acos.Felles.ASiCE.Manifest
{
    public interface IManifestCreator
    {
        ManifestContainer CreateManifest(IEnumerable<AsicePackageEntry> entries);
    }
}