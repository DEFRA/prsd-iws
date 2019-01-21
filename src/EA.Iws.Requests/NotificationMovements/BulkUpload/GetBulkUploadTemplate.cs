namespace EA.Iws.Requests.NotificationMovements.BulkUpload
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Documents;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovementsExternal)]
    public class GetBulkUploadTemplate : IRequest<byte[]>
    {
        public BulkType BulkType;

        public GetBulkUploadTemplate(BulkType bulkType)
        {
            BulkType = bulkType;
        }
    }
}
