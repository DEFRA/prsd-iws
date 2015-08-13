namespace EA.Iws.Web.Areas.NotificationAssessment.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
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
        }

        public Guid NotificationId { get; set; }

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
            if ((CommencementDate.IsCompleted || !string.IsNullOrWhiteSpace(NameOfOfficer)) && (!CommencementDate.IsCompleted || string.IsNullOrWhiteSpace(NameOfOfficer)))
            {
                yield return new ValidationResult("Please complete the date and name of officer", new[] { "CommencementDate" });
            }
        }
    }
}