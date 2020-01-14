namespace Acos.Felles.ASiCE.Crypto
{
    public interface IDigestReceiver
    {
        void ReceiveDigest(string fileName, byte[] digest);
    }
}