namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.AssignArea
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Web.ViewModels.Shared;

    public class AssignAreaViewModel
    {
        [Required(ErrorMessage = "Please select a local area")]
        [Display(Name = "Local area covered")]
        public Guid? LocalAreaId { get; set; }

        [Display(Name = "Area consultation received date")]
        public OptionalDateInputViewModel ReceivedDate { get; set; }

        public SelectList Areas { get; set; }

        public Guid NotificationId { get; set; }

        public AssignAreaViewModel() : this(null)
        {
        }

        public AssignAreaViewModel(DateTime? receivedDate)
        {
            ReceivedDate = new OptionalDateInputViewModel(receivedDate, true);
        }
    }
}