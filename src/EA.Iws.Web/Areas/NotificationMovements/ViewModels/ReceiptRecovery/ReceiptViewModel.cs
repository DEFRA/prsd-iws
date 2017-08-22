namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.ReceiptRecovery
{
    using Core.Movement;
    using Core.Shared;
    using Infrastructure.Validation;
    using Prsd.Core;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;

    public class ReceiptViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        public Guid SelectedmovementId { get; set; }

        public CertificateType Certificate { get; set; }

        public NotificationType? NotificationType { get; set; }

        [Required(ErrorMessageResourceName = "InvalidReceiptDay", ErrorMessageResourceType = typeof(ReceiptRecoveryViewModelResources))]
        [Range(1, 31, ErrorMessageResourceName = "InvalidReceiptDay", ErrorMessageResourceType = typeof(ReceiptRecoveryViewModelResources))]
        public int? Day { get; set; }

        [Required(ErrorMessageResourceName = "InvalidMonth", ErrorMessageResourceType = typeof(ReceiptRecoveryViewModelResources))]
        [Range(1, 12, ErrorMessageResourceName = "InvalidMonth", ErrorMessageResourceType = typeof(ReceiptRecoveryViewModelResources))]
        public int? Month { get; set; }

        [Required(ErrorMessageResourceName = "InvalidYear", ErrorMessageResourceType = typeof(ReceiptRecoveryViewModelResources))]
        [Range(2015, 3000, ErrorMessageResourceName = "InvalidYear", ErrorMessageResourceType = typeof(ReceiptRecoveryViewModelResources))]
        public int? Year { get; set; }

        [Display(Name = "Quantity", ResourceType = typeof(ReceiptRecoveryViewModelResources))]
        [Required(ErrorMessageResourceName = "QuantityReceivedRequired", ErrorMessageResourceType = typeof(ReceiptRecoveryViewModelResources))]
        [IsValidNumber(maxPrecision: 18, NumberStyle = NumberStyles.AllowDecimalPoint, ErrorMessageResourceName = "QuantityIsValid", ErrorMessageResourceType = typeof(ReceiptRecoveryViewModelResources))]
        public string Quantity { get; set; }
        public ShipmentQuantityUnits Unit { get; set; }
      
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            decimal quantity = Convert.ToDecimal(Quantity);

            if (quantity <= 0)
            {
                yield return new ValidationResult(ReceiptRecoveryViewModelResources.QuantityReceivedPositive, new[] { "Quantity" });
            }

            if (decimal.Round(quantity, ShipmentQuantityUnitsMetadata.Precision[Unit]) != quantity)
            {
                yield return new ValidationResult(
                    string.Format(ReceiptRecoveryViewModelResources.QuantityReceivedPrecision, ShipmentQuantityUnitsMetadata.Precision[Unit] + 1),
                    new[] { "Receipt.Quantity" });
            }

            DateTime dateReceived;
            if (!ParseDateInput(out dateReceived))
            {
                yield return new ValidationResult("Please enter a valid date", new[] { "Day" });
            }
            else if (dateReceived.Date > SystemTime.UtcNow.Date)
            {
                yield return new ValidationResult("This date cannot be in the future. Please enter a different date.", new[] { "Day" });
            }                
        }

        public DateTime GetDateReceived()
        {
            DateTime dateReceived;
            if (ParseDateInput(out dateReceived))
            {
                return dateReceived;
            }
            else
            {
                throw new InvalidOperationException("Date not valid");
            }
        }

        private bool ParseDateInput(out DateTime dateReceived)
        {
            return SystemTime.TryParse(
                Year.GetValueOrDefault(),
                Month.GetValueOrDefault(),
                Day.GetValueOrDefault(),
                out dateReceived);
        }
    }
}