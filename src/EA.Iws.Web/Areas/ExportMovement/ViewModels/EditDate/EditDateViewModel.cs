namespace EA.Iws.Web.Areas.ExportMovement.ViewModels.EditDate
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Prsd.Core;

    public class EditDateViewModel : IValidatableObject
    {
        public IList<DateTime> DateEditHistory { get; set; }

        [Required]
        public int? Day { get; set; }

        [Required]
        public int? Month { get; set; }

        [Required]
        public int? Year { get; set; }

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
            if (!AsDateTime().HasValue)
            {
                yield return new ValidationResult("Please enter a valid date", new[] { "Day" });
            }
            else if (AsDateTime() < SystemTime.UtcNow)
            {
                yield return new ValidationResult(string.Format("Please enter a future date"), new[] { "Day" });
            }
        }
    }
}