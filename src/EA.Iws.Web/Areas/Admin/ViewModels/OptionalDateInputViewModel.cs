namespace EA.Iws.Web.Areas.Admin.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class OptionalDateInputViewModel : IValidatableObject
    {
        [Range(1, 31)]
        public int? Day { get; set; }

        [Range(1, 12)]
        public int? Month { get; set; }

        public int? Year { get; set; }

        public bool AllowPastDates { get; set; }

        public bool IsCompleted
        {
            get 
            { 
                return Day.HasValue 
                && Month.HasValue 
                && Year.HasValue
                && Year.Value > 1 && Year.Value < 9999
                && Day.Value <= DateTime.DaysInMonth(Year.Value, Month.Value); 
            }
        }

        public OptionalDateInputViewModel(bool allowPastDates = false)
        {
            AllowPastDates = allowPastDates;
        }

        public OptionalDateInputViewModel(DateTime? date)
        {
            if (date.HasValue)
            {
                Day = date.Value.Day;
                Month = date.Value.Month;
                Year = date.Value.Year;
            }
        }

        public DateTime? AsDateTime()
        {
            if (Day.HasValue && Month.HasValue && Year.HasValue)
            {
                if ((Year > 9999) || (AllowPastDates && Year < 2000) || (!AllowPastDates && Year < 2010) || Day.Value > DateTime.DaysInMonth(Year.Value, Month.Value))
                {
                    return null;
                }
                return new DateTime(Year.Value, Month.Value, Day.Value);
            }
            return null;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Day.HasValue || Month.HasValue || Year.HasValue)
            {
                if (!Day.HasValue)
                {
                    yield return new ValidationResult("The Day field is required.", new[] { "Day" });
                }

                if (!Month.HasValue)
                {
                    yield return new ValidationResult("The Month field is required.", new[] { "Month" });
                }

                if (!Year.HasValue)
                {
                    yield return new ValidationResult("The Year field is required.", new[] { "Year" });
                }

                if (Day.HasValue && Month.HasValue && Year.HasValue)
                {
                    if ((Year > 9999) || (AllowPastDates && Year < 2000))
                    {
                        yield return new ValidationResult("The year must be between 2000 and 99999", new[] { "Year" });
                        yield break;
                    }

                    if (!AllowPastDates && Year < 2010)
                    {
                        yield return new ValidationResult("The year must be between 2010 and 99999", new[] { "Year" });
                    }

                    if (Day.Value > DateTime.DaysInMonth(Year.Value, Month.Value))
                    {
                        yield return new ValidationResult("The Day does not exist in the given month", new[] { "Day" });
                    }
                }
            }
        }
    }
}