namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Create
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ThreeWorkingDaysWarningViewModel : IValidatableObject
    {
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