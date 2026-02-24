namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using EA.Iws.Core.Authorization;
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Iws.Core.ImportNotificationAssessment;
    using EA.Iws.Domain.ImportNotificationAssessment;
    using EA.Prsd.Core.Mediator;
    using System;
    using System.Collections.Generic;

    [RequestAuthorization(ImportNotificationPermissions.CanReadImportNotificationAssessment)]
    public class GetImportNotificationProhibitionStatusChanges : IRequest<List<ImportNotificationStatusChangeData>>
    {
        public Guid NotificationId { get; private set; }

        public GetImportNotificationProhibitionStatusChanges(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
