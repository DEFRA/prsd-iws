namespace EA.Iws.Web.Areas.Movement.ViewModels.Quantity
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Shared;
    using Prsd.Core.Helpers;
    using Services;

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
        public string Quantity { get; set; }

        public ShipmentQuantityUnits? Units { get; set; }

        public ShipmentQuantityUnits NotificationUnits { get; set; }

        public string UnitsDisplay
        {
            get { return EnumHelper.GetDisplayName(Units); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Quantity))
            {
                yield return new ValidationResult("The Actual quantity field is required", new[] { "Quantity" });
            }

            if (!string.IsNullOrWhiteSpace(Quantity) && !ViewModelService.IsStringValidDecimalToNDecimalPlaces(Quantity, 4))
            {
                yield return new ValidationResult("Please enter a valid positive number with a maximum of 4 decimal places", new[] { "Quantity" });
            }

            if (Units.HasValue && Units != NotificationUnits)
            {
                if (Units.Value == ShipmentQuantityUnits.Kilograms
                    || Units.Value == ShipmentQuantityUnits.Litres)
                {
                    var hasTooManyDecimalPlaces = !ViewModelService.IsStringValidDecimalToNDecimalPlaces(Quantity, 1);

                    if (hasTooManyDecimalPlaces)
                    {
                        yield return new ValidationResult("The quantity expressed in Kilograms or Litres can only have a maximum of 1 decimal place", new[] { "Quantity" });
                    }
                }
            }
        }
    }
}