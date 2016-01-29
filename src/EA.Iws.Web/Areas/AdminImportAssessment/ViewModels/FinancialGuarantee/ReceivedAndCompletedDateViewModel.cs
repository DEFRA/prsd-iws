namespace EA.Iws.Web.Areas.AdminImportAssessment.ViewModels.FinancialGuarantee
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Core.ImportNotificationAssessment.FinancialGuarantee;
    using Infrastructure.Validation;
    using Web.ViewModels.Shared;

    public class ReceivedAndCompletedDateViewModel
    {
        [Display(Name = "ReceivedDateLabel", ResourceType = typeof(ReceivedAndCompletedDateViewModelResources))]
        [RequiredDateInput(ErrorMessageResourceName = "ReceivedDateRequired", ErrorMessageResourceType = typeof(ReceivedAndCompletedDateViewModelResources))]
        public OptionalDateInputViewModel ReceivedDate { get; set; }

        [Display(Name = "CompletedDateLabel", ResourceType = typeof(ReceivedAndCompletedDateViewModelResources))]
        [RequiredDateInput(ErrorMessageResourceName = "CompletedDateRequired", ErrorMessageResourceType = typeof(ReceivedAndCompletedDateViewModelResources))]
        public OptionalDateInputViewModel CompletedDate { get; set; }

        public DateTime OriginalReceivedDate { get; set; }

        public ReceivedAndCompletedDateViewModel() : this(null, null)
        {
        }

        public ReceivedAndCompletedDateViewModel(ReceivedAndCompletedDateData data) : this(data.ReceivedDate, data.CompletedDate)
        {
            OriginalReceivedDate = data.ReceivedDate.Value;
        }

        public ReceivedAndCompletedDateViewModel(DateTime? receivedDate, DateTime? completedDate)
        {
            ReceivedDate = new OptionalDateInputViewModel(receivedDate, true);
            CompletedDate = new OptionalDateInputViewModel(completedDate, true);
        }

        public bool IsReceivedDateChanged
        {
            get { return OriginalReceivedDate != ReceivedDate.AsDateTime().GetValueOrDefault(); }
        }
    }
}