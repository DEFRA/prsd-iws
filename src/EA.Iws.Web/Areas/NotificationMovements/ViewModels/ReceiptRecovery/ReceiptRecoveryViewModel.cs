namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.ReceiptRecovery
{
    using Core.Movement;
    using Core.MovementOperation;
    using Core.Shared;
    using Infrastructure.Validation;
    using Prsd.Core;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using Web.ViewModels.Shared;
    public class ReceiptRecoveryViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        public Guid SelectedmovementId { get; set; }

        public NotificationType? NotificationType { get; set; }

        public CertificateType Certificate { get; set; }

        public bool? IsSameAsReceiptDate { get; set; }

        [Required(ErrorMessageResourceName = "InvalidDay", ErrorMessageResourceType = typeof(ReceiptRecoveryViewModelResources))]
        [Range(1, 31, ErrorMessageResourceName = "InvalidDay", ErrorMessageResourceType = typeof(ReceiptRecoveryViewModelResources))]
        public int? Day { get; set; }

        [Required(ErrorMessageResourceName = "InvalidMonth", ErrorMessageResourceType = typeof(ReceiptRecoveryViewModelResources))]
        [Range(1, 12, ErrorMessageResourceName = "InvalidMonth", ErrorMessageResourceType = typeof(ReceiptRecoveryViewModelResources))]
        public int? Month { get; set; }

        [Required(ErrorMessageResourceName = "InvalidYear", ErrorMessageResourceType = typeof(ReceiptRecoveryViewModelResources))]
        [Range(2015, 3000, ErrorMessageResourceName = "InvalidYear", ErrorMessageResourceType = typeof(ReceiptRecoveryViewModelResources))]
        public int? Year { get; set; }

        [Display(Name = "Quantity", ResourceType = typeof(ReceiptRecoveryViewModelResources))]
        [Required(ErrorMessageResourceName = "QuantityReceivedRequired", ErrorMessageResourceType = typeof(ReceiptRecoveryViewModelResources))]
        [IsValidNumber(maxPrecision: 18, NumberStyle = NumberStyles.AllowDecimalPoint, ErrorMessageResourceName = "QuantityIsValid", ErrorMessageResourceType = typeof(ReceiptRecoveryViewModelResources))]
        public string Quantity { get; set; }
        public ShipmentQuantityUnits Unit { get; set; }

        [Range(1, 31, ErrorMessageResourceName = "InvalidDay", ErrorMessageResourceType = typeof(ReceiptRecoveryViewModelResources))]
        public int? RecoveryDay { get; set; }

        [Range(1, 12, ErrorMessageResourceName = "InvalidMonth", ErrorMessageResourceType = typeof(ReceiptRecoveryViewModelResources))]
        public int? RecoveryMonth { get; set; }

        [Range(2015, 3000, ErrorMessageResourceName = "InvalidYear", ErrorMessageResourceType = typeof(ReceiptRecoveryViewModelResources))]
        public int? RecoveryYear { get; set; }

        public DateTime ReceiptDate { get; set; }

        public ReceiptRecoveryViewModel()
        {
                IsSameAsReceiptDate = true;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            ValidateReceiptData();
            DateTime dateReceived;
            if (ParseDateInput(out dateReceived))
            {
                ReceiptDate = dateReceived;
            }

            return ValidateRecoveryData();
        }

        private IEnumerable<ValidationResult> ValidateReceiptData()
        {
            decimal quantity = Convert.ToDecimal(Quantity);

            if (quantity <= 0)
            {
                yield return new ValidationResult(ReceiptRecoveryViewModelResources.QuantityReceivedPositive, new[] { "Quantity" });
            }

            if (decimal.Round(quantity, ShipmentQuantityUnitsMetadata.Precision[Unit]) != quantity)
            {
                yield return new ValidationResult(
                    string.Format(ReceiptRecoveryViewModelResources.QuantityReceivedPrecision, ShipmentQuantityUnitsMetadata.Precision[Unit] + 1),
                    new[] { "Quantity" });
            }

            DateTime dateReceived;
            if (!ParseDateInput(out dateReceived))
            {
                yield return new ValidationResult("Please enter a valid date", new[] { "Day" });
            }
            else if (dateReceived.Date > SystemTime.UtcNow.Date)
            {
                yield return new ValidationResult("This date cannot be in the future. Please enter a different date.", new[] { "Day" });
            }
        }

        private IEnumerable<ValidationResult> ValidateRecoveryData()
        {
            if (!IsSameAsReceiptDate.Value)
            {
                if (RecoveryDay == null)
                {
                    yield return new ValidationResult(ReceiptRecoveryViewModelResources.InvalidDay, new[] { "RecoveryDay" });
                }
                if (RecoveryMonth == null)
                {
                    yield return new ValidationResult(ReceiptRecoveryViewModelResources.InvalidMonth, new[] { "RecoveryMonth" });
                }
                if (RecoveryYear == null)
                {
                    yield return new ValidationResult(ReceiptRecoveryViewModelResources.InvalidYear, new[] { "RecoveryYear" });
                }
            }
            DateTime dateComplete;
            if (!ParseCompleteDateInput(out dateComplete))
            {
                yield return new ValidationResult("Please enter a valid date", new[] { "RecoveryDay" });
            }
            if (dateComplete > SystemTime.UtcNow)
            {
                yield return new ValidationResult("This date cannot be in the future. Please enter a different date.", new[] { "RecoveryDay" });
            }

            DateTime dateReceived;
            if (ParseDateInput(out dateReceived))
            {
                if (dateComplete < dateReceived)
                {
                    yield return new ValidationResult("This date cannot be before the date of receipt. Please enter a different date.", new[] { "RecoveryDay" });
                }
            }           
        }

        private bool ParseDateInput(out DateTime dateReceived)
        {
            return SystemTime.TryParse(
                Year.GetValueOrDefault(),
                Month.GetValueOrDefault(),
                Day.GetValueOrDefault(),
                out dateReceived);
        }

        private bool ParseCompleteDateInput(out DateTime dateComplete)
        {
            if (!IsSameAsReceiptDate.Value)
            {
                return SystemTime.TryParse(
                   RecoveryYear.GetValueOrDefault(),
                    RecoveryMonth.GetValueOrDefault(),
                    RecoveryDay.GetValueOrDefault(),
                    out dateComplete);
            }
            else
            {
                return ParseDateInput(out dateComplete);
            }
        }

        public DateTime GetDateReceived()
        {
            DateTime dateReceived;
            if (ParseDateInput(out dateReceived))
            {
                return dateReceived;
            }
            else
            {
                throw new InvalidOperationException("Date not valid");
            }
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