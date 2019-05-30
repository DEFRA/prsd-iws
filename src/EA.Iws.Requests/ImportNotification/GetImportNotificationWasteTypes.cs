namespace EA.Iws.Requests.ImportNotification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportNotification.Update;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanEditImportNotification)]
    public class GetImportNotificationWasteTypes : IRequest<WasteTypes>
    {
        public Guid ImportNotificationId { get; private set; }

        public GetImportNotificationWasteTypes(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }
    }
}
