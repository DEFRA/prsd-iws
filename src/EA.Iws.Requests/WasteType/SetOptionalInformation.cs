namespace EA.Iws.Requests.WasteType
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;
    using Security;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class SetOptionalInformation : IRequest<Guid>
    {
        public SetOptionalInformation(string optionalInformation, bool hasAnnex, Guid notificationId)
        {
            NotificationId = notificationId;
            OptionalInformation = optionalInformation;
            HasAnnex = hasAnnex;
        }

        public string OptionalInformation { get; private set; }

        public bool HasAnnex { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}