namespace EA.Iws.Web.Areas.Movement.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class SubmitViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        public Guid MovementId { get; set; }

        [Display(Name = "Upload the signed copy of the pre-notification document")]
        public HttpPostedFileBase File { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (File == null || File.InputStream.Length == 0)
            {
                yield return new ValidationResult("Please upload the signed copy of the pre-notification document", new[] { "File" });
            }
        }
    }
}