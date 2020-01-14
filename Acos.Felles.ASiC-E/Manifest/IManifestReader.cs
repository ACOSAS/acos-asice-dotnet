using System.IO;
using Acos.Felles.ASiCE.Model;

namespace Acos.Felles.ASiCE.Manifest
{
    public interface IManifestReader<out M>
        where M : AbstractManifest
    {
        M FromStream(Stream stream);
    }
}