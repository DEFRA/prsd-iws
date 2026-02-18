namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using EA.Iws.Core.Authorization;
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Iws.Domain.ImportNotificationAssessment;
    using EA.Prsd.Core.Mediator;
    using System;

    [RequestAuthorization(ImportNotificationPermissions.CanReadImportNotificationAssessment)]

    public class GetImportNotificationStatusChange : IRequest<ImportNotificationStatusChange>
    {
        public Guid NotificationId { get; private set; }

        public GetImportNotificationStatusChange(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
