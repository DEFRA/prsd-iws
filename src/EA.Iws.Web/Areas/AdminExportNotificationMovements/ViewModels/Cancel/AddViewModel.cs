namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.Cancel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Core.Movement;
    using Web.ViewModels.Shared;

    public class AddViewModel : IValidatableObject
    {
        [Required(ErrorMessageResourceName = "AddShipmentNumberRequired", ErrorMessageResourceType = typeof(CancelViewModelResources))]
        [Range(1, int.MaxValue, ErrorMessage = null, ErrorMessageResourceName = "AddValidShipmentNumber", ErrorMessageResourceType = typeof(CancelViewModelResources))]
        public int? NewShipmentNumber { get; set; }

        [Required(ErrorMessageResourceName = "AddActualDateOfShipmentRequired", ErrorMessageResourceType = typeof(CancelViewModelResources))]
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
                yield return new ValidationResult(CancelViewModelResources.AddActualDateOfShipmentRequired, new[] { "NewActualShipmentDate" });
            }
        }
    }
}