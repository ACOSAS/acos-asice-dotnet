using System.IO;
using System.IO.Compression;
using Acos.Felles.ASiCE.Model;

namespace Acos.Felles.ASiCE
{
    public class AsiceReader : IAsicReader
    {
        public AsiceReadModel Read(Stream inputStream)
        {
            var zipArchive = new ZipArchive(inputStream, ZipArchiveMode.Read);
            return AsiceReadModel.Create(zipArchive);
        }
    }
}