namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.FinancialGuaranteeAssessment
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.Shared;
    using Prsd.Core;
    using Web.ViewModels.Shared;

    public class NewFinancialGuaranteeViewModel : IValidatableObject
    {
        [Required]
        [Display(ResourceType = typeof(FinancialGuaranteeAssessmentResources), Name = "ReceivedDate")]
        public OptionalDateInputViewModel ReceivedDate { get; set; }

        public bool HasAlreadyFinancialGuarantee { get; set; }

        public AdditionalChargeData AdditionalCharge { get; set; }

        public UKCompetentAuthority CompetentAuthority { get; set; }

        public NewFinancialGuaranteeViewModel()
        {
            ReceivedDate = new OptionalDateInputViewModel(allowPastDates: true);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ReceivedDate.IsCompleted)
            {
                yield return new ValidationResult(FinancialGuaranteeAssessmentResources.ReceivedDateRequired, new[] { "ReceivedDate.Day" });
            }

            if (ReceivedDate.AsDateTime() > SystemTime.UtcNow)
            {
                yield return new ValidationResult(FinancialGuaranteeAssessmentResources.ReceivedDateNotInFuture, new[] { "ReceivedDate.Day" });
            }
        }
    }
}