namespace EA.Iws.Web.Areas.Admin.ViewModels.FinancialGuarantee
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class FinancialGuaranteeInformationViewModel
    {
        [Required]
        [DisplayName("Guarantee received")]
        public OptionalDateInputViewModel Received { get; set; }

        [Required]
        [DisplayName("Guarantee complete")]
        public OptionalDateInputViewModel Completed { get; set; }

        [Required]
        [DisplayName("Decision required by")]
        public OptionalDateInputViewModel DecisionRequired { get; set; }

        public string Status { get; set; }
    }
}