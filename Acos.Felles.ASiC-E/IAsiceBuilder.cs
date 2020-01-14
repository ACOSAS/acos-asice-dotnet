using System;
using System.IO;
using Acos.Felles.ASiCE.Model;

namespace Acos.Felles.ASiCE
{
    public interface IAsiceBuilder<T> : IBuilder<T>, IDisposable
    {
        /// <summary>
        /// Add a file contained in a FileStream to the ASiC-E package. MIME type will be derived from the filename extension.
        /// </summary>
        /// <param name="file">The filestream to add</param>
        /// <returns>The same builder with the given file added</returns>
        IAsiceBuilder<T> AddFile(FileStream file);

        /// <summary>
        /// Add a stream of data with a given filename to the ASiC-E package. MIME type will be derived from the filename extension.
        /// </summary>
        /// <param name="stream">The stream to read the data from</param>
        /// <param name="filename">The filename of the file to be added</param>
        /// <returns>The same builder with the given file added</returns>
        IAsiceBuilder<T> AddFile(Stream stream, string filename);

        /// <summary>
        /// Add a stream of data with a given filename and MIME type to the ASiC-E package.
        /// </summary>
        /// <param name="stream">The stream to read the data from</param>
        /// <param name="filename">The filename of the file to be added</param>
        /// <param name="mimeType">The MIME type of the given file</param>
        /// <returns>The same builder with the given file added</returns>
        IAsiceBuilder<T> AddFile(Stream stream, string filename, MimeType mimeType);
    }
}