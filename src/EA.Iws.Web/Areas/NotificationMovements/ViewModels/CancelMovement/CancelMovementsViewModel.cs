namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.CancelMovement
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Core.Movement;

    public class CancelMovementsViewModel : IValidatableObject
    {
        public CancelMovementsViewModel()
        {
        }

        public CancelMovementsViewModel(Guid notificationId, List<SubmittedMovement> result)
        {
            NotificationId = notificationId;
            SubmittedMovements = result.OrderByDescending(m => m.ShipmentDate).ToList();
        }

        public Guid NotificationId { get; set; }

        public List<SubmittedMovement> SubmittedMovements { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!SubmittedMovements.Any(m => m.IsSelected))
            {
                yield return new ValidationResult("Please select at least one pre-notification to cancel", new[] { "SubmittedMovements" });
            }
        }
    }
}