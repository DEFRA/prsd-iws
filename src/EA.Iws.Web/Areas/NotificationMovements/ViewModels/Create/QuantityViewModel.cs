namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Create
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
        public QuantityViewModel()
        {
        }

        public QuantityViewModel(ShipmentQuantityUnits notificationShipmentUnits, int movementNumber)
        {
            MovementNumber = movementNumber;
            NotificationUnits = notificationShipmentUnits;
            AvailableUnits = ShipmentQuantityUnitsMetadata.GetUnitsOfThisType(notificationShipmentUnits).ToList();
        }

        public int MovementNumber { get; set; }

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

        [Display(Name = "ActualQuantity", ResourceType = typeof(QuantityViewModelResources))]
        [Required(ErrorMessageResourceName = "ActualQuantityRequired", ErrorMessageResourceType = typeof(QuantityViewModelResources))]
        [IsValidNumber(maxPrecision: 18, ErrorMessageResourceName = "ActualQuantityIsValid", ErrorMessageResourceType = typeof(QuantityViewModelResources))]
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
                yield return new ValidationResult(QuantityViewModelResources.ActualQuantityPositive, new[] { "Quantity" });
            }

            if (Units.HasValue && decimal.Round(quantity, ShipmentQuantityUnitsMetadata.Precision[Units.Value]) != quantity)
            {
                yield return new ValidationResult(string.Format(
                    QuantityViewModelResources.ActualQuantityPrecision,
                    ShipmentQuantityUnitsMetadata.Precision[Units.Value]),
                    new[] { "Quantity" });
            }
        }
    }
}