using System;

namespace Acos.Felles.ASiCE.Crypto
{
    public class DigestVerificationException : Exception
    {
        public DigestVerificationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public DigestVerificationException(string message) : this(message, null)
        {
        }

        private DigestVerificationException()
            : this(null)
        {
        }
    }
}