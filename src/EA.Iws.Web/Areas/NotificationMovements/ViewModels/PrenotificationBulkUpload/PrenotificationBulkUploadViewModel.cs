namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.PrenotificationBulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using Infrastructure.Validation;

    public class PrenotificationBulkUploadViewModel : IValidatableObject
    {
        public PrenotificationBulkUploadViewModel()
        {
        }

        public PrenotificationBulkUploadViewModel(Guid notificationId)
        {
            this.NotificationId = notificationId;
        }

        public Guid NotificationId { get; set; }

        [Display(Name = "Upload the signed copy of the prenotification document")]
        [RestrictToAllowedUploadTypes]
        public HttpPostedFileBase File { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (File == null || File.InputStream.Length == 0)
            {
                yield return new ValidationResult("Please upload the signed copy of the prenotification document", new[] { "File" });
            }
        }
    }
}