namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.ViewModels.Cancel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Core.Movement;

    public class AddViewModel : IValidatableObject
    {
        [Required(ErrorMessageResourceName = "AddShipmentNumberRequired", ErrorMessageResourceType = typeof(CancelResources))]
        [Range(1, int.MaxValue, ErrorMessage = null, ErrorMessageResourceName = "AddValidShipmentNumber", ErrorMessageResourceType = typeof(CancelResources))]
        public int? NewShipmentNumber { get; set; }

        [Required(ErrorMessageResourceName = "AddActualDateOfShipmentRequired", ErrorMessageResourceType = typeof(CancelResources))]
        public DateTime? NewActualShipmentDate { get; set; }

        public IList<AddedCancellableMovement> AddedMovements { get; set; }

        public AddViewModel(IEnumerable<AddedCancellableMovement> addedMovements)
        {
            AddedMovements = addedMovements.ToList();
        }

        public AddViewModel()
        {
            AddedMovements = new List<AddedCancellableMovement>();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!NewActualShipmentDate.HasValue)
            {
                yield return new ValidationResult(CancelResources.AddActualDateOfShipmentRequired, new[] { "NewActualShipmentDate" });
            }
        }
    }
}