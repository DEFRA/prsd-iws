namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.KeyDatesOverride
{
    using System.ComponentModel.DataAnnotations;
    using Core.Admin.KeyDates;
    using Web.ViewModels.Shared;

    public class IndexViewModel
    {
        public IndexViewModel()
        {
            NotificationReceivedDate = new DateInputViewmodel(true, false);
            CommencementDate = new DateInputViewmodel(true, false);
            CompleteDate = new DateInputViewmodel(true, false);
            TransmittedDate = new DateInputViewmodel(true, false);
            AcknowledgedDate = new DateInputViewmodel(true, false);
            DecisionRequiredByDate = new DateInputViewmodel(true, false);
            WithdrawnDate = new DateInputViewmodel(true, false);
            ObjectedDate = new DateInputViewmodel(true, false);
            ConsentedDate = new DateInputViewmodel(true, false);
            ConsentValidFromDate = new DateInputViewmodel(true, false);
            ConsentValidToDate = new DateInputViewmodel(true, false);
        }

        public IndexViewModel(KeyDatesOverrideData data)
        {
            NotificationReceivedDate = new DateInputViewmodel(data.NotificationReceivedDate, true, false);
            CommencementDate = new DateInputViewmodel(data.CommencementDate, true, false);
            CompleteDate = new DateInputViewmodel(data.CompleteDate, true, false);
            TransmittedDate = new DateInputViewmodel(data.TransmittedDate, true, false);
            AcknowledgedDate = new DateInputViewmodel(data.AcknowledgedDate, true, false);
            DecisionRequiredByDate = new DateInputViewmodel(data.DecisionRequiredByDate, true, false);
            WithdrawnDate = new DateInputViewmodel(data.WithdrawnDate, true, false);
            ObjectedDate = new DateInputViewmodel(data.ObjectedDate, true, false);
            ConsentedDate = new DateInputViewmodel(data.ConsentedDate, true, false);
            ConsentValidFromDate = new DateInputViewmodel(data.ConsentValidFromDate, true, false);
            ConsentValidToDate = new DateInputViewmodel(data.ConsentValidToDate, true, false);
        }

        [Display(Name = "NotificationReceivedDate", ResourceType = typeof(IndexViewModelResources))]
        public DateInputViewmodel NotificationReceivedDate { get; set; }

        [Display(Name = "CommencementDate", ResourceType = typeof(IndexViewModelResources))]
        public DateInputViewmodel CommencementDate { get; set; }

        [Display(Name = "CompleteDate", ResourceType = typeof(IndexViewModelResources))]
        public DateInputViewmodel CompleteDate { get; set; }

        [Display(Name = "TransmittedDate", ResourceType = typeof(IndexViewModelResources))]
        public DateInputViewmodel TransmittedDate { get; set; }

        [Display(Name = "AcknowledgedDate", ResourceType = typeof(IndexViewModelResources))]
        public DateInputViewmodel AcknowledgedDate { get; set; }

        [Display(Name = "DecisionRequiredByDate", ResourceType = typeof(IndexViewModelResources))]
        public DateInputViewmodel DecisionRequiredByDate { get; set; }

        [Display(Name = "WithdrawnDate", ResourceType = typeof(IndexViewModelResources))]
        public DateInputViewmodel WithdrawnDate { get; set; }

        [Display(Name = "ObjectedDate", ResourceType = typeof(IndexViewModelResources))]
        public DateInputViewmodel ObjectedDate { get; set; }

        [Display(Name = "ConsentedDate", ResourceType = typeof(IndexViewModelResources))]
        public DateInputViewmodel ConsentedDate { get; set; }

        [Display(Name = "ConsentValidFromDate", ResourceType = typeof(IndexViewModelResources))]
        public DateInputViewmodel ConsentValidFromDate { get; set; }

        [Display(Name = "ConsentValidToDate", ResourceType = typeof(IndexViewModelResources))]
        public DateInputViewmodel ConsentValidToDate { get; set; }
    }
}