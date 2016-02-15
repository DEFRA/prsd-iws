namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanReadImportNotificationAssessment)]
    public class GetImportNotificationLocalAreaId : IRequest<Guid>
    {
        public Guid NotificationId { get; private set; }

        public GetImportNotificationLocalAreaId(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
