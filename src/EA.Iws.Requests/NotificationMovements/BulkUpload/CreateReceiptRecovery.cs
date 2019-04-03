namespace EA.Iws.Requests.NotificationMovements.BulkUpload
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanEditExportMovements)]
    public class CreateReceiptRecovery : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public Guid DraftBulkUploadId { get; private set; }

        public byte[] SupportingDocument { get; private set; }

        public string FileExtension { get; private set; }

        public CreateReceiptRecovery(Guid notificationId,
            Guid draftBulkUploadId,
            byte[] supportingDocument,
            string fileExtension)
        {
            NotificationId = notificationId;
            DraftBulkUploadId = draftBulkUploadId;
            SupportingDocument = supportingDocument;
            FileExtension = fileExtension;
        }
    }
}
