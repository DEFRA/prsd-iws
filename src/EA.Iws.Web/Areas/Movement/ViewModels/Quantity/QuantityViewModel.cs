namespace EA.Iws.Web.Areas.Movement.ViewModels.Quantity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Shared;
    using Infrastructure.Validation;
    using Prsd.Core.Helpers;

    public class QuantityViewModel : IValidatableObject
    {
        public decimal TotalAvailable { get; set; }

        public decimal TotalUsed { get; set; }

        public decimal TotalNotified { get; set; }

        public SelectList UnitsSelectList
        {
            get
            {
                var units = AvailableUnits.Select(u => new
                {
                    Key = EnumHelper.GetDisplayName(u),
                    Value = (int)u
                });

                return new SelectList(units, "Value", "Key", Units);
            }
        }

        public IList<ShipmentQuantityUnits> AvailableUnits { get; set; }

        [Display(Name = "Actual quantity")]
        [IsValidNumber(maxPrecision: 18, ErrorMessage = "The actual quantity must be a valid number")]
        public string Quantity { get; set; }

        public ShipmentQuantityUnits? Units { get; set; }

        public ShipmentQuantityUnits NotificationUnits { get; set; }

        public string UnitsDisplay
        {
            get { return EnumHelper.GetDisplayName(Units); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            decimal quantity = Convert.ToDecimal(Quantity);

            if (quantity <= 0)
            {
                yield return new ValidationResult("The actual quantity must be a positive value", new[] { "Quantity" });
            }

            if (Units.HasValue && decimal.Round(quantity, ShipmentQuantityUnitsMetadata.Precision[Units.Value]) != quantity)
            {
                yield return new ValidationResult("Please enter a valid positive number with a maximum of "
                    + ShipmentQuantityUnitsMetadata.Precision[Units.Value]
                    + " decimal places",
                    new[] { "Quantity" });
            }
        }
    }
}