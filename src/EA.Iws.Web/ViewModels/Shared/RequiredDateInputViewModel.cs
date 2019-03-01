namespace EA.Iws.Web.ViewModels.Shared
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Prsd.Core;

    public class RequiredDateInputViewModel : IValidatableObject
    {
        [Required(ErrorMessageResourceName = "DayError", ErrorMessageResourceType = typeof(RequiredDateInputResources))]
        [Range(1, 31, ErrorMessageResourceName = "DayError", ErrorMessageResourceType = typeof(RequiredDateInputResources))]
        public int? Day { get; set; }

        [Required(ErrorMessageResourceName = "MonthError", ErrorMessageResourceType = typeof(RequiredDateInputResources))]
        [Range(1, 12, ErrorMessageResourceName = "MonthError", ErrorMessageResourceType = typeof(RequiredDateInputResources))]
        public int? Month { get; set; }

        [Required(ErrorMessageResourceName = "YearError", ErrorMessageResourceType = typeof(RequiredDateInputResources))]
        [Range(2000, 3000, ErrorMessageResourceName = "YearError", ErrorMessageResourceType = typeof(RequiredDateInputResources))]
        public int? Year { get; set; }

        public bool AllowPastDates { get; set; }

        public bool ShowLabels { get; set; }

        public bool IsAutoTabEnabled { get; set; }

        public RequiredDateInputViewModel(bool allowPastDates = false, bool showLabels = true)
        {
            AllowPastDates = allowPastDates;
            ShowLabels = showLabels;
            IsAutoTabEnabled = true;
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
            if (Day.HasValue && Month.HasValue && Year.HasValue)
            {
                if (CheckRange(1, 31, Day.Value) && CheckRange(1, 12, Month.Value) && CheckRange(2000, 3000, Year.Value))
                {
                    DateTime decisionRequireByDate;
                    bool isValidDate = SystemTime.TryParse(Year.GetValueOrDefault(), Month.GetValueOrDefault(), Day.GetValueOrDefault(), out decisionRequireByDate);
                    if (!isValidDate)
                    {
                        yield return new ValidationResult(DecisionDateResources.FromValid, new[] { "Day" });
                    }
                }
            }
        }

        private static bool CheckRange(int start, int end, int value)
        {
            return (value >= start && value <= end);
        }
    }
}