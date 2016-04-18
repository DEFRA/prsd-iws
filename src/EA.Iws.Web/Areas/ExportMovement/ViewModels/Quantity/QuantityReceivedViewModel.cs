namespace EA.Iws.Web.Areas.ExportMovement.ViewModels.Quantity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using Core.Shared;
    using Infrastructure.Validation;

    public class QuantityReceivedViewModel : IValidatableObject
    {
        public ShipmentQuantityUnits Unit { get; set; }

        public DateTime DateReceived { get; set; }

        [Display(Name = "Quantity", ResourceType = typeof(QuantityReceivedViewModelResources))]
        [Required(ErrorMessageResourceName = "QuantityReceivedRequired", ErrorMessageResourceType = typeof(QuantityReceivedViewModelResources))]
        [IsValidNumber(maxPrecision: 18, NumberStyle = NumberStyles.AllowDecimalPoint, ErrorMessageResourceName = "QuantityIsValid", ErrorMessageResourceType = typeof(QuantityReceivedViewModelResources))]
        public string Quantity { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            decimal quantity = Convert.ToDecimal(Quantity);

            if (quantity <= 0)
            {
                yield return new ValidationResult(QuantityReceivedViewModelResources.QuantityReceivedPositive, new[] { "Quantity" });
            }

            if (decimal.Round(quantity, ShipmentQuantityUnitsMetadata.Precision[Unit]) != quantity)
            {
                yield return new ValidationResult(
                    string.Format(QuantityReceivedViewModelResources.QuantityReceivedPrecision, ShipmentQuantityUnitsMetadata.Precision[Unit] + 1),
                    new[] { "Quantity" });
            }
        }
    }
}