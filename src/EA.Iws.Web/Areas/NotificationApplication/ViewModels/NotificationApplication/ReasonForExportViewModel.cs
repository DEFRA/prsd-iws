namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ReasonForExportViewModel
    {
        [Required(ErrorMessage = "Please enter a reason for export")]
        [StringLength(70, ErrorMessage = "Reason for export cannot be longer than 70 characters")]
        public string ReasonForExport { get; set; }

        public Guid NotificationId { get; set; }
    }
}