namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.FinancialGuaranteeDates
{
    using System.ComponentModel.DataAnnotations;
    using Core.FinancialGuarantee;
    using Web.ViewModels.Shared;

    public class UpdateFinancialGuaranteeDatesViewModel
    {
        [Display(ResourceType = typeof(FinancialGuaranteeDatesResources), Name = "ReceivedDate")]
        public OptionalDateInputViewModel ReceivedDate { get; set; }

        [Display(ResourceType = typeof(FinancialGuaranteeDatesResources), Name = "CompletedDate")]
        public OptionalDateInputViewModel CompletedDate { get; set; }

        [Display(ResourceType = typeof(FinancialGuaranteeDatesResources), Name = "DecisionDate")]
        public OptionalDateInputViewModel DecisionDate { get; set; }

        public UpdateFinancialGuaranteeDatesViewModel()
        {
            ReceivedDate = new OptionalDateInputViewModel(allowPastDates: true, showLabels: false);
            CompletedDate = new OptionalDateInputViewModel(allowPastDates: true, showLabels: false);
            DecisionDate = new OptionalDateInputViewModel(allowPastDates: true, showLabels: false);
        }

        public UpdateFinancialGuaranteeDatesViewModel(FinancialGuaranteeData data)
        {
            ReceivedDate = new OptionalDateInputViewModel(data.ReceivedDate, allowPastDates: true, showLabels: false);
            CompletedDate = new OptionalDateInputViewModel(data.CompletedDate, allowPastDates: true, showLabels: false);
            DecisionDate = new OptionalDateInputViewModel(data.DecisionDate, allowPastDates: true, showLabels: false);
        }
    }
}