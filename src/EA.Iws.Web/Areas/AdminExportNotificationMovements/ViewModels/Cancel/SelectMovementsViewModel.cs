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
        }

        public SelectMovementsViewModel(List<SubmittedMovement> result)
        {
            SubmittedMovements = result.OrderByDescending(m => m.ShipmentDate).ToList();
        }

        public List<SubmittedMovement> SubmittedMovements { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!SubmittedMovements.Any(m => m.IsSelected))
            {
                yield return new ValidationResult("Please select at least one prenotification to cancel", new[] { "SubmittedMovements" });
            }
        }
    }
}