namespace EA.Iws.Requests.Notification
{
    using System;
    using Authorization;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class SetSpecialHandling : IRequest<string>
    {
        public SetSpecialHandling(Guid notificationId, bool hasSpecialHandlingRequirements, string specialHandlingDetails)
        {
            NotificationId = notificationId;
            HasSpecialHandlingRequirements = hasSpecialHandlingRequirements;
            SpecialHandlingDetails = specialHandlingDetails;
        }

        public bool HasSpecialHandlingRequirements { get; set; }

        public Guid NotificationId { get; private set; }

        public string SpecialHandlingDetails { get; private set; }
    }
}