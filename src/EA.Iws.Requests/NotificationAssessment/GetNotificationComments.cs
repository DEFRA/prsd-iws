namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.NotificationAssessment;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditInternalComments)]
    public class GetNotificationComments : IRequest<NotificationCommentData>
    {
        public Guid NotificationId { get; private set; }

        public GetNotificationComments(Guid notificationId)
        {
            this.NotificationId = notificationId;
        }
    }
}
