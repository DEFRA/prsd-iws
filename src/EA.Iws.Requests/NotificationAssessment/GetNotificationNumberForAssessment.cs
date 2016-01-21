namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotificationAssessment)]
    public class GetNotificationNumberForAssessment : IRequest<string>
    {
        public Guid NotificationAssessmentId { get; private set; }

        public GetNotificationNumberForAssessment(Guid notificationAssessmentId)
        {
            NotificationAssessmentId = notificationAssessmentId;
        }
    }
}
