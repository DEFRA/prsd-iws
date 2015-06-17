namespace EA.Iws.Web.ViewModels.Shipment
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Prsd.Core;
    using Prsd.Core.Helpers;
    using Requests.Shipment;

    public class ShipmentInfoViewModel : IValidatableObject
    {
        public ShipmentInfoViewModel()
        {
            UnitsSelectList = new SelectList(EnumHelper.GetValues(typeof(ShipmentQuantityUnits)), "Key", "Value");
        }

        public ShipmentInfoViewModel(ShipmentData shipmentData)
        {
            NotificationId = shipmentData.NotificationId;
            UnitsSelectList = new SelectList(EnumHelper.GetValues(typeof(ShipmentQuantityUnits)), "Key", "Value");
            IsPreconsentedRecoveryFacility = shipmentData.IsPreconsentedRecoveryFacility;

            if (shipmentData.HasShipmentData)
            {
                EndDay = shipmentData.LastDate.Day;
                EndMonth = shipmentData.LastDate.Month;
                EndYear = shipmentData.LastDate.Year;
                NumberOfShipments = shipmentData.NumberOfShipments;
                Quantity = shipmentData.Quantity;
                StartDay = shipmentData.FirstDate.Day;
                StartMonth = shipmentData.FirstDate.Month;
                StartYear = shipmentData.FirstDate.Year;
                Units = shipmentData.Units;
            }
        }

        public Guid NotificationId { get; set; }

        public bool IsPreconsentedRecoveryFacility { get; set; }

        [Required(ErrorMessage = "Please enter the total number of intended shipments")]
        [Display(Name = "Number of shipments")]
        [Range(1, 99999, ErrorMessage = "The number of shipments must be at least 1 and cannot be greater than 99999")]
        [RegularExpression("[0-9]*", ErrorMessage = "The number of shipments must be a whole number")]
        public int? NumberOfShipments { get; set; }

        [Required(ErrorMessage = "Please enter the total intended quantity")]
        [Range(0.0001, 99999, ErrorMessage = "The quantity must be between 0.0001 and 99999")]
        [RegularExpression(@"\d+(\.\d{1,4})?",
            ErrorMessage = "The quantity must be a number with a maximum of 4 decimal places.")]
        public decimal? Quantity { get; set; }

        [Required(ErrorMessage = "Please select the units.")]
        public ShipmentQuantityUnits? Units { get; set; }

        public IEnumerable<SelectListItem> UnitsSelectList { get; set; }

        [Required(ErrorMessage = "Please enter a valid number in the 'Day' field")]
        [Display(Name = "Day")]
        [Range(1, 31, ErrorMessage = "Please enter a valid number in the 'Day' field")]
        public int? StartDay { get; set; }

        [Required(ErrorMessage = "Please enter a valid number in the 'Month' field")]
        [Display(Name = "Month")]
        [Range(1, 12, ErrorMessage = "Please enter a valid number in the 'Month' field")]
        public int? StartMonth { get; set; }

        [Required(ErrorMessage = "Please enter a valid number in the 'Year' field")]
        [Display(Name = "Year")]
        [Range(2015, 3000, ErrorMessage = "Please enter a valid number in the 'Year' field")]
        public int? StartYear { get; set; }

        [Required(ErrorMessage = "Please enter a valid number in the 'Day' field")]
        [Display(Name = "Day")]
        [Range(1, 31, ErrorMessage = "Please enter a valid number in the 'Day' field")]
        public int? EndDay { get; set; }

        [Required(ErrorMessage = "Please enter a valid number in the 'Month' field")]
        [Display(Name = "Month")]
        [Range(1, 12, ErrorMessage = "Please enter a valid number in the 'Month' field")]
        public int? EndMonth { get; set; }

        [Required(ErrorMessage = "Please enter a valid number in the 'Year' field")]
        [Display(Name = "Year")]
        [Range(2015, 3000, ErrorMessage = "Please enter a valid number in the 'Year' field")]
        public int? EndYear { get; set; }

        public DateTime StartDate
        {
            get { return new DateTime(StartYear.GetValueOrDefault(), StartMonth.GetValueOrDefault(), StartDay.GetValueOrDefault()); }
        }

        public DateTime EndDate
        {
            get { return new DateTime(EndYear.GetValueOrDefault(), EndMonth.GetValueOrDefault(), EndDay.GetValueOrDefault()); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate < SystemTime.Now.Date)
            {
                yield return new ValidationResult("The start date cannot be in the past");
            }

            if (StartDate > EndDate)
            {
                yield return new ValidationResult("The start date must be before the end date", new[] { "StartYear" });
            }

            var monthPeriodLength = IsPreconsentedRecoveryFacility ? 36 : 12;
            if (EndDate > StartDate.AddMonths(monthPeriodLength))
            {
                yield return
                    new ValidationResult(
                        string.Format("The start date and end date must be within a {0} month period.",
                            monthPeriodLength), new[] { "EndYear" });
            }
        }

        public CreateOrUpdateShipmentInfo ToRequest()
        {
            return new CreateOrUpdateShipmentInfo(
                NotificationId,
                NumberOfShipments.GetValueOrDefault(),
                Quantity.GetValueOrDefault(),
                Units.GetValueOrDefault(),
                StartDate,
                EndDate);
        }
    }
}