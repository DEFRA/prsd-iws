namespace EA.Iws.Core.Admin.KeyDates
{
    using System;

    public class KeyDatesOverrideData
    {
        public Guid NotificationId { get; set; }

        public DateTime? NotificationReceivedDate { get; set; }

        public DateTime? CommencementDate { get; set; }

        public DateTime? CompleteDate { get; set; }

        public DateTime? TransmittedDate { get; set; }

        public DateTime? AcknowledgedDate { get; set; }

        public DateTime? WithdrawnDate { get; set; }

        public DateTime? ObjectedDate { get; set; }

        public DateTime? ConsentedDate { get; set; }

        public DateTime? ConsentValidFromDate { get; set; }

        public DateTime? ConsentValidToDate { get; set; }
    }
}