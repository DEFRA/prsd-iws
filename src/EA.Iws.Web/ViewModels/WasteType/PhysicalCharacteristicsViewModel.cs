namespace EA.Iws.Web.ViewModels.WasteType
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.UI.WebControls;
    using Prsd.Core.Validation;
    using Requests.WasteType;
    using Shared;

    public class PhysicalCharacteristicsViewModel : IValidatableObject
    {
        public CheckBoxCollectionViewModel PhysicalCharacteristics { get; set; }

        public Guid NotificationId { get; set; }

        public bool OtherSelected { get; set; }

        [RequiredIf("OtherSelected", true, "Please enter a description")]
        public string OtherDescription { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (!PhysicalCharacteristics.PossibleValues.Any(p => p.Selected) && !OtherSelected)
            {
                results.Add(new ValidationResult("Please select at least one option"));
            }
            return results;
        }
    }
}