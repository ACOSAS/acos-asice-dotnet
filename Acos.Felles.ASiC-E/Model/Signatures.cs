using System;
using System.Collections.Generic;

namespace Acos.Felles.ASiCE.Model
{
    public class Signatures
    {
        public IEnumerable<SignatureFileContainer> Containers { get; }

        public Signatures(IEnumerable<SignatureFileContainer> containers)
        {
            Containers = containers ?? throw new ArgumentNullException(nameof(containers));
        }
    }
}