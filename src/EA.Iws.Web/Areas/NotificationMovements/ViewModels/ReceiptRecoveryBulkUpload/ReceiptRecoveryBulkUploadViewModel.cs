namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.PrenotificationBulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}