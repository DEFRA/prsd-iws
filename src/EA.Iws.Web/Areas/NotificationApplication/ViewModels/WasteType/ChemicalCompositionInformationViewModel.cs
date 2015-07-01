namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteType
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.WasteType;
    using Requests.WasteType;

    public class ChemicalCompositionInformationViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        public List<WoodInformationData> WasteComposition { get; set; }

        public string FurtherInformation { get; set; }

        public string Energy { get; set; }

        public ChemicalCompositionType ChemicalCompositionType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Energy))
            {
                yield return new ValidationResult("Please enter a value for Energy");
            }
        }
    }
}