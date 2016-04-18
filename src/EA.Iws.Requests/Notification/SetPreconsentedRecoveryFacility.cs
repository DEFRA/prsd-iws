namespace EA.Iws.Requests.Notification
{
    using System;
    using Authorization;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class SetPreconsentedRecoveryFacility : IRequest<string>
    {
        public SetPreconsentedRecoveryFacility(Guid notificationId, bool isPreconsentedRecoveryFacility)
        {
            NotificationId = notificationId;
            IsPreconsentedRecoveryFacility = isPreconsentedRecoveryFacility;
        }

        public Guid NotificationId { get; private set; }

        public bool IsPreconsentedRecoveryFacility { get; private set; }
    }
}