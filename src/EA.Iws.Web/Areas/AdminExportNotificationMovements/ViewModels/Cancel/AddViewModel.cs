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
        private const int MinShipmentNumber = 1;
        private const int MaxShipmentNumber = 999999;

        [Required(ErrorMessageResourceName = "ShipmentNumberRequired", ErrorMessageResourceType = typeof(CancelViewModelResources))]
        public int? NewShipmentNumber { get; set; }

        [Required(ErrorMessageResourceName = "ActualDateOfShipmentRequired", ErrorMessageResourceType = typeof(CancelViewModelResources))]
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
            if (!NewShipmentNumber.HasValue || NewShipmentNumber.Value < MinShipmentNumber || NewShipmentNumber.Value > MaxShipmentNumber)
            {
                yield return new ValidationResult(CancelViewModelResources.ShipmentNumberInvalid, new[] { "NewShipmentNumber" });
            }

            if (!NewActualShipmentDate.HasValue)
            {
                yield return new ValidationResult(CancelViewModelResources.ActualDateOfShipmentRequired, new[] { "NewActualShipmentDate" });
            }
        }
    }
}