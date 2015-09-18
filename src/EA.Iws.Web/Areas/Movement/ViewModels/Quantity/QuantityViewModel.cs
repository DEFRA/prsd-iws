namespace EA.Iws.Web.Areas.Movement.ViewModels.Quantity
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Shared;
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

            decimal quantity;
            if (decimal.TryParse(Quantity, out quantity))
            {
                if (quantity <= 0)
                {
                    yield return new ValidationResult("The Actual quantity field must be a positive value", new[] { "Quantity" });
                }

                if (Units.HasValue && !quantity.IsDecimalValidToNDecimalPlaces(4))
                {
                    yield return new ValidationResult("Please enter a valid positive number with a maximum of 4 decimal places",
                        new[] { "Quantity" });
                }
            }
        }
    }
}