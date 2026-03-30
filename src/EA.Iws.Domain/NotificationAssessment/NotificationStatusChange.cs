namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using Core.NotificationAssessment;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class NotificationStatusChange : Entity
    {
        protected NotificationStatusChange()
        {
        }

        public NotificationStatusChange(NotificationStatus status, string userId)
        {
            UserId = userId;
            Status = status;
            ChangeDate = new DateTimeOffset(SystemTime.UtcNow, TimeSpan.Zero);
        }

        public NotificationStatus Status { get; private set; }

        public string UserId { get; private set; }

        public DateTimeOffset ChangeDate { get; private set; }
    }
}