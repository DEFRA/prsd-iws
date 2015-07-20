namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.RecoveryInfo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.Notification;
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

            if (recoveryPercentageData.PercentageRecoverable != null)
            {
                if (recoveryPercentageData.PercentageRecoverable == 100.00M)
                {
                    IsHundredPercentRecoverable = true;
                    PercentageRecoverable = null;
                }
                else
                {
                    IsHundredPercentRecoverable = false;
                    PercentageRecoverable = recoveryPercentageData.PercentageRecoverable;
                }
               
                MethodOfDisposal = recoveryPercentageData.MethodOfDisposal;
            }

            if (IsProvidedByImporter)
            {
                PercentageRecoverable = null;
                MethodOfDisposal = null;
                IsHundredPercentRecoverable = null;
            }
        }

        public SetRecoveryPercentageData ToRequest()
        {
            var percentage = PercentageRecoverable;
            var isHundredPercentRecoverable = IsHundredPercentRecoverable;

            if (!isHundredPercentRecoverable.HasValue)
            {
                isHundredPercentRecoverable = false;
            }

            if ((bool)isHundredPercentRecoverable)
            {
                percentage = 100.00M;
            }

            if (IsProvidedByImporter)
            {
                percentage = null;
                MethodOfDisposal = null;
            }

            return new SetRecoveryPercentageData(NotificationId, IsProvidedByImporter,
                MethodOfDisposal, percentage);
        }

        public Guid NotificationId { get; set; }

        [Display(Name = "I confirm that the relevant information will be provided directly to the competent authorities involved by the importer-consignee.")]
        public bool IsProvidedByImporter { get; set; }

        public bool? IsHundredPercentRecoverable { get; set; }

        [Display(Name = "Enter the percentage (%) of recoverable material")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "The percentage (%) of recoverable material must be a number with a maximum of 2 decimal places.")]
        public decimal? PercentageRecoverable { get; set; }

        public decimal? PercentageNonRecoverable
        {
            get
            {
                return Decimal.Round(100 - (PercentageRecoverable ?? 0), 2, MidpointRounding.AwayFromZero);
            }
        }

        [Display(Name = "Planned method of disposal of the non-recoverable waste after recovery")]
        public string MethodOfDisposal { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsHundredPercentRecoverable == false)
            {
                var percentageRecoverable = PercentageRecoverable.GetValueOrDefault();

                if (PercentageRecoverable == null || percentageRecoverable > 99.99M || percentageRecoverable <= 0.00M)
                {
                    yield return new ValidationResult("The percentage (%) of recoverable material must be between 0 and 99.99", new[] { "PercentageRecoverable" });
                }
            }
            if (!IsProvidedByImporter)
            {
                if (IsHundredPercentRecoverable == null)
                {
                    yield return new ValidationResult("Please answer this question", new[] { "IsHundredPercentRecoverable" });
                }

                if (IsHundredPercentRecoverable == false)
                {
                    if (PercentageRecoverable == null)
                    {
                        yield return new ValidationResult("Please provide the percentage of the material that is recoverable", new[] { "PercentageRecoverable" });
                    }

                    if (string.IsNullOrEmpty(MethodOfDisposal))
                    {
                        yield return new ValidationResult("Please provide details of the method of disposal for the non recoverable waste", new[] { "MethodOfDisposal" });
                    }
                }
            }
        }
    }
}