namespace EA.Iws.Web.Areas.Movement.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class ReceiptCompleteViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        [Display(Name = "Upload the signed copy of the certificate of receipt")]
        public HttpPostedFileBase File { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (File == null || File.InputStream.Length == 0)
            {
                yield return new ValidationResult("Please upload the signed copy of the certificate of receipt", new[] { "File" });
            }
        }
    }
}