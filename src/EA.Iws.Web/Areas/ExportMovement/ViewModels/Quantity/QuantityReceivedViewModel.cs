namespace EA.Iws.Web.Areas.ExportMovement.ViewModels.Quantity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.Shared;

    public class QuantityReceivedViewModel : IValidatableObject
    {
        public ShipmentQuantityUnits Unit { get; set; }

        public DateTime DateReceived { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public decimal? Quantity { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Quantity.HasValue)
            {
                yield return new ValidationResult(QuantityReceivedViewModelResources.QuantityReceivedRequired, new[] { "Quantity" });
            }

            if (Quantity <= 0)
            {
                yield return new ValidationResult(QuantityReceivedViewModelResources.QuantityReceivedPositive, new[] { "Quantity" });
            }

            if (Quantity.HasValue && decimal.Round(Quantity.Value, ShipmentQuantityUnitsMetadata.Precision[Unit]) != Quantity.Value)
            {
                yield return new ValidationResult(
                    string.Format(QuantityReceivedViewModelResources.QuantityReceivedPrecision, ShipmentQuantityUnitsMetadata.Precision[Unit] + 1),
                    new[] { "Quantity" });
            }
        }
    }
}