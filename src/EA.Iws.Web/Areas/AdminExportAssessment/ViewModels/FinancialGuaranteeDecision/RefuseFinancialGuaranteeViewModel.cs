namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.FinancialGuaranteeDecision
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.FinancialGuarantee;
    using Prsd.Core;
    using Web.ViewModels.Shared;

    public class RefuseFinancialGuaranteeViewModel : IValidatableObject
    {
        public RefuseFinancialGuaranteeViewModel()
        {
            DecisionMadeDate = new OptionalDateInputViewModel();
        }

        public RefuseFinancialGuaranteeViewModel(FinancialGuaranteeData financialGuarantee)
        {
            DecisionMadeDate = new OptionalDateInputViewModel();
            CompletedDate = financialGuarantee.CompletedDate.Value;
        }

        public Guid NotificationId { get; set; }

        public Guid FinancialGuaranteeId { get; set; }

        public DateTime CompletedDate { get; set; }

        [Display(ResourceType = typeof(FinancialGuaranteeDecisionResources), Name = "DecisionMadeDate")]
        public OptionalDateInputViewModel DecisionMadeDate { get; set; }

        [MaxLength(2048)]
        [Display(ResourceType = typeof(FinancialGuaranteeDecisionResources), Name = "ReasonForRefusal")]
        public string ReasonForRefusal { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DecisionMadeDate.IsCompleted && DecisionMadeDate.AsDateTime() < CompletedDate)
            {
                yield return
                    new ValidationResult(string.Format(FinancialGuaranteeDecisionResources.DecisionMadeDateNotBeforeCompleteDate,
                        CompletedDate.ToShortDateString()),
                        new[] { "DecisionMadeDate.Day" });
            }

            if (!DecisionMadeDate.IsCompleted)
            {
                yield return
                    new ValidationResult(FinancialGuaranteeDecisionResources.DecisionMadeDateRequired, new[] { "DecisionMadeDate.Day" });
            }

            if (DecisionMadeDate.AsDateTime() > SystemTime.UtcNow)
            {
                yield return new ValidationResult(FinancialGuaranteeDecisionResources.DecisionMadeDateNotInFuture, new[] { "DecisionMadeDate.Day" });
            }

            if (string.IsNullOrWhiteSpace(ReasonForRefusal))
            {
                yield return new ValidationResult(FinancialGuaranteeDecisionResources.ReasonForRefusalRequired, new[] { "ReasonForRefusal" });
            }
        }
    }
}