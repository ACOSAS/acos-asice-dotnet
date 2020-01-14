using System.Collections.Generic;

namespace Acos.Felles.ASiCE.Model
{
    public abstract class AbstractManifest
    {
        private readonly ManifestSpec _manifestSpec;

        public ManifestSpec Spec => _manifestSpec;

        public abstract IEnumerable<SignatureFileRef> GetSignatureRefs();

        public abstract IDictionary<string, DeclaredDigestFile> GetDeclaredDigests();

        protected AbstractManifest(ManifestSpec manifestSpec)
        {
            _manifestSpec = manifestSpec;
        }
    }
}