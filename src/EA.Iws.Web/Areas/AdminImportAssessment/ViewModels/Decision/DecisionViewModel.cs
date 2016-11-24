namespace EA.Iws.Web.Areas.AdminImportAssessment.ViewModels.Decision
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Admin;
    using Core.ImportNotificationAssessment;
    using Prsd.Core;
    using Prsd.Core.Helpers;
    using Web.ViewModels.Shared;

    public class DecisionViewModel : IValidatableObject
    {
        public ImportNotificationStatus Status { get; set; }
        
        [Display(Name = "Decision", ResourceType = typeof(DecisionViewModelResources))]
        [Required(ErrorMessageResourceName = "DecisionRequired", ErrorMessageResourceType = typeof(DecisionViewModelResources))]
        public DecisionType? Decision { get; set; }

        public IList<DecisionType> DecisionTypes { get; set; }

        public SelectList PossibleDecisions
        {
            get
            {
                var keyValues = DecisionTypes
                    .Select(e => new KeyValuePair<string, DecisionType>(EnumHelper.GetDisplayName(e), e));
                return new SelectList(keyValues, "Value", "Key");
            }
        }

        [Display(Name = "ConsentGiven", ResourceType = typeof(DecisionViewModelResources))]
        public OptionalDateInputViewModel ConsentGivenDate { get; set; }

        [Display(Name = "ConsentValidFrom", ResourceType = typeof(DecisionViewModelResources))]
        public OptionalDateInputViewModel ConsentValidFromDate { get; set; }

        [Display(Name = "ConsentValidTo", ResourceType = typeof(DecisionViewModelResources))]
        public OptionalDateInputViewModel ConsentValidToDate { get; set; }

        [Display(Name = "ConsentConditions", ResourceType = typeof(DecisionViewModelResources))]
        public string ConsentConditions { get; set; }

        [Display(Name = "ConsentWithdrawnDateLabel", ResourceType = typeof(DecisionViewModelResources))]
        public OptionalDateInputViewModel ConsentWithdrawnDate { get; set; }

        [Display(Name = "ReasonConsentWithdrawalLabel", ResourceType = typeof(DecisionViewModelResources))]
        public string ReasonsForConsentWithdrawal { get; set; }

        [Display(Name = "ObjectedDateLabel", ResourceType = typeof(DecisionViewModelResources))]
        public OptionalDateInputViewModel ObjectionDate { get; set; }

        [Display(Name = "ReasonObjectedLabel", ResourceType = typeof(DecisionViewModelResources))]
        public string ReasonForObjection { get; set; }

        [Display(Name = "WithdrawnDateLabel", ResourceType = typeof(DecisionViewModelResources))]
        public OptionalDateInputViewModel WithdrawnDate { get; set; }

        [Display(Name = "ReasonWithdrawnLabel", ResourceType = typeof(DecisionViewModelResources))]
        public string ReasonForWithdrawal { get; set; }

        public OptionalDateInputViewModel NotificationReceivedDate { get; set; }

        public DecisionViewModel()
        {
            ConsentValidFromDate = new OptionalDateInputViewModel(true);
            ConsentValidToDate = new OptionalDateInputViewModel(true);
            ConsentGivenDate = new OptionalDateInputViewModel(true);
            DecisionTypes = new List<DecisionType>();
            ObjectionDate = new OptionalDateInputViewModel(true);
            ConsentWithdrawnDate = new OptionalDateInputViewModel(true);
            WithdrawnDate = new OptionalDateInputViewModel(true);
            NotificationReceivedDate = new OptionalDateInputViewModel(true);
        }

        public DecisionViewModel(ImportNotificationAssessmentDecisionData data) : this()
        {
            DecisionTypes = data.AvailableDecisions;
            Status = data.Status;
            NotificationReceivedDate = new OptionalDateInputViewModel(data.NotificationReceivedDate, true);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            switch (Decision)
            {
                case DecisionType.Consent:
                    return ValidateConsent();
                case DecisionType.ConsentWithdraw:
                    return ValidateConsentWithdrawn();
                case DecisionType.Object:
                    return ValidateObject();
                case DecisionType.Withdraw:
                    return ValidateWithdrawn();
                default:
                    return new ValidationResult[0];
            }
        }

        private IEnumerable<ValidationResult> ValidateConsent()
        {
            if (!ConsentValidFromDate.IsCompleted)
            {
                yield return new ValidationResult(DecisionViewModelResources.ConsentValidFromRequired,
                    new[] { "ConsentValidFromDate" });
            }

            if (!ConsentValidToDate.IsCompleted)
            {
                yield return new ValidationResult(DecisionViewModelResources.ConsentValidToRequired,
                    new[] { "ConsentValidToDate" });
            }

            if (ConsentValidFromDate.IsCompleted && ConsentValidFromDate.IsCompleted
                && ConsentValidFromDate.AsDateTime() > ConsentValidToDate.AsDateTime())
            {
                yield return new ValidationResult(DecisionViewModelResources.ConsentValidFromBeforeTo,
                    new[] { "ConsentValidFromDate" });
            }

            if (!ConsentGivenDate.IsCompleted)
            {
                yield return new ValidationResult(DecisionViewModelResources.ConsentGivenRequired,
                    new[] { "ConsentGivenDate" });
            }
        }

        private IEnumerable<ValidationResult> ValidateConsentWithdrawn()
        {
            if (string.IsNullOrWhiteSpace(ReasonsForConsentWithdrawal))
            {
                yield return new ValidationResult(DecisionViewModelResources.ReasonConsentWithdrawnRequired,
                    new[] { "ReasonsForConsentWithdrawal" });
            }

            if (!ConsentWithdrawnDate.IsCompleted)
            {
                yield return new ValidationResult(DecisionViewModelResources.ConsentWithdrawnDateRequired, new[] { "ConsentWithdrawnDate" });
            }
        }

        private IEnumerable<ValidationResult> ValidateObject()
        {
            if (string.IsNullOrWhiteSpace(ReasonForObjection))
            {
                yield return new ValidationResult(DecisionViewModelResources.ReasonObjectedRequired,
                    new[] { "ReasonForObjection" });
            }

            if (!ObjectionDate.IsCompleted)
            {
                yield return new ValidationResult(DecisionViewModelResources.ObjectedDateRequired, new[] { "ObjectionDate" });
            }
        }

        private IEnumerable<ValidationResult> ValidateWithdrawn()
        {
            if (string.IsNullOrWhiteSpace(ReasonForWithdrawal))
            {
                yield return new ValidationResult(DecisionViewModelResources.ReasonWithdrawnRequired, new[] { "ReasonForWithdrawal" });
            }

            if (!WithdrawnDate.IsCompleted)
            {
                yield return new ValidationResult(DecisionViewModelResources.WithdrawnDateRequired, new[] { "WithdrawnDate" });
            }

            if (WithdrawnDate.AsDateTime() > SystemTime.UtcNow.Date)
            {
                yield return new ValidationResult(DecisionViewModelResources.WithdrawnDateNotFuture, new[] { "WithdrawnDate" });
            }

            if (WithdrawnDate.AsDateTime() < NotificationReceivedDate.AsDateTime())
            {
                yield return new ValidationResult(DecisionViewModelResources.WithdrawnDateNotBeforeReceived, new[] { "WithdrawnDate" });
            }
        } 
    }
}