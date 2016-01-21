namespace EA.Iws.Requests.WasteType
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;
    using Security;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class SetOtherWasteAdditionalInformation : IRequest<Guid>
    {
        public SetOtherWasteAdditionalInformation(Guid notificationId, string description, bool hasAnnex)
        {
            NotificationId = notificationId;
            Description = description;
            HasAnnex = hasAnnex;
        }

        public Guid NotificationId { get; private set; }

        public string Description { get; private set; }

        public bool HasAnnex { get; private set; }
    }
}