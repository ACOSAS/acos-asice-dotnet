using System.IO;
using Acos.Felles.ASiCE.Model;

namespace Acos.Felles.ASiCE
{
    public interface IAsicReader
    {
        AsiceReadModel Read(Stream inputStream);
    }
}