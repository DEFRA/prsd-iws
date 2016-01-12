namespace EA.Iws.Web.Areas.ExportMovement.ViewModels.Complete
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using Core.Shared;
    using Infrastructure;
    using Infrastructure.Validation;

    public class UploadViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        public NotificationType NotificationType { get; set; }

        [Display(Name = "Upload the signed copy")]
        [RestrictToAllowedUploadTypes]
        public HttpPostedFileBase File { get; set; }

        public DateTime CompletedDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (File == null || File.InputStream.Length == 0)
            {
                yield return new ValidationResult("Please select a file", new[] { "File" });
            }
        }
    }
}