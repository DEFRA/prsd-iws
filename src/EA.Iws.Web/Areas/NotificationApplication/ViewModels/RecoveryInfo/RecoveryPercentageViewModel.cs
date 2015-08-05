namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.RecoveryInfo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.Notification;
    using Prsd.Core.Validation;
    using Requests.Notification;

    public class RecoveryPercentageViewModel : IValidatableObject
    {
        public RecoveryPercentageViewModel()
        {
        }

        public RecoveryPercentageViewModel(RecoveryPercentageData recoveryPercentageData)
        {
            NotificationId = recoveryPercentageData.NotificationId;
            IsProvidedByImporter = recoveryPercentageData.IsProvidedByImporter.GetValueOrDefault();

            if (IsProvidedByImporter)
            {
                PercentageRecoverable = null;
            }

            if (recoveryPercentageData.PercentageRecoverable != null)
            {
                PercentageRecoverable = recoveryPercentageData.PercentageRecoverable;
                IsProvidedByImporter = false;
            }
        }

        public SetRecoveryPercentageData ToRequest()
        {
            if (IsProvidedByImporter)
            {
                return new SetRecoveryPercentageData(NotificationId, true, null, null);
            }

            if (PercentageRecoverable.HasValue)
            {
                return new SetRecoveryPercentageData(NotificationId, false, PercentageRecoverable.Value, null);
            }

            throw new InvalidOperationException("Recovery percentage data cannot be set without method of disposal, when recovery percentage is less than 100.");
        }

        public Guid NotificationId { get; set; }

        [Display(Name = "I confirm that the relevant information will be provided directly to the competent authorities involved by the importer-consignee.")]
        [RequiredIf("HasPercentageRecoverableValue", false, "Please enter the percentage (%) of recoverable material or check the box")]
        public bool IsProvidedByImporter { get; set; }

        [Range(0, 100, ErrorMessage = "The percentage (%) of recoverable material must be between 0 and 100")]
        [Display(Name = "Enter the percentage (%) of recoverable material")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "The percentage (%) of recoverable material must be a number with a maximum of 2 decimal places.")]
        [RequiredIf("IsProvidedByImporter", false, "Please enter the percentage (%) of recoverable material or check the box")]
        public decimal? PercentageRecoverable { get; set; }

        public bool HasPercentageRecoverableValue
        {
            get { return PercentageRecoverable.HasValue; }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var percentageRecoverable = PercentageRecoverable.GetValueOrDefault();

            if (!IsProvidedByImporter)
            {
                if (PercentageRecoverable == null)
                {
                    yield return new ValidationResult("Please provide the percentage of the material that is recoverable", new[] { "PercentageRecoverable" });
                }

                if (PercentageRecoverable == null || percentageRecoverable > 100M || percentageRecoverable < 0M)
                {
                    yield return new ValidationResult("The percentage (%) of recoverable material must be from 0 to 100", new[] { "PercentageRecoverable" });
                }
            }
            if (PercentageRecoverable != null)
            {
                if (IsProvidedByImporter)
                {
                    yield return new ValidationResult("Please enter either the percentage (%) of recoverable material or check the box", new[] { "PercentageRecoverable" });
                }
            }
        }
    }
}