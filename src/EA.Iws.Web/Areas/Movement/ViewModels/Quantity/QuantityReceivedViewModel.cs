namespace EA.Iws.Web.Areas.Movement.ViewModels.Quantity
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.Shared;

    public class QuantityReceivedViewModel : IValidatableObject
    {
        public ShipmentQuantityUnits Unit { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public decimal? Quantity { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Quantity.HasValue)
            {
                yield return new ValidationResult("The Actual quantity field is required", new[] { "Quantity" });
            }

            if (Quantity <= 0)
            {
                yield return new ValidationResult("The Actual quantity field must be a positive value", new[] { "Quantity" });
            }

            if (Quantity.HasValue && decimal.Round(Quantity.Value, ShipmentQuantityUnitsMetadata.Precision[Unit]) != Quantity.Value)
            {
                yield return new ValidationResult("Please enter a valid positive number with a maximum of " 
                    + ShipmentQuantityUnitsMetadata.Precision[Unit]
                    + " decimal places",
                    new[] { "Quantity" });
            }
        }
    }
}