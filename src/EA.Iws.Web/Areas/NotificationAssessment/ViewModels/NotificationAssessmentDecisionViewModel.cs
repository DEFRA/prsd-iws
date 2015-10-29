namespace EA.Iws.Web.Areas.NotificationAssessment.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Core.Admin;
    using Core.NotificationAssessment;
    using Prsd.Core;
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
                return new SelectList(DecisionTypes);
            }
        }

        public IList<DecisionType> DecisionTypes { get; set; }

        [Required]
        [Display(Name = "Decision")]
        public DecisionType? SelectedDecision { get; set; }
        
        public OptionalDateInputViewModel ConsentValidFromDate { get; set; }

        public OptionalDateInputViewModel ConsentValidToDate { get; set; }

        public string ConsentConditions { get; set; }

        public string ReasonForObjection { get; set; }

        public NotificationAssessmentDecisionViewModel()
        {
            ConsentValidFromDate = new OptionalDateInputViewModel();
            ConsentValidToDate = new OptionalDateInputViewModel();
            PreviousDecisions = new List<DecisionRecordViewModel>();
            DecisionTypes = new List<DecisionType>();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SelectedDecision == DecisionType.Consent)
            {
                return ValidateConsent();
            }

            return new ValidationResult[0];
        }

        private IEnumerable<ValidationResult> ValidateConsent()
        {
            if (!ConsentValidFromDate.IsCompleted)
            {
                yield return new ValidationResult("The consent valid from date is required", 
                    new[] { "ConsentValidFromDate" });
            }

            if (!ConsentValidToDate.IsCompleted)
            {
                yield return new ValidationResult("The consent valid to date is required", 
                    new[] { "ConsentValidToDate" });
            }

            if (ConsentValidFromDate.IsCompleted && ConsentValidFromDate.IsCompleted 
                && ConsentValidFromDate.AsDateTime() > ConsentValidToDate.AsDateTime())
            {
                yield return new ValidationResult("The consent valid from date must be before the consent valid to date", 
                    new[] { "ConsentValidFromDate" });
            }

            if (ConsentValidFromDate.IsCompleted 
                && ConsentValidFromDate.AsDateTime().GetValueOrDefault().Date < SystemTime.UtcNow.Date)
            {
                yield return new ValidationResult("The consent valid from date must not be in the past", 
                    new[] { "ConsentValidFromDate" });
            }
        } 
    }
}