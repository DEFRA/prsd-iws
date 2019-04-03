namespace EA.Iws.Requests.NotificationMovements.BulkUpload
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Documents;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovementsExternal)]
    public class GetBulkUploadTemplate : IRequest<byte[]>
    {
        public Guid NotificationId { get; set; }

        public BulkType BulkType { get; set; }

        public GetBulkUploadTemplate(Guid notificationId, BulkType bulkType)
        {
            NotificationId = notificationId;
            BulkType = bulkType;
        }
    }
}
