namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Create
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class ThreeOrLessDaysViewModel : IValidatableObject
    {
        public ThreeOrLessWorkingDaysSelection Selection { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!(Selection == ThreeOrLessWorkingDaysSelection.ContinueAnyway || Selection == ThreeOrLessWorkingDaysSelection.AbandonCreation))
            {
                yield return new ValidationResult("Select one of the options", new[] { "Selection" });
            }
        }
    }
}