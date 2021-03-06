﻿namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.PrenotificationBulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using Core.Movement.BulkPrenotification;
    using Core.Movement.BulkUpload;

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

        public List<PrenotificationContentRuleResult<PrenotificationContentRules>> FailedContentRules { get; set; }

        [Display(Name = "Upload the data file containing your prenotification data")]
        public HttpPostedFileBase File { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (File == null || File.InputStream.Length == 0)
            {
                yield return new ValidationResult("Upload the data file containing your prenotification data", new[] { "File" });
            }
        }
    }
}