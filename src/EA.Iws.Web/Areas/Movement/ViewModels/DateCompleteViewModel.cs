namespace EA.Iws.Web.Areas.Movement.ViewModels
{
    using System;
    using System.Collections.Generic;
    using Core.Shared;
    using System.ComponentModel.DataAnnotations;
    using EA.Prsd.Core;

    public class DateCompleteViewModel : IValidatableObject
    {
        public NotificationType NotificationType { get; set; }

        [Required]
        public int? Day { get; set; }
        [Required]
        public int? Month { get; set; }
        [Required]
        public int? Year { get; set; }

        public DateCompleteViewModel()
        {
        }

        public DateCompleteViewModel(DateTime? dateComplete, NotificationType notificationType)
        {
            if (dateComplete.HasValue)
            {
                Day = dateComplete.Value.Day;
                Month = dateComplete.Value.Month;
                Year = dateComplete.Value.Year;
            }

            NotificationType = notificationType;
        }

        public DateTime GetDateComplete()
        {
            DateTime dateComplete;
            if (ParseDateInput(out dateComplete))
            {
                return dateComplete;
            }
            else
            {
                throw new InvalidOperationException("Date not valid");
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            DateTime dateComplete;
            if (!ParseDateInput(out dateComplete))
            {
                yield return new ValidationResult("Please enter a valid date", new[] { "Day" });
            }

            if (dateComplete > SystemTime.UtcNow)
            {
                yield return new ValidationResult("Date completed cannot be in the future", new[] { "Day" });
            }
        }

        private bool ParseDateInput(out DateTime dateComplete)
        {
            return SystemTime.TryParse(
                Year.GetValueOrDefault(),
                Month.GetValueOrDefault(),
                Day.GetValueOrDefault(),
                out dateComplete);
        }
    }
}