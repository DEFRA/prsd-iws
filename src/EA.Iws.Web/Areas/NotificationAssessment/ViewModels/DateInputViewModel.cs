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
        public static readonly string AssessmentCommenced = "assessmentCommenced";
        public static readonly string NotificationComplete = "notificationComplete";

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
            PaymentReceivedDate = new OptionalDateInputViewModel(dates.PaymentReceivedDate, true);
            CommencementDate = new OptionalDateInputViewModel(dates.CommencementDate, true);
            NotificationCompleteDate = new OptionalDateInputViewModel(dates.CompletedDate, true);
            NotificationTransmittedDate = new OptionalDateInputViewModel(true);
            NotificationAcknowledgedDate = new OptionalDateInputViewModel(true);
            DecisionDate = new OptionalDateInputViewModel(true);
            NameOfOfficer = dates.NameOfOfficer;
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
            else if (Command == PaymentReceived && !PaymentReceivedDate.IsCompleted)
            {
                yield return new ValidationResult("Please enter the payment received date", new[] { "PaymentReceivedDate" });
            }
            else if (Command == AssessmentCommenced)
            {
                if (!CommencementDate.IsCompleted)
                {
                    yield return new ValidationResult("Please enter the assessment commenced date", new[] { "CommencementDate" });
                }

                if (string.IsNullOrWhiteSpace(NameOfOfficer))
                {
                    yield return new ValidationResult("Please enter the name of the officer", new[] { "NameOfOfficer" });
                }
            }
            else if (Command == NotificationComplete && !NotificationCompleteDate.IsCompleted)
            {
                yield return new ValidationResult("Please enter the notification complete date", new[] { "NotificationCompleteDate" });   
            }
        }
    }
}