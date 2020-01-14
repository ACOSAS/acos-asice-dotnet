using System.Collections.Generic;
using System.Linq;

namespace Acos.Felles.ASiCE.Model
{
    public class DigestVerificationResult
    {
        private IEnumerable<string> _validElements;
        private IEnumerable<string> _invalidElements;

        public DigestVerificationResult(IEnumerable<string> validElements, IEnumerable<string> invalidElements)
        {
            _validElements = validElements;
            _invalidElements = invalidElements;
        }

        public bool AllValid => !_invalidElements.Any();

        public IEnumerable<string> ValidElements => _validElements.ToList();

        public IEnumerable<string> InvalidElements => _invalidElements.ToList();
    }
}