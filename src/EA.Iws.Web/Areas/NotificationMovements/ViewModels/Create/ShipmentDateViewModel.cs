namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Create
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.Movement;
    using Prsd.Core;

    public class ShipmentDateViewModel : IValidatableObject
    {
        public ShipmentDateViewModel()
        {
        }

        public ShipmentDateViewModel(ShipmentDates shipmentDates, int movementNumber)
        {
            StartDate = shipmentDates.StartDate;
            EndDate = shipmentDates.EndDate;
            MovementNumber = movementNumber;
        }

        public int MovementNumber { get; set; }

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

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string DateHintText
        {
            get
            {
                var dateString = SystemTime.Now.Day + " " + SystemTime.Now.Month + " " + SystemTime.Now.Year;

                return "For example, " + dateString;
            }
        }

        public Guid MovementId { get; set; }

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
            DateTime shipmentDate;
            bool isValidDate = SystemTime.TryParse(Year.GetValueOrDefault(), Month.GetValueOrDefault(), Day.GetValueOrDefault(), out shipmentDate);
            if (!isValidDate)
            {
                yield return new ValidationResult("Please enter a valid date", new[] { "Day" });
            }
            else if (shipmentDate < StartDate || shipmentDate > EndDate)
            {
                yield return new ValidationResult("The date is not within the given range", new[] { "Day" });
            }
        }
    }
}