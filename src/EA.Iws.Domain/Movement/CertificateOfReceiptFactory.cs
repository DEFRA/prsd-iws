namespace EA.Iws.Domain.Movement
{
    using System.Threading.Tasks;
    using FileStore;

    public class CertificateOfReceiptFactory
    {
        private readonly CertificateOfReceiptName certificateOfReceiptName;

        public CertificateOfReceiptFactory(
            CertificateOfReceiptName certificateOfReceiptName)
        {
            this.certificateOfReceiptName = certificateOfReceiptName;
        }

        public async Task<File> CreateForMovement(Movement movement, byte[] content, string fileType)
        {
            var fileName = await certificateOfReceiptName.GetValue(movement);
            return new File(fileName, fileType, content);
        }
    }
}