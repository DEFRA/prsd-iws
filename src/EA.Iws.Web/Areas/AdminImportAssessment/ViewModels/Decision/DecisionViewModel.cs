namespace EA.Iws.Web.Areas.AdminImportAssessment.ViewModels.Decision
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Admin;
    using Core.ImportNotificationAssessment;
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
        
        public DecisionViewModel()
        {
            ConsentValidFromDate = new OptionalDateInputViewModel(true);
            ConsentValidToDate = new OptionalDateInputViewModel(true);
            ConsentGivenDate = new OptionalDateInputViewModel(true);
            DecisionTypes = new List<DecisionType>();
        }

        public DecisionViewModel(ImportNotificationAssessmentDecisionData data)
        {
            DecisionTypes = data.AvailableDecisions;
            Status = data.Status;
            ConsentValidFromDate = new OptionalDateInputViewModel(true);
            ConsentValidToDate = new OptionalDateInputViewModel(true);
            ConsentGivenDate = new OptionalDateInputViewModel(true);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            switch (Decision)
            {
                case DecisionType.Consent:
                    return ValidateConsent();
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
    }
}