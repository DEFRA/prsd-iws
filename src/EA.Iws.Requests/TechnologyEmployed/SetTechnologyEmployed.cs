namespace EA.Iws.Requests.TechnologyEmployed
{
    using System;
    using Authorization;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class SetTechnologyEmployed : IRequest<Guid>
    {
        public Guid NotificationId { get; private set; }
        public bool AnnexProvided { get; private set; }
        public string Details { get; private set; }
        public string FurtherDetails { get; private set; }

        public SetTechnologyEmployed(Guid notificationId, bool annexProvided, string details, string furtherDetails)
        {
            NotificationId = notificationId;
            AnnexProvided = annexProvided;
            Details = details;
            FurtherDetails = furtherDetails;
        }
    }
}