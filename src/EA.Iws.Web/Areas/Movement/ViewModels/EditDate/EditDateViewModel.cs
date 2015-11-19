namespace EA.Iws.Web.Areas.Movement.ViewModels.EditDate
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Prsd.Core;

    public class EditDateViewModel : IValidatableObject
    {
        public IList<DateTime> DateEditHistory { get; set; }

        [Required]
        public int? Day { get; set; }

        [Required]
        public int? Month { get; set; }

        public DateTime OriginalDate
        {
            get { return DateEditHistory.First(); }
        }

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
            DateTime shipmentDate;
            bool isValidDate = SystemTime.TryParse(Year.GetValueOrDefault(), Month.GetValueOrDefault(), Day.GetValueOrDefault(), out shipmentDate);
            if (!isValidDate)
            {
                yield return new ValidationResult("Please enter a valid date", new[] { "Day" });
            }
            if (AsDateTime() > OriginalDate.AddDays(10))
            {
                yield return new ValidationResult(string.Format("Please enter a date that is not 10 days greater than {0:dd MMM yyyy}", OriginalDate), new[] { "Day" });
            }
        }
    }
}