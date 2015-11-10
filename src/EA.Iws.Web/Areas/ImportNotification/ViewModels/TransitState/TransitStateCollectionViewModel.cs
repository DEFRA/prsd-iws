namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.TransitState
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.TransitState;

    public class TransitStateCollectionViewModel : IValidatableObject
    {
        [Display(Name = "HasNoTransitStates", ResourceType = typeof(TransitStateCollectionViewModelResources))]
        public bool HasNoTransitStates { get; set; }

        public List<TransitStateData> TransitStates { get; set; }

        public TransitStateCollectionViewModel()
        {
            TransitStates = new List<TransitStateData>();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (HasNoTransitStates && TransitStates.Count > 0)
            {
                yield return new ValidationResult(TransitStateCollectionViewModelResources.ValidationMessage, new[] { "HasNoTransitStates" });
            }
        }
    }
}