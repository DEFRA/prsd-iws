namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.PhysicalCharacteristics
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Prsd.Core.Validation;
    using Views.PhysicalCharacteristics;
    using Web.ViewModels.Shared;

    public class PhysicalCharacteristicsViewModel : IValidatableObject
    {
        public CheckBoxCollectionViewModel PhysicalCharacteristics { get; set; }

        public Guid NotificationId { get; set; }

        public bool OtherSelected { get; set; }

        [RequiredIf("OtherSelected", true, ErrorMessageResourceName = "DescriptionRequired", ErrorMessageResourceType = typeof(PhysicalCharacteristicsResources))]
        public string OtherDescription { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (!PhysicalCharacteristics.PossibleValues.Any(p => p.Selected) && !OtherSelected)
            {
                results.Add(new ValidationResult(PhysicalCharacteristicsResources.PhysicalCharRequired));
            }
            return results;
        }
    }
}