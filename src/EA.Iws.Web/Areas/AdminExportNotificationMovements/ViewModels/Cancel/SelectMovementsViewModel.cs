namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.Cancel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Core.Movement;

    public class SelectMovementsViewModel : IValidatableObject
    {
        public SelectMovementsViewModel()
        {
            SubmittedMovements = new List<SubmittedMovement>();
            AddedMovements = new List<AddedCancellableMovement>();
        }

        public SelectMovementsViewModel(IEnumerable<SubmittedMovement> result,
            IEnumerable<AddedCancellableMovement> addedMovements)
        {
            SubmittedMovements = result.OrderBy(x => x.Number).ToList();
            AddedMovements = addedMovements.OrderBy(x => x.Number).ToList();
        }

        public List<SubmittedMovement> SubmittedMovements { get; set; }

        public IList<AddedCancellableMovement> AddedMovements { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!SubmittedMovements.Any(m => m.IsSelected))
            {
                yield return new ValidationResult("Please select at least one prenotification to cancel", new[] { "SubmittedMovements" });
            }
        }
    }
}