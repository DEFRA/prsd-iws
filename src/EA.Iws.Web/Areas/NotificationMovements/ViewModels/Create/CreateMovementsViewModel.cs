namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Create
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Movement;
    using Core.PackagingType;
    using Core.Shared;
    using Infrastructure.Validation;
    using Prsd.Core;
    using Prsd.Core.Helpers;
    using Web.ViewModels.Shared;

    public class CreateMovementsViewModel : IValidatableObject
    {
        public CreateMovementsViewModel()
        {
        }

        public CreateMovementsViewModel(ShipmentInfo shipmentInfo)
        {
            StartDate = shipmentInfo.ShipmentDates.StartDate;
            EndDate = shipmentInfo.ShipmentDates.EndDate;
            NotificationUnits = shipmentInfo.ShipmentQuantityUnits;
            AvailableUnits = ShipmentQuantityUnitsMetadata.GetUnitsOfThisType(shipmentInfo.ShipmentQuantityUnits).ToList();

            var items = shipmentInfo.PackagingData.PackagingTypes
                .Where(x => x != PackagingType.Other)
                .Select(x => new SelectListItem
                {
                    Text = EnumHelper.GetDisplayName(x),
                    Value = ((int)x).ToString()
                })
                .ToList();

            if (shipmentInfo.PackagingData.PackagingTypes.Contains(PackagingType.Other))
            {
                items.Add(new SelectListItem
                {
                    Text = string.Format("{0} - {1}", EnumHelper.GetShortName(PackagingType.Other),
                        shipmentInfo.PackagingData.OtherDescription),
                    Value = ((int)PackagingType.Other).ToString()
                });
            }

            PackagingTypes = new CheckBoxCollectionViewModel();
            PackagingTypes.ShowEnumValue = true;
            PackagingTypes.PossibleValues = items;
        }

        [Required(ErrorMessage = "Please enter a valid number in the 'Day' field")]
        [Display(Name = "Day")]
        [Range(1, 31, ErrorMessage = "Please enter a valid number in the 'Day' field")]
        public int? Day { get; set; }

        [Required(ErrorMessage = "Please enter a valid number in the 'Month' field")]
        [Display(Name = "Month")]
        [Range(1, 12, ErrorMessage = "Please enter a valid number in the 'Month' field")]
        public int? Month { get; set; }

        [Required(ErrorMessage = "Please enter a valid number in the 'Year' field")]
        [Display(Name = "Year")]
        [Range(2015, 3000, ErrorMessage = "Please enter a valid number in the 'Year' field")]
        public int? Year { get; set; }

        public string DateHintText
        {
            get
            {
                var dateString = SystemTime.Now.Day + " " + SystemTime.Now.Month + " " + SystemTime.Now.Year;

                return "For example, " + dateString;
            }
        }

        [Required(ErrorMessage = "Please enter the number of shipments")]
        [Display(Name = "NumberToCreate", ResourceType = typeof(CreateMovementsViewModelResources))]
        public int? NumberToCreate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public SelectList UnitsSelectList
        {
            get
            {
                var units = AvailableUnits.Select(u => new
                {
                    Key = EnumHelper.GetDisplayName(u),
                    Value = (int)u
                });

                return new SelectList(units, "Value", "Key", (int)NotificationUnits);
            }
        }

        public IList<ShipmentQuantityUnits> AvailableUnits { get; set; }

        [Display(Name = "ActualQuantity", ResourceType = typeof(CreateMovementsViewModelResources))]
        [Required(ErrorMessageResourceName = "ActualQuantityRequired", ErrorMessageResourceType = typeof(CreateMovementsViewModelResources))]
        [IsValidNumber(maxPrecision: 18, NumberStyle = NumberStyles.AllowDecimalPoint, ErrorMessageResourceName = "ActualQuantityIsValid", ErrorMessageResourceType = typeof(CreateMovementsViewModelResources))]
        public string Quantity { get; set; }

        public ShipmentQuantityUnits? Units { get; set; }

        public ShipmentQuantityUnits NotificationUnits { get; set; }

        public string UnitsDisplay
        {
            get { return EnumHelper.GetDisplayName(Units); }
        }

        public CheckBoxCollectionViewModel PackagingTypes { get; set; }

        public IList<PackagingType> SelectedValues
        {
            get
            {
                return PackagingTypes
                    .PossibleValues
                    .Where(x => x.Selected)
                    .Select(x => (PackagingType)Convert.ToInt32(x.Value))
                    .ToList();
            }
        }

        public DateTime? AsDateTime()
        {
            if (Day.HasValue && Month.HasValue && Year.HasValue)
            {
                if (Day.Value > DateTime.DaysInMonth(Year.Value, Month.Value))
                {
                    return null;
                }
                return new DateTime(Year.Value, Month.Value, Day.Value);
            }
            return null;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NumberToCreate == null || NumberToCreate.Value <= 0)
            {
                yield return new ValidationResult("Please enter a valid number of shipments", new[] { "NumberToCreate" });
            }

            DateTime shipmentDate;
            bool isValidDate = SystemTime.TryParse(Year.GetValueOrDefault(),
                Month.GetValueOrDefault(),
                Day.GetValueOrDefault(),
                out shipmentDate);

            if (!isValidDate)
            {
                yield return new ValidationResult("Please enter a valid date",
                    new[] { "Day" });
            }
            else if (shipmentDate < SystemTime.UtcNow.Date)
            {
                yield return new ValidationResult("The actual date of shipment cannot be in the past. Please enter a different date.",
                    new[] { "Day" });
            }

            decimal quantity = Convert.ToDecimal(Quantity);

            if (quantity <= 0)
            {
                yield return new ValidationResult(CreateMovementsViewModelResources.ActualQuantityPositive, new[] { "Quantity" });
            }

            if (Units.HasValue && decimal.Round(quantity, ShipmentQuantityUnitsMetadata.Precision[Units.Value]) != quantity)
            {
                yield return new ValidationResult(string.Format(
                    CreateMovementsViewModelResources.ActualQuantityPrecision,
                    ShipmentQuantityUnitsMetadata.Precision[Units.Value] + 1),
                    new[] { "Quantity" });
            }

            if (!SelectedValues.Any())
            {
                yield return new ValidationResult("Please select at least one packaging type", new[] { "PackagingTypes" });
            }
        }
    }
}