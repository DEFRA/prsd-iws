namespace EA.Iws.Requests.ImportNotification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportNotification.Update;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanEditImportNotification)]
    public class GetWasteOperationData : IRequest<WasteOperationData>
    {
        public Guid ImportNotificationId { get; private set; }

        public GetWasteOperationData(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }
    }
}