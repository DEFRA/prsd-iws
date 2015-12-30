namespace EA.Iws.Web.Areas.AdminImportAssessment.ViewModels.KeyDates
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.ImportNotificationAssessment;
    using Core.NotificationAssessment;
    using Web.ViewModels.Shared;

    public class KeyDatesViewModel : IValidatableObject
    {
        public KeyDatesViewModel()
        {
            NotificationReceivedDate = new OptionalDateInputViewModel(true);
            CommencementDate = new OptionalDateInputViewModel(true);
            NotificationCompleteDate = new OptionalDateInputViewModel(true);
            NotificationAcknowledgedDate = new OptionalDateInputViewModel(true);
            DecisionDate = new OptionalDateInputViewModel(true);
            NewDate = new OptionalDateInputViewModel(true);
            Decisions = new List<NotificationAssessmentDecision>();
        }

        public KeyDatesViewModel(KeyDatesData keyDates)
        {
            NotificationReceivedDate = new OptionalDateInputViewModel(keyDates.NotificationReceived, true);
            CommencementDate = new OptionalDateInputViewModel(true);
            NotificationCompleteDate = new OptionalDateInputViewModel(true);
            NotificationAcknowledgedDate = new OptionalDateInputViewModel(true);
            DecisionDate = new OptionalDateInputViewModel(true);
            NewDate = new OptionalDateInputViewModel(true);
            Decisions = new List<NotificationAssessmentDecision>();
        }

        public Guid NotificationId { get; set; }

        public KeyDatesStatusEnum Command { get; set; }

        [Display(Name = "NotificationReceivedDate", ResourceType = typeof(KeyDatesViewModelResources))]
        public OptionalDateInputViewModel NotificationReceivedDate { get; set; }

        [Display(Name = "PaymentReceivedDate", ResourceType = typeof(KeyDatesViewModelResources))]
        public DateTime? PaymentReceivedDate { get; set; }

        public bool PaymentIsComplete { get; set; }

        [Display(Name = "CommencementDate", ResourceType = typeof(KeyDatesViewModelResources))]
        public OptionalDateInputViewModel CommencementDate { get; set; }

        [Display(Name = "NotificationCompletedDate", ResourceType = typeof(KeyDatesViewModelResources))]
        public OptionalDateInputViewModel NotificationCompleteDate { get; set; }

        [Display(Name = "NotificationAcknowledgedDate", ResourceType = typeof(KeyDatesViewModelResources))]
        public OptionalDateInputViewModel NotificationAcknowledgedDate { get; set; }

        [Display(Name = "DecisionDate", ResourceType = typeof(KeyDatesViewModelResources))]
        public OptionalDateInputViewModel DecisionDate { get; set; }

        [Display(Name = "NameOfOfficer", ResourceType = typeof(KeyDatesViewModelResources))]
        public string NameOfOfficer { get; set; }

        public OptionalDateInputViewModel NewDate { get; set; }

        public bool CommencementComplete
        {
            get { return CommencementDate.AsDateTime() != null && !string.IsNullOrWhiteSpace(NameOfOfficer); }
        }

        public IList<NotificationAssessmentDecision> Decisions { get; set; } 

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!NewDate.IsCompleted)
            {
                yield return new ValidationResult(KeyDatesViewModelResources.DateRequiredError, new[] {"NewDate"});
            }

            if (Command == KeyDatesStatusEnum.AssessmentCommenced)
            {
                if (string.IsNullOrWhiteSpace(NameOfOfficer))
                {
                    yield return new ValidationResult(KeyDatesViewModelResources.NameOfOfficerRequiredError, new[] { "NameOfOfficer" });
                }
                else if (NameOfOfficer.Length > 256)
                {
                    yield return new ValidationResult(KeyDatesViewModelResources.NameOfOfficerLengthError, new[] { "NameOfOfficer" });
                }
            }
        }
    }
}