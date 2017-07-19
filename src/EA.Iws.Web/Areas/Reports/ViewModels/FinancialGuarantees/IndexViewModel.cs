namespace EA.Iws.Web.Areas.Reports.ViewModels.FinancialGuarantees
{
    using System.ComponentModel.DataAnnotations;

    public class IndexViewModel
    {
        [Display(ResourceType = typeof(IndexViewModelResources), Name = "FinancialGuaranteeReferenceNumber")]
        public string FinancialGuaranteeReferenceNumber { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "ExporterName")]
        public string ExporterName { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "ImporterName")]
        public string ImporterName { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "ProducerName")]
        public string ProducerName { get; set; }
    }
}