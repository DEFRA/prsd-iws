namespace EA.Iws.Requests.NotificationMovements.BulkUpload
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanEditExportMovements)]
    public class CreateBulkPrenotification : IRequest<bool>
    {
        public Guid NotificationId { get; set; }

        public Guid DraftBulkUploadId { get; set; }

        public byte[] SupportingDocument { get; set; }

        public string FileExtension { get; set; }

        public CreateBulkPrenotification(Guid notificationId, 
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
