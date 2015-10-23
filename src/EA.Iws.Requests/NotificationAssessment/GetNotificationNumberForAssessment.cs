namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Prsd.Core.Mediator;

    public class GetNotificationNumberForAssessment : IRequest<string>
    {
        public Guid NotificationAssessmentId { get; private set; }

        public GetNotificationNumberForAssessment(Guid notificationAssessmentId)
        {
            NotificationAssessmentId = notificationAssessmentId;
        }
    }
}
