namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.Decision
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Admin;
    using Core.NotificationAssessment;
    using Prsd.Core.Helpers;
    using Web.ViewModels.Shared;

    public class NotificationAssessmentDecisionViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        public NotificationStatus Status { get; set; }

        public IList<DecisionRecordViewModel> PreviousDecisions { get; set; }

        public SelectList PossibleDecisions
        {
            get
            {
                var keyValues = DecisionTypes
                    .Select(e => new KeyValuePair<string, DecisionType>(EnumHelper.GetDisplayName(e), e));
                return new SelectList(keyValues, "Value", "Key");
            }
        }

        [Required]
        [Display(Name = "DecisionLabel", ResourceType = typeof(NotificationAssessmentDecisionViewModelResources))]
        public DecisionType? SelectedDecision { get; set; }

        public IList<DecisionType> DecisionTypes { get; set; }

        public OptionalDateInputViewModel ConsentValidFromDate { get; set; }

        public OptionalDateInputViewModel ConsentValidToDate { get; set; }

        public OptionalDateInputViewModel ConsentedDate { get; set; }

        [Display(Name = "ReasonConsentWithdrawalLabel", ResourceType = typeof(NotificationAssessmentDecisionViewModelResources))]
        public string ReasonsForConsentWithdrawal { get; set; }

        public string ConsentConditions { get; set; }

        [Display(Name = "ObjectedDateLabel", ResourceType = typeof(NotificationAssessmentDecisionViewModelResources))]
        public OptionalDateInputViewModel ObjectionDate { get; set; }

        [Display(Name = "ReasonObjectedLabel", ResourceType = typeof(NotificationAssessmentDecisionViewModelResources))]
        public string ReasonForObjection { get; set; }

        [Display(Name = "WithdrawnDateLabel", ResourceType = typeof(NotificationAssessmentDecisionViewModelResources))]
        public OptionalDateInputViewModel WithdrawnDate { get; set; }

        [Display(Name = "ReasonWithdrawnLabel", ResourceType = typeof(NotificationAssessmentDecisionViewModelResources))]
        public string ReasonForWithdrawal { get; set; }

        public NotificationAssessmentDecisionViewModel()
        {
            ConsentValidFromDate = new OptionalDateInputViewModel();
            ConsentValidToDate = new OptionalDateInputViewModel();
            ConsentedDate = new OptionalDateInputViewModel(true);
            PreviousDecisions = new List<DecisionRecordViewModel>();
            DecisionTypes = new List<DecisionType>();
            ObjectionDate = new OptionalDateInputViewModel(true);
            WithdrawnDate = new OptionalDateInputViewModel(true);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SelectedDecision == DecisionType.Consent)
            {
                return ValidateConsent();
            }

            if (SelectedDecision == DecisionType.ConsentWithdrawn)
            {
                return ValidateConsentWithdrawn();
            }

            if (SelectedDecision == DecisionType.Object)
            {
                return ValidateObject();
            }

            if (SelectedDecision == DecisionType.Withdrawn)
            {
                return ValidateWithdrawn();
            }

            return new ValidationResult[0];
        }

        private IEnumerable<ValidationResult> ValidateConsent()
        {
            if (!ConsentValidFromDate.IsCompleted)
            {
                yield return new ValidationResult(NotificationAssessmentDecisionViewModelResources.ConsentValidFromRequired,
                    new[] { "ConsentValidFromDate" });
            }

            if (!ConsentValidToDate.IsCompleted)
            {
                yield return new ValidationResult(NotificationAssessmentDecisionViewModelResources.ConsentValidToRequired,
                    new[] { "ConsentValidToDate" });
            }

            if (!ConsentedDate.IsCompleted)
            {
                yield return new ValidationResult(NotificationAssessmentDecisionViewModelResources.ConsentedDateRequired,
                    new[] { "ConsentedDate" });
            }

            if (ConsentValidFromDate.IsCompleted && ConsentValidFromDate.IsCompleted
                && ConsentValidFromDate.AsDateTime() > ConsentValidToDate.AsDateTime())
            {
                yield return new ValidationResult(NotificationAssessmentDecisionViewModelResources.ConsentValidFromBeforeValidTo,
                    new[] { "ConsentValidFromDate" });
            }
        }

        private IEnumerable<ValidationResult> ValidateConsentWithdrawn()
        {
            if (string.IsNullOrWhiteSpace(ReasonsForConsentWithdrawal))
            {
                yield return new ValidationResult(NotificationAssessmentDecisionViewModelResources.ReasonConsentWithdrawnRequired,
                    new[] { "ReasonsForConsentWithdrawal" });
            }
        }

        private IEnumerable<ValidationResult> ValidateObject()
        {
            if (string.IsNullOrWhiteSpace(ReasonForObjection))
            {
                yield return new ValidationResult(NotificationAssessmentDecisionViewModelResources.ReasonObjectedRequired,
                    new[] { "ReasonForObjection" });
            }

            if (!ObjectionDate.IsCompleted)
            {
                yield return new ValidationResult(NotificationAssessmentDecisionViewModelResources.ObjectedDateRequired, new[] { "ObjectionDate" });
            }
        }

        private IEnumerable<ValidationResult> ValidateWithdrawn()
        {
            if (string.IsNullOrWhiteSpace(ReasonForWithdrawal))
            {
                yield return new ValidationResult(NotificationAssessmentDecisionViewModelResources.ReasonWithdrawnRequired,
                    new[] { "ReasonForWithdrawal" });
            }

            if (!WithdrawnDate.IsCompleted)
            {
                yield return new ValidationResult(NotificationAssessmentDecisionViewModelResources.WithdrawnDateRequired, new[] { "WithdrawnDate" });
            }
        }
    }
}