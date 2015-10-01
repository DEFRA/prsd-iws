namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.NotificationAssessment;
    using Prsd.Core.Mediator;

    public class GetNotificationAssessmentDecisionData : IRequest<NotificationAssessmentDecisionData>
    {
        public Guid NotificationId { get; private set; }

        public GetNotificationAssessmentDecisionData(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
