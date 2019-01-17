namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.PrenotificationBulkUpload
{
    using System;
    using System.Web;
    using Infrastructure.Validation;

    public class PrenotificationBulkUploadViewModel
    {
        public Guid NotificationId { get; set; }

        [RestrictToAllowedUploadTypes]
        public HttpPostedFileBase File { get; set; }
    }
}