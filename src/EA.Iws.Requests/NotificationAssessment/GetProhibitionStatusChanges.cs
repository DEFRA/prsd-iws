namespace EA.Iws.Requests.NotificationAssessment
{
    using EA.Iws.Core.Authorization;
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Iws.Core.NotificationAssessment;
    using EA.Iws.Domain.NotificationAssessment;
    using EA.Prsd.Core.Mediator;
    using System;
    using System.Collections.Generic;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotificationInternal)]
    public class GetProhibitionStatusChanges : IRequest<List<NotificationStatusChangeData>>
    {
        public Guid NotificationId { get; private set; }

        public GetProhibitionStatusChanges(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
