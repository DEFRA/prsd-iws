namespace EA.Iws.Web.Areas.MovementDocument.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Prsd.Core;

    public class ShipmentDateViewModel : IValidatableObject
    {
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
        }
    }
}