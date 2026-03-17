namespace EA.Iws.Core.NotificationAssessment
{
    using EA.Iws.Core.Registration.Users;
    using System;
    public class NotificationStatusChangeData
    {
        public NotificationStatus Status { get; set; }

        public string UserId { get; set; }

        public string FullName { get; set; }

        public User User { get; set; }

        public DateTime? ChangeDate { get; set; }
    }
}
