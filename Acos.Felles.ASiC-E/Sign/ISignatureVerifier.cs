using Acos.Felles.ASiCE.Xsd;

namespace Acos.Felles.ASiCE.Sign
{
    public interface ISignatureVerifier
    {
        certificate Validate(byte[] data, byte[] signature);
    }
}