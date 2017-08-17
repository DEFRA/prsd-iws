namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.ReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.Movement;
    using Core.Shared;
    public class QuantityAbnormalViewModel : IValidatableObject
    {
        public QuantityReceivedTolerance Tolerance { get; set; }

        [Display(Name = "Quantity", ResourceType = typeof(QuantityAbnormalViewModelResources))]
        [Required(ErrorMessageResourceName = "QuantityRequired", ErrorMessageResourceType = typeof(QuantityAbnormalViewModelResources))]
        public decimal? Quantity { get; set; }

        [Display(Name = "Unit", ResourceType = typeof(QuantityAbnormalViewModelResources))]
        public ShipmentQuantityUnits Unit { get; set; }

        [Display(Name = "IsCorrect", ResourceType = typeof(QuantityAbnormalViewModelResources))]
        public bool? IsCorrect { get; set; }

        public DateTime DateReceived { get; set; }

        public DateTime? DateRecovered { get; set; }

        public NotificationType NotificationType { get; set; }

        public Guid NotificationId { get; set; }

        public CertificateType Certificate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!IsCorrect.HasValue)
            {
                yield return new ValidationResult(QuantityAbnormalViewModelResources.IsCorrectRequired, new[] { "IsCorrect" });
            }

            if (!Quantity.HasValue)
            {
                yield return new ValidationResult(QuantityAbnormalViewModelResources.QuantityRequired, new[] { "Quantity" });
            }

            if (Quantity < 0)
            {
                yield return new ValidationResult(QuantityAbnormalViewModelResources.QuantityNonNegative, new[] { "Quantity" });
            }

            if (Quantity.HasValue && decimal.Round(Quantity.Value, ShipmentQuantityUnitsMetadata.Precision[Unit]) != Quantity.Value)
            {
                yield return new ValidationResult(string.Format(QuantityAbnormalViewModelResources.QuantityPrecision, ShipmentQuantityUnitsMetadata.Precision[Unit]),
                    new[] { "Quantity" });
            }
        }
    }
}