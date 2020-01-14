using System;

namespace Acos.Felles.ASiCE.Sign
{
    public class SignatureVerificationException : Exception
    {
        public SignatureVerificationException()
        {
        }

        public SignatureVerificationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public SignatureVerificationException(string message)
            : base(message)
        {
        }
    }
}