namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using Core.Notification;
    using Core.NotificationAssessment;
    using Prsd.Core;
    using Web.ViewModels.Shared;

    public class DateInputViewModel : IValidatableObject
    {
        public DateInputViewModel()
        {
            NotificationReceivedDate = new OptionalDateInputViewModel(true);
            CommencementDate = new OptionalDateInputViewModel(true);
            NotificationCompleteDate = new OptionalDateInputViewModel(true);
            NotificationTransmittedDate = new OptionalDateInputViewModel(true);
            NotificationAcknowledgedDate = new OptionalDateInputViewModel(true);
            DecisionDate = new OptionalDateInputViewModel(true);
            NewDate = new OptionalDateInputViewModel(true);
            AssessmentDecisions = new List<NotificationAssessmentDecision>();
            NotificationFileClosedDate = new OptionalDateInputViewModel(true);
        }

        public DateInputViewModel(NotificationDatesData dates)
        {
            CurrentStatus = dates.CurrentStatus;
            NotificationId = dates.NotificationId;
            NotificationReceivedDate = new OptionalDateInputViewModel(dates.NotificationReceivedDate, true);
            PaymentReceivedDate = dates.PaymentReceivedDate;
            PaymentIsComplete = dates.PaymentIsComplete;
            CommencementDate = new OptionalDateInputViewModel(dates.CommencementDate, true);
            NotificationCompleteDate = new OptionalDateInputViewModel(dates.CompletedDate, true);
            NotificationTransmittedDate = new OptionalDateInputViewModel(dates.TransmittedDate, true);
            NotificationAcknowledgedDate = new OptionalDateInputViewModel(dates.AcknowledgedDate, true);
            DecisionDate = new OptionalDateInputViewModel(dates.DecisionRequiredDate, true);
            NewDate = new OptionalDateInputViewModel(true);
            NameOfOfficer = dates.NameOfOfficer;
            AssessmentDecisions = new List<NotificationAssessmentDecision>();
            NotificationFileClosedDate = new OptionalDateInputViewModel(dates.FileClosedDate, true);
            ArchiveReference = dates.ArchiveReference;
        }

        public Guid NotificationId { get; set; }

        public KeyDatesStatusEnum Command { get; set; }

        public NotificationStatus CurrentStatus { get; set; }

        [Display(Name = "Notification received")]
        public OptionalDateInputViewModel NotificationReceivedDate { get; set; }

        [Display(Name = "Payment received")]
        public DateTime? PaymentReceivedDate { get; set; }

        public bool PaymentIsComplete { get; set; }

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

        [Display(Name = "File closed on")]
        public OptionalDateInputViewModel NotificationFileClosedDate { get; set; }

        [Display(Name = "Archive reference")]
        public string ArchiveReference { get; set; }

        [Display(Name = "Name of officer")]
        public string NameOfOfficer { get; set; }

        public OptionalDateInputViewModel NewDate { get; set; }

        public bool CommencementComplete
        {
            get { return CommencementDate.AsDateTime() != null && !string.IsNullOrWhiteSpace(NameOfOfficer); }
        }

        public IList<NotificationAssessmentDecision> AssessmentDecisions { get; set; }

        public bool IsAreaAssigned { get; set; }

        public UKCompetentAuthority CompetentAuthority { get; set; }

        public bool? IsInterim { get; set; }

        public bool ShowAssessmentDecisionLink { get; set; }

        public bool CanSetDates
        {
            get
            {
                return CurrentStatus != NotificationStatus.Withdrawn
                       || CurrentStatus != NotificationStatus.FileClosed;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Command != KeyDatesStatusEnum.ArchiveReference && !NewDate.IsCompleted)
            {
                yield return new ValidationResult("Please enter a valid date", new[] {"NewDate"});
            }

            if (Command == KeyDatesStatusEnum.NotificationReceived)
            {
                if (NewDate.AsDateTime() > SystemTime.UtcNow)
                {
                    yield return new ValidationResult(DateInputViewModelResources.ReceivedInFuture, new[] { "NewDate" });
                }
            }

            if (Command == KeyDatesStatusEnum.AssessmentCommenced)
            {
                if (string.IsNullOrWhiteSpace(NameOfOfficer))
                {
                    yield return new ValidationResult(DateInputViewModelResources.NameOfOfficer, new[] { "NameOfOfficer" });
                }
                else if (NameOfOfficer.Count() > 256)
                {
                    yield return new ValidationResult(DateInputViewModelResources.NameOfOfficerLength, new[] { "NameOfOfficer" });
                }

                if (NewDate.AsDateTime() > SystemTime.UtcNow)
                {
                    yield return new ValidationResult(DateInputViewModelResources.CommencedInFuture, new[] { "NewDate" });
                }

                if (NotificationReceivedDate == null || PaymentReceivedDate == null)
                {
                    yield return new ValidationResult(DateInputViewModelResources.CommencedOtherDatesRequired, new[] { "NewDate" });
                }

                if ((NotificationReceivedDate != null && NewDate.AsDateTime() < NotificationReceivedDate.AsDateTime()) ||
                    (PaymentReceivedDate != null && NewDate.AsDateTime() < PaymentReceivedDate))
                {
                    yield return new ValidationResult(DateInputViewModelResources.CommencedNotBefore, new[] { "NewDate" });
                }
            }

            if (Command == KeyDatesStatusEnum.NotificationComplete)
            {
                if (NewDate.AsDateTime() > SystemTime.UtcNow)
                {
                    yield return new ValidationResult(DateInputViewModelResources.CompleteInFuture, new[] { "NewDate" });
                }

                if (NotificationReceivedDate == null || PaymentReceivedDate == null || CommencementDate == null)
                {
                    yield return new ValidationResult(DateInputViewModelResources.CompleteOtherDatesRequired, new[] { "NewDate" });
                }

                if ((NotificationReceivedDate != null && NewDate.AsDateTime() < NotificationReceivedDate.AsDateTime()) ||
                    (PaymentReceivedDate != null && NewDate.AsDateTime() < PaymentReceivedDate) ||
                    (CommencementDate != null && NewDate.AsDateTime() < CommencementDate.AsDateTime()))
                {
                    yield return new ValidationResult(DateInputViewModelResources.CompleteNotBefore, new[] { "NewDate" });
                }
            }

            if (Command == KeyDatesStatusEnum.NotificationTransmitted)
            {
                if (NewDate.AsDateTime() > SystemTime.UtcNow)
                {
                    yield return new ValidationResult(DateInputViewModelResources.TransmittedInFuture, new[] { "NewDate" });
                }

                if (NotificationReceivedDate == null || PaymentReceivedDate == null || CommencementDate == null ||
                    NotificationCompleteDate == null)
                {
                    yield return new ValidationResult(DateInputViewModelResources.TransmittedOtherDatesRequired, new[] { "NewDate" });
                }

                if ((NotificationReceivedDate != null && NewDate.AsDateTime() < NotificationReceivedDate.AsDateTime()) ||
                    (PaymentReceivedDate != null && NewDate.AsDateTime() < PaymentReceivedDate) ||
                    (CommencementDate != null && NewDate.AsDateTime() < CommencementDate.AsDateTime()) ||
                    (NotificationCompleteDate != null && NewDate.AsDateTime() < NotificationCompleteDate.AsDateTime()))
                {
                    yield return new ValidationResult(DateInputViewModelResources.TransmittedNotBefore, new[] { "NewDate" });
                }
            }

            if (Command == KeyDatesStatusEnum.NotificationAcknowledged)
            {
                if (NewDate.AsDateTime() > SystemTime.UtcNow)
                {
                    yield return new ValidationResult(DateInputViewModelResources.AcknowledgedInFuture, new[] { "NewDate" });
                }

                if (NotificationReceivedDate == null || PaymentReceivedDate == null || CommencementDate == null ||
                    NotificationCompleteDate == null || NotificationTransmittedDate == null)
                {
                    yield return new ValidationResult(DateInputViewModelResources.AcknowledgedOtherDatesRequired, new[] { "NewDate" });
                }

                if ((NotificationReceivedDate != null && NewDate.AsDateTime() < NotificationReceivedDate.AsDateTime()) ||
                    (PaymentReceivedDate != null && NewDate.AsDateTime() < PaymentReceivedDate) ||
                    (CommencementDate != null && NewDate.AsDateTime() < CommencementDate.AsDateTime()) ||
                    (NotificationCompleteDate != null && NewDate.AsDateTime() < NotificationCompleteDate.AsDateTime()) ||
                    (NotificationTransmittedDate != null && NewDate.AsDateTime() < NotificationTransmittedDate.AsDateTime()))
                {
                    yield return new ValidationResult(DateInputViewModelResources.AcknowledgedNotBefore, new[] { "NewDate" });
                }
            }

            if (Command == KeyDatesStatusEnum.FileClosed)
            {
                if (NewDate.AsDateTime() > SystemTime.UtcNow)
                {
                    yield return new ValidationResult(DateInputViewModelResources.FileClosedInFuture, new[] { "NewDate" });
                }

                if (NotificationReceivedDate == null)
                {
                    yield return new ValidationResult(DateInputViewModelResources.CompleteOtherDatesRequired, new[] { "NewDate" });
                }

                if (NotificationReceivedDate != null && NewDate.AsDateTime() < NotificationReceivedDate.AsDateTime())
                {
                    yield return new ValidationResult(DateInputViewModelResources.FileClosedNotBefore, new[] { "NewDate" });
                }
            }

            if (Command == KeyDatesStatusEnum.ArchiveReference)
            {
                if (string.IsNullOrWhiteSpace(ArchiveReference))
                {
                    yield return new ValidationResult(DateInputViewModelResources.ArchiveReferenceRequired, new[] { "ArchiveReference" });
                }
                else if (ArchiveReference.Length > 100)
                {
                    yield return new ValidationResult(DateInputViewModelResources.ArchiveReferenceLength, new[] { "ArchiveReference" });
                }
            }
        }
    }
}