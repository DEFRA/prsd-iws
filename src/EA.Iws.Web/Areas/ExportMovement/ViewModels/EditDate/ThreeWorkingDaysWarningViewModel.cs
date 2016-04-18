namespace EA.Iws.Web.Areas.ExportMovement.ViewModels.EditDate
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ThreeWorkingDaysWarningViewModel : IValidatableObject
    {
        public ThreeWorkingDaysWarningViewModel()
        {
        }

        public ThreeWorkingDaysWarningViewModel(DateTime shipmentDate)
        {
            ShipmentDate = shipmentDate;
        }

        public DateTime ShipmentDate { get; set; }
        
        public ThreeWorkingDaysSelection Selection { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!(Selection == ThreeWorkingDaysSelection.ContinueAnyway || Selection == ThreeWorkingDaysSelection.ChangeDate))
            {
                yield return new ValidationResult(ThreeWorkingDaysWarningViewModelResources.SelectionHasBeenMadeValidation, new[] { "Selection" });
            }
        }
    }
}