namespace EA.Iws.Web.Areas.MovementDocument.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Prsd.Core;
    using Requests.Movement;

    public class ShipmentDateViewModel : IValidatableObject
    {
        public ShipmentDateViewModel()
        {
        }

        public ShipmentDateViewModel(MovementDatesData movementDatesData)
        {
            MovementId = movementDatesData.MovementId;
            StartDate = movementDatesData.FirstDate;
            EndDate = movementDatesData.LastDate;

            if (movementDatesData.ActualDate.Year > 1)
            {
                Day = movementDatesData.ActualDate.Day;
                Month = movementDatesData.ActualDate.Month;
                Year = movementDatesData.ActualDate.Year;
            }
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

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string StartDateString 
        {
            get
            {
                return StartDate.ToString("dd.MM.yyyy");
            }
        }

        public string EndDateString
        {
            get
            {
                return EndDate.ToString("dd.MM.yyyy");
            }
        }

        public Guid MovementId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            DateTime shipmentDate;
            bool isValidDate = SystemTime.TryParse(Year.GetValueOrDefault(), Month.GetValueOrDefault(), Day.GetValueOrDefault(), out shipmentDate);
            if (!isValidDate)
            {
                yield return new ValidationResult("Please enter a valid date", new[] {"Day"});
            }

            if (shipmentDate < StartDate || shipmentDate > EndDate)
            {
                yield return new ValidationResult(string.Format("The date must be between {0} and {1}", StartDateString, EndDateString), new[] { "Day" });
            }

            if (shipmentDate < SystemTime.Now.Date)
            {
                yield return new ValidationResult("The shipment date cannot be in the past", new[] { "Day" });
            }
        }

        public SetActualDateOfMovement ToRequest()
        {
            DateTime date;
            SystemTime.TryParse(Year.GetValueOrDefault(), Month.GetValueOrDefault(), Day.GetValueOrDefault(), out date);

            return new SetActualDateOfMovement(MovementId, date);
        }
    }
}