namespace EA.Iws.Web.Areas.Movement.ViewModels.Quantity
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.Shared;
    using Services;

    public class QuantityReceivedViewModel : IValidatableObject
    {
        public ShipmentQuantityUnits MovementUnits { get; set; }

        public ShipmentQuantityUnits NotificationUnits { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public decimal? Quantity { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Quantity.HasValue)
            {
                yield return new ValidationResult("The quantity field is required", new[] { "Quantity" });
            }

            if (Quantity <= 0)
            {
                yield return new ValidationResult("The quantity field must be a positive value", new[] { "Quantity" });
            }
            
            bool hasTooManyDecimalPlaces;
            int i;

            if (MovementUnits != NotificationUnits
                && (MovementUnits == ShipmentQuantityUnits.Kilograms
                    || MovementUnits == ShipmentQuantityUnits.Litres))
            {
                i = 1;
                hasTooManyDecimalPlaces = !ViewModelService.IsDecimalValidToNDecimalPlaces(Quantity, 1);
            }
            else
            {
                i = 4;
                hasTooManyDecimalPlaces = !ViewModelService.IsDecimalValidToNDecimalPlaces(Quantity, 4);
            }

            if (hasTooManyDecimalPlaces)
            {
                yield return new ValidationResult("The quantity field must be a decimal to " + i + " decimal places", new[] { "Quantity" });
            }
        }
    }
}