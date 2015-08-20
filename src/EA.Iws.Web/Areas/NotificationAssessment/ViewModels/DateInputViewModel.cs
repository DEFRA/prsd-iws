namespace EA.Iws.Web.Areas.NotificationAssessment.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.NotificationAssessment;
    using Web.ViewModels.Shared;

    public class DateInputViewModel : IValidatableObject
    {
        public static readonly string NotificationReceived = "notificationReceived";
        public static readonly string PaymentReceived = "paymentReceived";

        public DateInputViewModel()
        {
            NotificationReceivedDate = new OptionalDateInputViewModel(true);
            PaymentReceivedDate = new OptionalDateInputViewModel(true);
            CommencementDate = new OptionalDateInputViewModel(true);
            NotificationCompleteDate = new OptionalDateInputViewModel(true);
            NotificationTransmittedDate = new OptionalDateInputViewModel(true);
            NotificationAcknowledgedDate = new OptionalDateInputViewModel(true);
            DecisionDate = new OptionalDateInputViewModel(true);
        }

        public DateInputViewModel(NotificationDatesData dates)
        {
            NotificationId = dates.NotificationId;
            NotificationReceivedDate = new OptionalDateInputViewModel(dates.NotificationReceivedDate, true);
            PaymentReceivedDate = new OptionalDateInputViewModel(true);
            CommencementDate = new OptionalDateInputViewModel(true);
            NotificationCompleteDate = new OptionalDateInputViewModel(true);
            NotificationTransmittedDate = new OptionalDateInputViewModel(true);
            NotificationAcknowledgedDate = new OptionalDateInputViewModel(true);
            DecisionDate = new OptionalDateInputViewModel(true);
        }

        public Guid NotificationId { get; set; }

        public string Command { get; set; }

        public OptionalDateInputViewModel NotificationReceivedDate { get; set; }

        public OptionalDateInputViewModel PaymentReceivedDate { get; set; }

        public OptionalDateInputViewModel CommencementDate { get; set; }

        public OptionalDateInputViewModel NotificationCompleteDate { get; set; }

        public OptionalDateInputViewModel NotificationTransmittedDate { get; set; }

        public OptionalDateInputViewModel NotificationAcknowledgedDate { get; set; }

        public OptionalDateInputViewModel DecisionDate { get; set; }

        [Display(Name = "Name of officer")]
        public string NameOfOfficer { get; set; }

        public bool CommencementComplete
        {
            get { return CommencementDate.AsDateTime() != null && !string.IsNullOrWhiteSpace(NameOfOfficer); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Command == NotificationReceived && !NotificationReceivedDate.IsCompleted)
            {
                yield return new ValidationResult("Please enter the notification received date", new[] { "NotificationReceivedDate" });
            }

            if (Command == PaymentReceived && !PaymentReceivedDate.IsCompleted)
            {
                yield return new ValidationResult("Please enter the payment received date", new[] { "PaymentReceivedDate" });
            }

            if ((CommencementDate.IsCompleted || !string.IsNullOrWhiteSpace(NameOfOfficer)) && (!CommencementDate.IsCompleted || string.IsNullOrWhiteSpace(NameOfOfficer)))
            {
                yield return new ValidationResult("Please complete the date and name of officer", new[] { "CommencementDate" });
            }
        }
    }
}