namespace EA.Iws.Web.Areas.AdminImportAssessment.ViewModels.FinancialGuarantee
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Infrastructure.Validation;
    using Web.ViewModels.Shared;

    public class ReceivedDateViewModel
    {
        [Display(Name = "ReceivedDateLabel", ResourceType = typeof(ReceivedDateViewModelResources))]
        [RequiredDateInput(ErrorMessageResourceName = "ReceivedDateRequired", ErrorMessageResourceType = typeof(ReceivedDateViewModelResources))]
        public OptionalDateInputViewModel ReceivedDate { get; set; }

        public ReceivedDateViewModel() : this(null)
        {
        }

        public ReceivedDateViewModel(DateTime? receivedDate)
        {
            ReceivedDate = new OptionalDateInputViewModel(receivedDate, true);
        }
    }
}