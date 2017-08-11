namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.ReceiptRecovery
{
    using Core.Shared;
    using Prsd.Core;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class RecoveryViewModel : IValidatableObject
    {
        public NotificationType NotificationType { get; set; }

        public Guid NotificationId { get; set; }

        public IEnumerable<Guid> SelectedmovementIds { get; set; }

        public CertificateType Certificate { get; set; }

        [Required(ErrorMessageResourceName = "InvalidDay", ErrorMessageResourceType = typeof(ReceiptRecoveryViewModelResources))]
        [Range(1, 31, ErrorMessageResourceName = "InvalidDay", ErrorMessageResourceType = typeof(ReceiptRecoveryViewModelResources))]
        public int? Day { get; set; }

        [Required(ErrorMessageResourceName = "InvalidMonth", ErrorMessageResourceType = typeof(ReceiptRecoveryViewModelResources))]
        [Range(1, 12, ErrorMessageResourceName = "InvalidMonth", ErrorMessageResourceType = typeof(ReceiptRecoveryViewModelResources))]
        public int? Month { get; set; }

        [Required(ErrorMessageResourceName = "InvalidYear", ErrorMessageResourceType = typeof(ReceiptRecoveryViewModelResources))]
        [Range(2015, 3000, ErrorMessageResourceName = "InvalidYear", ErrorMessageResourceType = typeof(ReceiptRecoveryViewModelResources))]
        public int? Year { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            DateTime dateComplete;
            if (!ParseCompleteDateInput(out dateComplete))
            {
                yield return new ValidationResult("Please enter a valid date", new[] { "Recovery.Day" });
            }
            if (dateComplete > SystemTime.UtcNow)
            {
                yield return new ValidationResult("This date cannot be in the future. Please enter a different date.", new[] { "Recovery.Day" });
            }
        }

        private bool ParseCompleteDateInput(out DateTime dateComplete)
        {
           return ParseDateInput(out dateComplete);            
        }

        private bool ParseDateInput(out DateTime dateReceived)
        {
            return SystemTime.TryParse(
                Year.GetValueOrDefault(),
                Month.GetValueOrDefault(),
                Day.GetValueOrDefault(),
                out dateReceived);
        }
        public DateTime GetDateRecovered()
        {
            DateTime dateComplete;
            if (ParseCompleteDateInput(out dateComplete))
            {
                return dateComplete;
            }
            else
            {
                throw new InvalidOperationException("Date not valid");
            }
        }
    }
}