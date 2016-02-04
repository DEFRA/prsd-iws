namespace EA.Iws.Core.Notification
{
    using System;
    using NotificationAssessment;

    public class SubmitSummaryData
    {
        public Guid NotificationId { get; set; }

        public UKCompetentAuthority CompetentAuthority { get; set; }

        public string NotificationNumber { get; set; }

        public DateTime CreatedDate { get; set; }

        public NotificationStatus Status { get; set; }

        public int Charge { get; set; }
    }
}
