namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.NotificationAssessment;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotificationAssessment)]
    public class GetKeyDatesSummaryInformation : IRequest<KeyDatesSummaryData>
    {
        public Guid NotificationId { get; private set; }

        public GetKeyDatesSummaryInformation(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
