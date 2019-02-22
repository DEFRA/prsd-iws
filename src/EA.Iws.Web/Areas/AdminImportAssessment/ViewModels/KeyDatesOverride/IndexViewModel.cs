namespace EA.Iws.Web.Areas.AdminImportAssessment.ViewModels.KeyDatesOverride
{
    using System.ComponentModel.DataAnnotations;
    using Core.Admin.KeyDates;
    using Web.ViewModels.Shared;

    public class IndexViewModel
    {
        public IndexViewModel()
        {
            NotificationReceivedDate = new OptionalDateInputViewModel(allowPastDates: true, showLabels: false);
            CommencementDate = new OptionalDateInputViewModel(allowPastDates: true, showLabels: false);
            CompleteDate = new OptionalDateInputViewModel(allowPastDates: true, showLabels: false);
            AcknowledgedDate = new OptionalDateInputViewModel(allowPastDates: true, showLabels: false);
            DecisionRequiredByDate = new OptionalDateInputViewModel(allowPastDates: true, showLabels: false);
            WithdrawnDate = new OptionalDateInputViewModel(allowPastDates: true, showLabels: false);
            ObjectedDate = new OptionalDateInputViewModel(allowPastDates: true, showLabels: false);
            ConsentedDate = new OptionalDateInputViewModel(allowPastDates: true, showLabels: false);
            ConsentValidFromDate = new OptionalDateInputViewModel(allowPastDates: true, showLabels: false);
            ConsentValidToDate = new OptionalDateInputViewModel(allowPastDates: true, showLabels: false);
        }

        public IndexViewModel(KeyDatesOverrideData data)
        {
            NotificationReceivedDate = new OptionalDateInputViewModel(data.NotificationReceivedDate, allowPastDates: true, showLabels: false);
            CommencementDate = new OptionalDateInputViewModel(data.CommencementDate, allowPastDates: true, showLabels: false);
            CompleteDate = new OptionalDateInputViewModel(data.CompleteDate, allowPastDates: true, showLabels: false);
            AcknowledgedDate = new OptionalDateInputViewModel(data.AcknowledgedDate, allowPastDates: true, showLabels: false);
            DecisionRequiredByDate = new OptionalDateInputViewModel(data.DecisionRequiredByDate, allowPastDates: true, showLabels: false);
            WithdrawnDate = new OptionalDateInputViewModel(data.WithdrawnDate, allowPastDates: true, showLabels: false);
            ObjectedDate = new OptionalDateInputViewModel(data.ObjectedDate, allowPastDates: true, showLabels: false);
            ConsentedDate = new OptionalDateInputViewModel(data.ConsentedDate, allowPastDates: true, showLabels: false);
            ConsentValidFromDate = new OptionalDateInputViewModel(data.ConsentValidFromDate, allowPastDates: true, showLabels: false);
            ConsentValidToDate = new OptionalDateInputViewModel(data.ConsentValidToDate, allowPastDates: true, showLabels: false);
        }

        [Display(Name = "NotificationReceivedDate", ResourceType = typeof(IndexViewModelResources))]
        public OptionalDateInputViewModel NotificationReceivedDate { get; set; }

        [Display(Name = "CommencementDate", ResourceType = typeof(IndexViewModelResources))]
        public OptionalDateInputViewModel CommencementDate { get; set; }

        [Display(Name = "CompleteDate", ResourceType = typeof(IndexViewModelResources))]
        public OptionalDateInputViewModel CompleteDate { get; set; }

        [Display(Name = "AcknowledgedDate", ResourceType = typeof(IndexViewModelResources))]
        public OptionalDateInputViewModel AcknowledgedDate { get; set; }

        [Display(Name = "DecisionRequiredByDate", ResourceType = typeof(IndexViewModelResources))]
        public OptionalDateInputViewModel DecisionRequiredByDate { get; set; }

        [Display(Name = "WithdrawnDate", ResourceType = typeof(IndexViewModelResources))]
        public OptionalDateInputViewModel WithdrawnDate { get; set; }

        [Display(Name = "ObjectedDate", ResourceType = typeof(IndexViewModelResources))]
        public OptionalDateInputViewModel ObjectedDate { get; set; }

        [Display(Name = "ConsentedDate", ResourceType = typeof(IndexViewModelResources))]
        public OptionalDateInputViewModel ConsentedDate { get; set; }

        [Display(Name = "ConsentValidFromDate", ResourceType = typeof(IndexViewModelResources))]
        public OptionalDateInputViewModel ConsentValidFromDate { get; set; }

        [Display(Name = "ConsentValidToDate", ResourceType = typeof(IndexViewModelResources))]
        public OptionalDateInputViewModel ConsentValidToDate { get; set; }
    }
}