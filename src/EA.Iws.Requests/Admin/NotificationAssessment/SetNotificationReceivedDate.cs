namespace EA.Iws.Requests.Admin.NotificationAssessment
{
    using System;
    using Prsd.Core.Mediator;

    public class SetNotificationReceivedDate : IRequest<bool>
    {
        public SetNotificationReceivedDate(Guid notificationId, DateTime notificationReceivedDate)
        {
            NotificationId = notificationId;
            NotificationReceivedDate = notificationReceivedDate;
        }

        public Guid NotificationId { get; private set; }

        public DateTime NotificationReceivedDate { get; private set; }
    }
}