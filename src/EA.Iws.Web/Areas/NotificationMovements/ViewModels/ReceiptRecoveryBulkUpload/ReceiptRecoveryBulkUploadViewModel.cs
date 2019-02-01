namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.PrenotificationBulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class ReceiptRecoveryBulkUploadViewModel : IValidatableObject
    {
        public ReceiptRecoveryBulkUploadViewModel()
        {
        }

        public ReceiptRecoveryBulkUploadViewModel(Guid notificationId)
        {
            this.NotificationId = notificationId;
        }

        public Guid NotificationId { get; set; }

        [Display(Name = "Upload the data file containing your receipt / recovery data")]
        public HttpPostedFileBase File { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (File == null || File.InputStream.Length == 0)
            {
                yield return new ValidationResult("Upload the data file containing your receipt / recovery data", new[] { "File" });
            }
        }
    }
}