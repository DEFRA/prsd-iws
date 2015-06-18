namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.RecoveryInfo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Prsd.Core.Validation;

    public class RecoveryPercentageViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        [Display(Name = "Recovery information will be provided by the importer-consignee")]
        public bool IsProvidedByImporter { get; set; }

        public bool? IsHundredPercentRecoverable { get; set; }

        [Display(Name = "Enter the percentage (%) of recoverable material")]
        [Range(0, 100)]
        public decimal? PercentageRecoverable { get; set; }

        public decimal? PercentageNonRecoverable
        {
            get { return 100 - this.PercentageRecoverable; }
        }

        [Display(Name = "Planned method of disposal of the non-recoverable waste after recovery")]
        public string MethodOfDisposal { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsProvidedByImporter == false)
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