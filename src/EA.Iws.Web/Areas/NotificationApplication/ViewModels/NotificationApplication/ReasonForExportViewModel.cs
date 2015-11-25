namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Views.ReasonForExport;

    public class ReasonForExportViewModel
    {
        [Required(ErrorMessageResourceName = "ReasonRequired", ErrorMessageResourceType = typeof(ReasonForExportResources))]
        [StringLength(70, ErrorMessageResourceName = "ReasonLengthLimit", ErrorMessageResourceType = typeof(ReasonForExportResources))]
        public string ReasonForExport { get; set; }

        public Guid NotificationId { get; set; }
    }
}