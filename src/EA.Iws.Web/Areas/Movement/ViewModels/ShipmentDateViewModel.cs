namespace EA.Iws.Web.Areas.Movement.ViewModels
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

            if (movementDatesData.ActualDate != null)
            {
                Day = movementDatesData.ActualDate.Value.Day;
                Month = movementDatesData.ActualDate.Value.Month;
                Year = movementDatesData.ActualDate.Value.Year;
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

        public string DateHintText
        {
            get
            {
                var dateString = SystemTime.Now.Day + " " + SystemTime.Now.Month + " " + SystemTime.Now.Year;

                return "For example, " + dateString;
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
                yield return new ValidationResult("The date is not within the given range", new[] { "Day" });
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