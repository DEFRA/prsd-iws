namespace EA.Iws.Web.Areas.ExportMovement.ViewModels.Complete
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.Movement;
    using Core.Shared;
    using Prsd.Core;

    public class DateViewModel : IValidatableObject
    {
        public NotificationType NotificationType { get; set; }

        public DateTime MovementDate { get; set; }

        public DateTime ReceiptDate { get; set; }

        [Required]
        public int? Day { get; set; }
        [Required]
        public int? Month { get; set; }
        [Required]
        public int? Year { get; set; }

        public DateViewModel()
        {
        }

        public DateViewModel(OperationCompleteData data)
        {
            NotificationType = data.NotificationType;
            MovementDate = data.MovementDate;
            ReceiptDate = data.ReceiptDate;
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

            if (dateComplete < ReceiptDate)
            {
                yield return new ValidationResult("This date cannot be before the date of receipt. Please enter a different date.", new[] { "Day" });
            }

            if (dateComplete < MovementDate)
            {
                yield return new ValidationResult("This date cannot be before the actual date of shipment. Please enter a different date.", new[] { "Day" });
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