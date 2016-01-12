namespace EA.Iws.Web.Areas.ExportMovement.ViewModels.Submit
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using Infrastructure;
    using Infrastructure.Validation;

    public class SubmitViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        public Guid MovementId { get; set; }

        [Display(Name = "File", ResourceType = typeof(SubmitViewModelResources))]
        [RestrictToAllowedUploadTypes]
        public HttpPostedFileBase File { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (File == null || File.InputStream.Length == 0)
            {
                yield return new ValidationResult(SubmitViewModelResources.FileRequired, new[] { "File" });
            }
        }
    }
}