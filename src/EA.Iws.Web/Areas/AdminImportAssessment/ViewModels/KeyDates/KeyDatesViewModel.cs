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
            AssessmentStartedDate = new OptionalDateInputViewModel(true);
            NotificationCompleteDate = new OptionalDateInputViewModel(true);
            NotificationAcknowledgedDate = new OptionalDateInputViewModel(true);
            DecisionDate = new OptionalDateInputViewModel(true);
            NewDate = new OptionalDateInputViewModel(true);
            Decisions = new List<NotificationAssessmentDecision>();
        }

        public KeyDatesViewModel(KeyDatesData keyDates)
        {
            NotificationReceivedDate = new OptionalDateInputViewModel(keyDates.NotificationReceived, true);
            PaymentReceivedDate = keyDates.PaymentReceived;
            PaymentIsComplete = keyDates.IsPaymentComplete;
            AssessmentStartedDate = new OptionalDateInputViewModel(keyDates.AssessmentStarted, true);
            NameOfOfficer = keyDates.NameOfOfficer;
            NotificationCompleteDate = new OptionalDateInputViewModel(keyDates.NotificationCompletedDate, true);
            NotificationAcknowledgedDate = new OptionalDateInputViewModel(keyDates.AcknowlegedDate, true);
            DecisionDate = new OptionalDateInputViewModel(keyDates.DecisionRequiredByDate, true);
            NewDate = new OptionalDateInputViewModel(true);
            Decisions = new List<NotificationAssessmentDecision>();
            IsInterim = keyDates.IsInterim;
        }

        public Guid NotificationId { get; set; }

        public KeyDatesCommand Command { get; set; }

        [Display(Name = "NotificationReceivedDate", ResourceType = typeof(KeyDatesViewModelResources))]
        public OptionalDateInputViewModel NotificationReceivedDate { get; set; }

        [Display(Name = "PaymentReceivedDate", ResourceType = typeof(KeyDatesViewModelResources))]
        public DateTime? PaymentReceivedDate { get; set; }

        public bool PaymentIsComplete { get; set; }

        [Display(Name = "CommencementDate", ResourceType = typeof(KeyDatesViewModelResources))]
        public OptionalDateInputViewModel AssessmentStartedDate { get; set; }

        [Display(Name = "NotificationCompletedDate", ResourceType = typeof(KeyDatesViewModelResources))]
        public OptionalDateInputViewModel NotificationCompleteDate { get; set; }

        [Display(Name = "NotificationAcknowledgedDate", ResourceType = typeof(KeyDatesViewModelResources))]
        public OptionalDateInputViewModel NotificationAcknowledgedDate { get; set; }

        [Display(Name = "DecisionDate", ResourceType = typeof(KeyDatesViewModelResources))]
        public OptionalDateInputViewModel DecisionDate { get; set; }

        [Display(Name = "NameOfOfficer", ResourceType = typeof(KeyDatesViewModelResources))]
        public string NameOfOfficer { get; set; }

        public OptionalDateInputViewModel NewDate { get; set; }

        public bool IsInterim { get; set; }

        public bool CommencementComplete
        {
            get { return AssessmentStartedDate.AsDateTime() != null && !string.IsNullOrWhiteSpace(NameOfOfficer); }
        }

        public IList<NotificationAssessmentDecision> Decisions { get; set; } 

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!NewDate.IsCompleted)
            {
                yield return new ValidationResult(KeyDatesViewModelResources.DateRequiredError, new[] {"NewDate"});
            }

            if (Command == KeyDatesCommand.BeginAssessment)
            {
                if (string.IsNullOrWhiteSpace(NameOfOfficer))
                {
                    yield return new ValidationResult(KeyDatesViewModelResources.NameOfOfficerRequiredError, new[] { "NameOfOfficer" });
                }
                else if (NameOfOfficer.Length > 256)
                {
                    yield return new ValidationResult(KeyDatesViewModelResources.NameOfOfficerLengthError, new[] { "NameOfOfficer" });
                }

                if (NewDate.AsDateTime() > DateTime.UtcNow)
                {
                    yield return new ValidationResult(KeyDatesViewModelResources.CommencementNotInFuture, new[] { "NewDate" });
                }

                if (NotificationReceivedDate == null)
                {
                    yield return new ValidationResult(KeyDatesViewModelResources.CommencementOthersRequired, new[] { "NewDate" });
                }

                if ((NotificationReceivedDate != null && NewDate.AsDateTime() < NotificationReceivedDate.AsDateTime()))
                {
                    yield return new ValidationResult(KeyDatesViewModelResources.CommencementNotBeforeOthers, new[] { "NewDate" });
                }
            }

            if (Command == KeyDatesCommand.NotificationComplete)
            {
                if (NewDate.AsDateTime() > DateTime.UtcNow)
                {
                    yield return new ValidationResult(KeyDatesViewModelResources.CompletedNotInFuture, new[] { "NewDate" });
                }

                if (NotificationReceivedDate == null)
                {
                    yield return new ValidationResult(KeyDatesViewModelResources.CompletedOthersRequired, new[] { "NewDate" });
                }

                if ((NotificationReceivedDate != null && NewDate.AsDateTime() < NotificationReceivedDate.AsDateTime()))
                {
                    yield return new ValidationResult(KeyDatesViewModelResources.CompletedNotBeforeOthers, new[] { "NewDate" });
                }
            }
        }
    }
}