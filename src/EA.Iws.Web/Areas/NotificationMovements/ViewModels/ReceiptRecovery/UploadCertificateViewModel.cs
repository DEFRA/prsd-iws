namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.ReceiptRecovery
{
    using Core.Movement;
    using Core.Shared;
    using Infrastructure.Validation;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class UploadCertificateViewModel : IValidatableObject
    {
        public UploadCertificateViewModel()
        {
        }

        public Guid NotificationId { get; set; }

        public IEnumerable<Guid> MovementIds { get; set; }

        public NotificationType NotificationType { get; set; }

        public CertificateType Certificate { get; set; }

        public string ShipmentNumbers { get; set; }

        public ShipmentQuantityUnits? Unit { get; set; }

        public DateTime DateReceived { get; set; }

        public decimal? Quantity { get; set; }

        public DateTime DateRecovered { get; set; }

        [RestrictToAllowedUploadTypes]
        public HttpPostedFileBase File { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (File == null || File.InputStream.Length == 0)
            {
                yield return new ValidationResult("Please upload the signed copy of the certificate", new[] { "File" });
            }
        }
    }
}