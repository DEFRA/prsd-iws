namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteType
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.WasteType;
    using Requests.WasteType;

    public class ChemicalCompositionConcentrationLevelsViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        public List<WasteTypeCompositionData> WasteComposition { get; set; }

        public List<WasteTypeCompositionData> OtherCodes { get; set; }

        public string Command { get; set; }

        public string Description { get; set; }

        public ChemicalCompositionType ChemicalCompositionType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ChemicalCompositionType == ChemicalCompositionType.Wood && string.IsNullOrEmpty(Description))
            {
                yield return new ValidationResult("Description is required");
            }
        }
    }
}