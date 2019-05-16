namespace EA.Iws.Web.ViewModels.Shared
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Prsd.Core;

    public class DateInputViewmodel : IValidatableObject
    {
        [Required(ErrorMessageResourceName = "DayError", ErrorMessageResourceType = typeof(DateInputResources))]
        [Range(1, 31, ErrorMessageResourceName = "DayError", ErrorMessageResourceType = typeof(DateInputResources))]
        public int? Day { get; set; }

        [Required(ErrorMessageResourceName = "MonthError", ErrorMessageResourceType = typeof(DateInputResources))]
        [Range(1, 12, ErrorMessageResourceName = "MonthError", ErrorMessageResourceType = typeof(DateInputResources))]
        public int? Month { get; set; }

        [Required(ErrorMessageResourceName = "YearError", ErrorMessageResourceType = typeof(DateInputResources))]
        [Range(2000, 3000, ErrorMessageResourceName = "YearError", ErrorMessageResourceType = typeof(DateInputResources))]
        public int? Year { get; set; }

        public bool AllowPastDates { get; set; }

        public bool ShowLabels { get; set; }

        public bool HadValues { get; set; }

        public bool HasAnyValues
        {
            get
            {
                return Day.HasValue
                || Month.HasValue
                || Year.HasValue;
            }
        }

        public DateInputViewmodel(bool allowPastDates = false, bool showLabels = true)
        {
            AllowPastDates = allowPastDates;
            ShowLabels = showLabels;
            IsAutoTabEnabled = true;
        }

        public DateInputViewmodel(DateTime? date, bool allowPastDates = false, bool showLabels = true)
        {
            IsAutoTabEnabled = true;
            AllowPastDates = allowPastDates;
            ShowLabels = showLabels;
            if (date.HasValue)
            {
                Day = date.Value.Day;
                Month = date.Value.Month;
                Year = date.Value.Year;
                HadValues = true;
            }
            else
            {
                HadValues = false;
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

        public bool CheckRange(int start, int end, int value)
        {
            return (value >= start && value <= end);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Day.HasValue && Month.HasValue && Year.HasValue)
            {
                if (CheckRange(1, 31, Day.Value) && CheckRange(1, 12, Month.Value) && CheckRange(2000, 3000, Year.Value))
                {
                    DateTime decisionRequireByDate;
                    if (
                        !SystemTime.TryParse(Year.GetValueOrDefault(), Month.GetValueOrDefault(),
                            Day.GetValueOrDefault(), out decisionRequireByDate))
                    {
                        yield return new ValidationResult(DateInputResources.FromValid, new[] { "Day" });
                    }
                }
            }
        }
    }
}