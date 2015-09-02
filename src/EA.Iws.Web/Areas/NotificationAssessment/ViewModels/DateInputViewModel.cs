namespace EA.Iws.Web.Areas.NotificationAssessment.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.NotificationAssessment;
    using Web.ViewModels.Shared;

    public class DateInputViewModel : IValidatableObject
    {
        public DateInputViewModel()
        {
            NotificationReceivedDate = new OptionalDateInputViewModel(true);
            PaymentReceivedDate = new OptionalDateInputViewModel(true);
            CommencementDate = new OptionalDateInputViewModel(true);
            NotificationCompleteDate = new OptionalDateInputViewModel(true);
            NotificationTransmittedDate = new OptionalDateInputViewModel(true);
            NotificationAcknowledgedDate = new OptionalDateInputViewModel(true);
            DecisionDate = new OptionalDateInputViewModel(true);
            NewDate = new OptionalDateInputViewModel(true);
        }

        public DateInputViewModel(NotificationDatesData dates)
        {
            NotificationId = dates.NotificationId;
            NotificationReceivedDate = new OptionalDateInputViewModel(dates.NotificationReceivedDate, true);
            PaymentReceivedDate = new OptionalDateInputViewModel(dates.PaymentReceivedDate, true);
            CommencementDate = new OptionalDateInputViewModel(dates.CommencementDate, true);
            NotificationCompleteDate = new OptionalDateInputViewModel(dates.CompletedDate, true);
            NotificationTransmittedDate = new OptionalDateInputViewModel(dates.TransmittedDate, true);
            NotificationAcknowledgedDate = new OptionalDateInputViewModel(dates.AcknowledgedDate, true);
            DecisionDate = new OptionalDateInputViewModel(dates.DecisionRequiredDate, true);
            NewDate = new OptionalDateInputViewModel(true);
            NameOfOfficer = dates.NameOfOfficer;
        }

        public Guid NotificationId { get; set; }

        public KeyDatesStatusEnum Command { get; set; }

        [Display(Name = "Notification received")]
        public OptionalDateInputViewModel NotificationReceivedDate { get; set; }

        [Display(Name = "Payment received")]
        public OptionalDateInputViewModel PaymentReceivedDate { get; set; }

        [Display(Name = "Assessment started")]
        public OptionalDateInputViewModel CommencementDate { get; set; }

        [Display(Name = "Date completed")]
        public OptionalDateInputViewModel NotificationCompleteDate { get; set; }

        [Display(Name = "Transmitted on")]
        public OptionalDateInputViewModel NotificationTransmittedDate { get; set; }

        [Display(Name = "Acknowledged on")]
        public OptionalDateInputViewModel NotificationAcknowledgedDate { get; set; }

        [Display(Name = "Decision required by")]
        public OptionalDateInputViewModel DecisionDate { get; set; }

        [Display(Name = "Name of officer")]
        public string NameOfOfficer { get; set; }

        public OptionalDateInputViewModel NewDate { get; set; }

        public bool CommencementComplete
        {
            get { return CommencementDate.AsDateTime() != null && !string.IsNullOrWhiteSpace(NameOfOfficer); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!NewDate.IsCompleted)
            {
                yield return new ValidationResult("Please enter a valid date", new[] {"NewDate"});
            }

            if (Command == KeyDatesStatusEnum.AssessmentCommenced && string.IsNullOrWhiteSpace(NameOfOfficer))
            {
                yield return new ValidationResult("Please enter the name of the officer", new[] { "NameOfOfficer" });
            }
        }
    }
}