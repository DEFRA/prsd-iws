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

        [Display(Name = "Day")]
        [Range(1, 31, ErrorMessageResourceType = typeof(CreateMovementsViewModelResources), ErrorMessageResourceName = "DayRequired")]
        public int? Day { get; set; }

        [Display(Name = "Month")]
        [Range(1, 12, ErrorMessageResourceType = typeof(CreateMovementsViewModelResources), ErrorMessageResourceName = "MonthRequired")]
        public int? Month { get; set; }

        [Display(Name = "Year")]
        [Range(2015, 3000, ErrorMessageResourceType = typeof(CreateMovementsViewModelResources), ErrorMessageResourceName = "YearRequired")]
        public int? Year { get; set; }

        public string DateHintText
        {
            get
            {
                var dateString = SystemTime.Now.Day + " " + SystemTime.Now.Month + " " + SystemTime.Now.Year;

                return CreateMovementsViewModelResources.DateHint + dateString;
            }
        }

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
        public string Quantity { get; set; }

        public ShipmentQuantityUnits? Units { get; set; }

        public ShipmentQuantityUnits NotificationUnits { get; set; }

        public string UnitsDisplay
        {
            get { return EnumHelper.GetDisplayName(Units); }
        }

        public CheckBoxCollectionViewModel PackagingTypes { get; set; }

        public IList<PackagingType> SelectedPackagingTypes
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

        public DateTime? ShipmentDate
        {
            get
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
        }

        public bool OverrideRule { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NumberToCreate == null)
            {
                yield return new ValidationResult(CreateMovementsViewModelResources.NumberToCreateRequired, new[] { "NumberToCreate" });
            }
            int result;
            if (NumberToCreate != null && (!int.TryParse(NumberToCreate.ToString(), out result) || NumberToCreate.Value <= 0))
            {
                yield return new ValidationResult(CreateMovementsViewModelResources.NumberToCreateValid, new[] { "NumberToCreate" });
            }
            if (Day == null)
            {
                yield return new ValidationResult(CreateMovementsViewModelResources.DayRequired, new[] { "Day" });
            }
            if (Month == null)
            {
                yield return new ValidationResult(CreateMovementsViewModelResources.MonthRequired, new[] { "Month" });
            }
            if (Year == null)
            {
                yield return new ValidationResult(CreateMovementsViewModelResources.YearRequired, new[] { "Year" });
            }
            if (string.IsNullOrEmpty(Quantity))
            {
                yield return new ValidationResult(CreateMovementsViewModelResources.ActualQuantityRequired, new[] { "Quantity" });
            }
            if (!SelectedPackagingTypes.Any())
            {
                yield return new ValidationResult(CreateMovementsViewModelResources.PackagingTypeRequired, new[] { "PackagingTypes" });
            }
          
            if (!string.IsNullOrEmpty(Quantity))
            {
                decimal number;
                if (decimal.TryParse(Quantity, NumberStyles.AllowDecimalPoint, new NumberFormatInfo(), out number))
                {
                    if (!NumberIsValid(number))
                    {
                        yield return new ValidationResult(CreateMovementsViewModelResources.ActualQuantityIsValid, new[] { "Quantity" });
                    }
                    else
                    {
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
                    }
                }
                else
                {
                    yield return new ValidationResult(CreateMovementsViewModelResources.ActualQuantityIsValid, new[] { "Quantity" });
                }
            }
            if (Day != null && Month != null && Year != null)
            {
                DateTime shipmentDate;
                bool isValidDate = SystemTime.TryParse(Year.GetValueOrDefault(),
                    Month.GetValueOrDefault(),
                    Day.GetValueOrDefault(),
                    out shipmentDate);

                if (!isValidDate)
                {
                    yield return new ValidationResult(CreateMovementsViewModelResources.DateValid,
                        new[] { "Day" });
                }
                else if (shipmentDate < SystemTime.UtcNow.Date)
                {
                    yield return new ValidationResult(CreateMovementsViewModelResources.DateNotInPast,
                        new[] { "Day" });
                }
            }
        }

        private bool NumberIsValid(decimal number)
        {
            var maxNumber = (long)Math.Pow(10, 18) - 1;
            var minNumber = 0 - maxNumber;

            return number > minNumber && number < maxNumber;
        }
    }
}