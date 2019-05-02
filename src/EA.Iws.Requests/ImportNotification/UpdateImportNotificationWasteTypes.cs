namespace EA.Iws.Requests.ImportNotification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportNotification.Update;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanChangeWasteTypes)]
    public class UpdateImportNotificationWasteTypes : IRequest<Guid>
    {
        public Guid ImportNotificationId { get; private set; }

        public WasteTypes WasteTypes { get; set; }

        public UpdateImportNotificationWasteTypes(Guid importNotificationId, WasteTypes wasteTypes)
        {
            ImportNotificationId = importNotificationId;
            WasteTypes = wasteTypes;
        }
    }
}
