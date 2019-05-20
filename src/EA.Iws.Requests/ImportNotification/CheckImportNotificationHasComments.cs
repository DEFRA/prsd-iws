namespace EA.Iws.Requests.ImportNotification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanEditComments)]
    public class CheckImportNotificationHasComments : IRequest<bool>
    {
        public Guid ImportNotificationId { get; private set; }

        public CheckImportNotificationHasComments(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }
    }
}
