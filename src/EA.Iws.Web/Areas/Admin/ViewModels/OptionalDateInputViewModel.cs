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

        [Range(2010, int.MaxValue)]
        public int? Year { get; set; }

        public bool IsCompleted
        {
            get 
            { 
                return Day.HasValue 
                && Month.HasValue 
                && Year.HasValue
                && Day.Value <= DateTime.DaysInMonth(Year.Value, Month.Value); 
            }
        }

        public OptionalDateInputViewModel()
        {
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
                    if (Day.Value > DateTime.DaysInMonth(Year.Value, Month.Value))
                    {
                        yield return new ValidationResult("The Day does not exist in the given month", new[] { "Day" });
                    }
                }
            }
        }
    }
}