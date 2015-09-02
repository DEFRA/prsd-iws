namespace EA.Iws.Requests.Admin.NotificationAssessment
{
    using System;
    using Prsd.Core.Mediator;

    public class SetNotificationDecisionRequiredByDate : IRequest<bool>
    {
        public SetNotificationDecisionRequiredByDate(Guid notificationId, DateTime decisionRequiredByDate)
        {
            NotificationId = notificationId;
            DecisionRequiredByDate = decisionRequiredByDate;
        }

        public Guid NotificationId { get; private set; }

        public DateTime DecisionRequiredByDate { get; private set; }
    }
}