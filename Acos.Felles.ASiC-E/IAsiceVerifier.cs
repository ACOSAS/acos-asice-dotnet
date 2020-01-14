using System.IO;
using Acos.Felles.ASiCE.Xsd;

namespace Acos.Felles.ASiCE
{
    public interface IAsiceVerifier
    {
        /// <summary>
        /// Verify a given ASiC archive
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        asicManifest Verify(Stream inputData);
    }
}