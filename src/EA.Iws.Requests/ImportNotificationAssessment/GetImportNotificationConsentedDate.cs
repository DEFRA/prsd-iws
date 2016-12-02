namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanReadImportNotificationAssessment)]
    public class GetImportNotificationConsentedDate : IRequest<DateTime?>
    {
        public Guid NotificationId { get; private set; }

        public GetImportNotificationConsentedDate(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
