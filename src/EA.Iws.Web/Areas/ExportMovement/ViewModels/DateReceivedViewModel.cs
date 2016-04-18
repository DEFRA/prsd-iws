namespace EA.Iws.Web.Areas.ExportMovement.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Prsd.Core;

    public class DateReceivedViewModel : IValidatableObject
    {
        private const string invalidDay = "Please enter a valid number in the 'Day' field";
        private const string invalidMonth = "Please enter a valid number in the 'Month' field";
        private const string invalidYear = "Please enter a valid number in the 'Year' field";

        [Required(ErrorMessage = invalidDay)]
        [Range(1, 31, ErrorMessage = invalidDay)]
        public int? Day { get; set; }

        [Required(ErrorMessage = invalidMonth)]
        [Range(1, 12, ErrorMessage = invalidMonth)]
        public int? Month { get; set; }

        [Required(ErrorMessage = invalidYear)]
        [Range(2015, 3000, ErrorMessage = invalidYear)]
        public int? Year { get; set; }

        public DateTime MovementDate { get; set; }

        public DateReceivedViewModel()
        {
        }
        
        public DateReceivedViewModel(DateTime movementDate)
        {
            MovementDate = movementDate;
        }

        public DateTime GetDateReceived()
        {
            DateTime dateReceived;
            if (ParseDateInput(out dateReceived))
            {
                return dateReceived;
            }
            else
            {
                throw new InvalidOperationException("Date not valid");
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            DateTime dateReceived;
            if (!ParseDateInput(out dateReceived))
            {
                yield return new ValidationResult("Please enter a valid date", new[] { "Day" });
            }
            else if (dateReceived.Date > SystemTime.UtcNow.Date)
            {
                yield return new ValidationResult("This date cannot be in the future. Please enter a different date.", new[] { "Day" });
            }
            else if (dateReceived < MovementDate)
            {
                yield return new ValidationResult("This date cannot be before the actual date of shipment. Please enter a different date.", new[] { "Day" });
            }
        }

        private bool ParseDateInput(out DateTime dateReceived)
        {
            return SystemTime.TryParse(
                Year.GetValueOrDefault(),
                Month.GetValueOrDefault(),
                Day.GetValueOrDefault(),
                out dateReceived);
        }
    }
}