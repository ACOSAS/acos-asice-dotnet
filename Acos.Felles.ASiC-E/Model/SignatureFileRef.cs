namespace Acos.Felles.ASiCE.Model
{
    public class SignatureFileRef : FileRef
    {
        public SignatureFileRef(string fileName)
            : base(fileName, AsiceConstants.MimeTypeCadesSignature)
        {
        }
    }
}