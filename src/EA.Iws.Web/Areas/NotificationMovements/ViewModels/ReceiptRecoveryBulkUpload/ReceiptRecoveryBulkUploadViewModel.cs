﻿namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.ReceiptRecoveryBulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Movement.BulkUpload;
    using Core.Shared;

    public class ReceiptRecoveryBulkUploadViewModel : IValidatableObject
    {
        public ReceiptRecoveryBulkUploadViewModel()
        {
        }

        public ReceiptRecoveryBulkUploadViewModel(Guid notificationId, NotificationType type)
        {
            this.NotificationId = notificationId;
            this.NotificationType = type;
        }

        public Guid NotificationId { get; set; }
        public NotificationType NotificationType { get; set; }

        public int ErrorsCount
        {
            get
            {
                var failedFileRulesCount = FailedFileRules != null ? FailedFileRules.Count : 0;
                var failedContentRulesCount = FailedContentRules != null ? FailedContentRules.Count : 0;
                return failedFileRulesCount + failedContentRulesCount;
            }
        }

        public List<BulkFileRules> FailedFileRules { get; set; }

        public List<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>> FailedContentRules { get; set; }

        [Display(Name = "Upload the data file containing your receipt and/or recovery data")]
        public HttpPostedFileBase File { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (File == null || File.InputStream.Length == 0)
            {
                yield return new ValidationResult("Upload the data file containing your receipt and/or recovery data", new[] { "File" });
            }
        }
    }
}