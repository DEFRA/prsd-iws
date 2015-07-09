namespace EA.Iws.Core.Notification
{
    using System;

    public class SubmitSummaryData
    {
        public CompetentAuthority CompetentAuthority { get; set; }

        public string NotificationNumber { get; set; }

        public DateTime CreatedDate { get; set; }

        public Status Status { get; set; }

        public int Charge { get; set; }
    }
}
