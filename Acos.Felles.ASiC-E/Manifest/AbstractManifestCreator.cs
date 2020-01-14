using System;
using System.Collections.Generic;
using Acos.Felles.ASiCE.Model;

namespace Acos.Felles.ASiCE.Manifest
{
    public abstract class AbstractManifestCreator : IManifestCreator
    {
        public abstract ManifestContainer CreateManifest(IEnumerable<AsicePackageEntry> entries);

        protected static SignatureFileRef CreateSignatureRef()
        {
            var uuid = Guid.NewGuid().ToString();
            return new SignatureFileRef($"META-INF/signature-{uuid}.p7s");
        }
    }
}