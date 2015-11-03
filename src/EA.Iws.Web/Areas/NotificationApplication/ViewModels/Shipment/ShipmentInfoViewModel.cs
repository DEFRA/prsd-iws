namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Shipment
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using Core.IntendedShipments;
    using Core.Shared;
    using Prsd.Core;
    using Prsd.Core.Helpers;
    using Requests.IntendedShipments;
    using Views.Shipment;

    public class ShipmentInfoViewModel : IValidatableObject
    {
        private const NumberStyles Style = NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint;

        public ShipmentInfoViewModel()
        {
            UnitsSelectList = new SelectList(EnumHelper.GetValues(typeof(ShipmentQuantityUnits)), "Key", "Value");
        }

        public ShipmentInfoViewModel(IntendedShipmentData intendedShipmentData)
        {
            NotificationId = intendedShipmentData.NotificationId;
            UnitsSelectList = new SelectList(EnumHelper.GetValues(typeof(ShipmentQuantityUnits)), "Key", "Value");
            IsPreconsentedRecoveryFacility = intendedShipmentData.IsPreconsentedRecoveryFacility;

            if (intendedShipmentData.HasShipmentData)
            {
                EndDay = intendedShipmentData.LastDate.Day;
                EndMonth = intendedShipmentData.LastDate.Month;
                EndYear = intendedShipmentData.LastDate.Year;
                NumberOfShipments = intendedShipmentData.NumberOfShipments.ToString();
                Quantity = intendedShipmentData.Quantity.ToString("G29");
                StartDay = intendedShipmentData.FirstDate.Day;
                StartMonth = intendedShipmentData.FirstDate.Month;
                StartYear = intendedShipmentData.FirstDate.Year;
                Units = intendedShipmentData.Units;
            }
        }

        public Guid NotificationId { get; set; }

        public bool IsPreconsentedRecoveryFacility { get; set; }

        [Required(ErrorMessageResourceName = "NumberOfShipmentsRequired", ErrorMessageResourceType = typeof(ShipmentResources))]
        [Display(Name = "NumberOfShipments", ResourceType = typeof(ShipmentResources))]
        public string NumberOfShipments { get; set; }

        [Required(ErrorMessageResourceName = "QuantityRequired", ErrorMessageResourceType = typeof(ShipmentResources))]
        public string Quantity { get; set; }

        [Required(ErrorMessageResourceName = "UnitsRequired", ErrorMessageResourceType = typeof(ShipmentResources))]
        public ShipmentQuantityUnits? Units { get; set; }

        public IEnumerable<SelectListItem> UnitsSelectList { get; set; }

        [Required(ErrorMessageResourceName = "DayError", ErrorMessageResourceType = typeof(ShipmentResources))]
        [Display(Name = "Day", ResourceType = typeof(ShipmentResources))]
        [Range(1, 31, ErrorMessageResourceName = "DayError", ErrorMessageResourceType = typeof(ShipmentResources))]
        public int? StartDay { get; set; }

        [Required(ErrorMessageResourceName = "MonthError", ErrorMessageResourceType = typeof(ShipmentResources))]
        [Display(Name = "Month", ResourceType = typeof(ShipmentResources))]
        [Range(1, 12, ErrorMessageResourceName = "MonthError", ErrorMessageResourceType = typeof(ShipmentResources))]
        public int? StartMonth { get; set; }

        [Required(ErrorMessageResourceName = "YearError", ErrorMessageResourceType = typeof(ShipmentResources))]
        [Display(Name = "Year", ResourceType = typeof(ShipmentResources))]
        [Range(2015, 3000, ErrorMessageResourceName = "YearError", ErrorMessageResourceType = typeof(ShipmentResources))]
        public int? StartYear { get; set; }

        [Required(ErrorMessageResourceName = "DayError", ErrorMessageResourceType = typeof(ShipmentResources))]
        [Display(Name = "Day", ResourceType = typeof(ShipmentResources))]
        [Range(1, 31, ErrorMessageResourceName = "DayError", ErrorMessageResourceType = typeof(ShipmentResources))]
        public int? EndDay { get; set; }

        [Required(ErrorMessageResourceName = "MonthError", ErrorMessageResourceType = typeof(ShipmentResources))]
        [Display(Name = "Month", ResourceType = typeof(ShipmentResources))]
        [Range(1, 12, ErrorMessageResourceName = "MonthError", ErrorMessageResourceType = typeof(ShipmentResources))]
        public int? EndMonth { get; set; }

        [Required(ErrorMessageResourceName = "YearError", ErrorMessageResourceType = typeof(ShipmentResources))]
        [Display(Name = "Year", ResourceType = typeof(ShipmentResources))]
        [Range(2015, 3000, ErrorMessageResourceName = "YearError", ErrorMessageResourceType = typeof(ShipmentResources))]
        public int? EndYear { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!IsNumberOfShipmentsValid())
            {
                yield return new ValidationResult(ShipmentResources.NumberOfShipmentsValid, new[] { "NumberOfShipments" });
            }

            if (!IsQuantityValid() && Units.HasValue)
            {
                yield return new ValidationResult(string.Format(ShipmentResources.DecimalPlacesValid, 
                    ShipmentQuantityUnitsMetadata.Precision[Units.Value]), new[] { "Quantity" });
            }

            DateTime startDate;
            bool isValidStartDate = SystemTime.TryParse(StartYear.GetValueOrDefault(), StartMonth.GetValueOrDefault(), StartDay.GetValueOrDefault(), out startDate);
            if (!isValidStartDate)
            {
                yield return new ValidationResult(ShipmentResources.FirstDepartureValid, new[] { "StartDay" });
            }

            DateTime endDate;
            bool isValidEndDate = SystemTime.TryParse(EndYear.GetValueOrDefault(), EndMonth.GetValueOrDefault(), EndDay.GetValueOrDefault(), out endDate);
            if (!isValidEndDate)
            {
                yield return new ValidationResult(ShipmentResources.LastDepartureValid, new[] { "EndDay" });
            }

            if (!(isValidStartDate && isValidEndDate))
            {
                // Stop further validation if either date is not a valid date
                yield break;
            }

            if (startDate < SystemTime.Now.Date)
            {
                yield return new ValidationResult(ShipmentResources.FirstDeparturePastDate);
            }

            if (startDate > endDate)
            {
                yield return new ValidationResult(ShipmentResources.FirstDepartureBeforeLastDate, new[] { "StartYear" });
            }

            var monthPeriodLength = IsPreconsentedRecoveryFacility ? 36 : 12;
            if (endDate >= startDate.AddMonths(monthPeriodLength))
            {
                yield return new ValidationResult(string.Format(ShipmentResources.DepartureDateRange, monthPeriodLength), new[] { "EndYear" });
            }
        }

        private bool IsNumberOfShipmentsValid()
        {
            int numberOfShipments;
            string numberOfShipmentsString = NumberOfShipments;
            if (numberOfShipmentsString.Contains(","))
            {
                Regex rgx = new Regex(@"^(?=[\d.])\d{0,3}(?:\d*|(?:,\d{3})*)?$");
                if (rgx.IsMatch(numberOfShipmentsString))
                {
                    numberOfShipmentsString = numberOfShipmentsString.Replace(",", string.Empty);
                }
                else
                {
                    return false;
                }
            }

            if (!Int32.TryParse(numberOfShipmentsString, out numberOfShipments))
            {
                return false;
            }

            if (numberOfShipments < 1 || numberOfShipments > 99999)
            {
                return false;
            }
            return true;
        }

        public bool IsQuantityValid()
        {
            if (!Units.HasValue)
            {
                return false;
            }

            decimal quantity;
            return Decimal.TryParse(Quantity, Style, CultureInfo.CurrentCulture, out quantity)
                && decimal.Round(quantity, ShipmentQuantityUnitsMetadata.Precision[Units.Value]) == quantity;
        }

        public SetIntendedShipmentInfoForNotification ToRequest()
        {
            DateTime startDate;
            SystemTime.TryParse(StartYear.GetValueOrDefault(), StartMonth.GetValueOrDefault(), StartDay.GetValueOrDefault(), out startDate);

            DateTime endDate;
            SystemTime.TryParse(EndYear.GetValueOrDefault(), EndMonth.GetValueOrDefault(), EndDay.GetValueOrDefault(), out endDate);

            return new SetIntendedShipmentInfoForNotification(
                NotificationId,
                Convert.ToInt32(NumberOfShipments),
                Convert.ToDecimal(Quantity),
                Units.GetValueOrDefault(),
                startDate,
                endDate);
        }
    }
}