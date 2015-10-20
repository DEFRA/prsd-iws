namespace EA.Iws.Requests.Movement
{
    using System;
    using Prsd.Core.Mediator;

    public class SaveCertificateOfReceiptFile : IRequest<Guid>
    {
        public SaveCertificateOfReceiptFile(Guid movementId, byte[] certificateBytes, string fileType)
        {
            MovementId = movementId;
            CertificateBytes = certificateBytes;
            FileType = fileType;
        }

        public Guid MovementId { get; private set; }

        public byte[] CertificateBytes { get; private set; }

        public string FileType { get; private set; }
    }
}