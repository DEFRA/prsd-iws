namespace EA.Iws.Web.Areas.Movement.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using Core.Shared;

    public class OperationCompleteViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }
        public NotificationType NotificationType { get; set; }

        [Display(Name = "Upload the signed copy")]
        public HttpPostedFileBase File { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (File == null || File.InputStream.Length == 0)
            {
                yield return new ValidationResult("Please select a file", new[] { "File" });
            }
        }
    }
}