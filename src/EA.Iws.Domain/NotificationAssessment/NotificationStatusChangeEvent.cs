namespace EA.Iws.Domain.NotificationAssessment
{
    using Core.NotificationAssessment;
    using Prsd.Core.Domain;

    public class NotificationStatusChangeEvent : IEvent
    {
        public NotificationAssessment NotificationAssessment { get; private set; }

        public NotificationStatus TargetStatus { get; private set; }

        public NotificationStatusChangeEvent(NotificationAssessment notificationAssessment, NotificationStatus targetStatus)
        {
            NotificationAssessment = notificationAssessment;
            TargetStatus = targetStatus;
        }
    }
}