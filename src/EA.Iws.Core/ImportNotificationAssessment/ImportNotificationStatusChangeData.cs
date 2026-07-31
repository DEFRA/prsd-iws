namespace EA.Iws.Core.ImportNotificationAssessment
{
    using EA.Iws.Core.Registration.Users;
    using System;
    public class ImportNotificationStatusChangeData
    {
        public ImportNotificationStatus Status { get; set; }

        public string UserId { get; set; }

        public string FullName { get; set; }

        public User User { get; set; }

        public DateTime ChangeDate { get; set; }
    }
}
