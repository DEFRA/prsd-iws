namespace EA.Iws.Requests.Movement.Receive
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanEditExportMovements)]
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