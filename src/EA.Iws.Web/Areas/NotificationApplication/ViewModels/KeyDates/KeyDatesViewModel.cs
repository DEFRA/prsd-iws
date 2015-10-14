namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.KeyDates
{
    using System;
    using Core.NotificationAssessment;

    public class KeyDatesViewModel
    {
        public KeyDatesViewModel(NotificationDatesData dates)
        {
            NotificationReceivedDate = dates.NotificationReceivedDate;
            PaymentReceivedDate = dates.PaymentReceivedDate;
            CommencementDate = dates.CommencementDate;
            CompletedDate = dates.CompletedDate;
            TransmittedDate = dates.TransmittedDate;
            AcknowledgedDate = dates.AcknowledgedDate;
            DecisionRequiredDate = dates.DecisionRequiredDate;
        }
        public Guid NotificationId { get; set; }

        public DateTime? NotificationReceivedDate { get; set; }

        public DateTime? PaymentReceivedDate { get; set; }

        public DateTime? CommencementDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public DateTime? TransmittedDate { get; set; }

        public DateTime? AcknowledgedDate { get; set; }

        public DateTime? DecisionRequiredDate { get; set; }
    }
}