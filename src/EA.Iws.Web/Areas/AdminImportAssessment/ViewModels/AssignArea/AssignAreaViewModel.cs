namespace EA.Iws.Web.Areas.AdminImportAssessment.ViewModels.AssignArea
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class AssignAreaViewModel
    {
        [Required(ErrorMessage = "Please select a local area")]
        [Display(Name = "Local area covered")]
        public Guid? LocalAreaId { get; set; }

        public SelectList Areas { get; set; }

        public Guid NotificationId { get; set; }
    }
}