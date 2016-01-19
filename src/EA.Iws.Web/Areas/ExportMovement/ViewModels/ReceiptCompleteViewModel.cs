namespace EA.Iws.Web.Areas.ExportMovement.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using Core.Shared;
    using Infrastructure.Validation;

    public class ReceiptCompleteViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        public ShipmentQuantityUnits? Unit { get; set; }

        public DateTime DateReceived { get; set; }

        public decimal? Quantity { get; set; }

        [Display(Name = "Upload the signed copy of the certificate of receipt")]
        [RestrictToAllowedUploadTypes]
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