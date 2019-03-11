namespace EA.Iws.Web.ViewModels.Shared
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using EA.Prsd.Core;

    public class DateEntryViewModel : IValidatableObject
    {
        [Range(1, 31)]
        public int? Day { get; set; }

        [Range(1, 12)]
        public int? Month { get; set; }

        public int? Year { get; set; }

        public bool AllowPastDates { get; set; }

        public bool ShowLabels { get; set; }

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

        public DateEntryViewModel(bool allowPastDates = false, bool showLabels = true)
        {
            AllowPastDates = allowPastDates;
            ShowLabels = showLabels;
            IsAutoTabEnabled = true;
        }

        public DateEntryViewModel(DateTime? date, bool allowPastDates = false, bool showLabels = true)
        {
            IsAutoTabEnabled = true;
            AllowPastDates = allowPastDates;
            ShowLabels = showLabels;
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

        public bool IsStarted
        {
            get { return Day.HasValue || Month.HasValue || Year.HasValue; }
        }

        public bool IsAutoTabEnabled { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
                if (Day == null || Day < 1 || Day > 31)
                {
                    yield return new ValidationResult("Please enter a valid number in the 'Day' field", new[] { "Day" });
                }
                if (Month == null || Month < 1 || Month > 12)
                {
                    yield return new ValidationResult("Please enter a valid number in the 'Month' field", new[] { "Month" });
                }
                if (Year == null || Year < 2014 || Year > 3000)
                {
                    yield return new ValidationResult("Please enter a valid number in the 'Year' field", new[] { "Year" });
                }
        }
    }
}