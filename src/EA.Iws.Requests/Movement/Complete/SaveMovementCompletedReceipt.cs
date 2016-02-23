namespace EA.Iws.Requests.Movement.Complete
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanEditExportMovementsExternal)]
    public class SaveMovementCompletedReceipt : IRequest<Guid>
    {
        public Guid MovementId { get; private set; }
        public DateTime CompletedDate { get; private set; }
        public byte[] CertificateBytes { get; private set; }
        public string FileType { get; private set; }

        public SaveMovementCompletedReceipt(Guid movementId, DateTime completedDate, byte[] certificateBytes, string fileType)
        {
            MovementId = movementId;
            CompletedDate = completedDate;
            CertificateBytes = certificateBytes;
            FileType = fileType;
        }
    }
}
