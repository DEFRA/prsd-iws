namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.ReceiptRecoveryBulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using Core.Movement.BulkReceiptRecovery;

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

        public int ErrorsCount
        {
            get
            {
                var failedFileRulesCount = FailedFileRules != null ? FailedFileRules.Count : 0;
                var failedContentRulesCount = FailedContentRules != null ? FailedContentRules.Count : 0;
                return failedFileRulesCount + failedContentRulesCount;
            }
        }

        public List<ReceiptRecoveryFileRules> FailedFileRules { get; set; }

        public List<ContentRuleResult<ReceiptRecoveryContentRules>> WarningContentRules { get; set; }

        public List<ContentRuleResult<ReceiptRecoveryContentRules>> FailedContentRules { get; set; }

        public int WarningsCount
        {
            get
            {
                return WarningContentRules != null ? WarningContentRules.Count : 0;
            }
        }

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