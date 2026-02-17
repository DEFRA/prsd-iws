namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using EA.Iws.Core.ImportNotificationAssessment;
    using Prsd.Core.Mediator;
    using System;

    [RequestAuthorization(ImportNotificationPermissions.CanReadImportNotification)]
    public class GetImportInterimStatus : IRequest<ImportInterimStatus>
    {
        public GetImportInterimStatus(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}