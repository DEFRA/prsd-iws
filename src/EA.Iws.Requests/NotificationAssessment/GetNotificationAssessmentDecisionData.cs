namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.NotificationAssessment;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotificationAssessment)]
    public class GetNotificationAssessmentDecisionData : IRequest<NotificationAssessmentDecisionData>
    {
        public Guid NotificationId { get; private set; }

        public GetNotificationAssessmentDecisionData(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
