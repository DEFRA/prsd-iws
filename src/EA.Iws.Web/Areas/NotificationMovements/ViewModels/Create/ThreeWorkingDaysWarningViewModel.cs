namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Create
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Prsd.Core;
    using Web.ViewModels.Shared;

    public class ThreeWorkingDaysWarningViewModel : IValidatableObject
    {
        public ThreeWorkingDaysWarningViewModel()
        {
            DateInput = new OptionalDateInputViewModel();
        }

        public ThreeWorkingDaysWarningViewModel(int movementNumber)
        {
            MovementNumber = movementNumber;
            DateInput = new OptionalDateInputViewModel();
        }

        public int MovementNumber { get; set; }
        
        public ThreeWorkingDaysSelection Selection { get; set; }

        [Display(Name = "DateInputDisplayName", ResourceType = typeof(ThreeWorkingDaysWarningViewModelResources))]
        public OptionalDateInputViewModel DateInput { get; set; }

        public string DateHintText
        {
            get
            {
                var dateString = SystemTime.Now.Day + " " + SystemTime.Now.Month + " " + SystemTime.Now.Year;

                return "For example, " + dateString;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!(Selection == ThreeWorkingDaysSelection.ContinueAnyway || Selection == ThreeWorkingDaysSelection.ChangeDate))
            {
                yield return new ValidationResult(ThreeWorkingDaysWarningViewModelResources.SelectionHasBeenMadeValidation, new[] { "Selection" });
            }
        }
    }
}