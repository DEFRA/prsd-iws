namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportNotificationAssessment;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanEditComments)]
    public class GetImportNotificationComments : IRequest<ImportNotificationCommentsData>
    {
        public Guid NotificationId { get; private set; }

        public GetImportNotificationComments(Guid notificationId)
        {
            this.NotificationId = notificationId;
        }
    }
}
