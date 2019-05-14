namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.ViewModels.Cancel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Core.Movement;

    public class AddViewModel : IValidatableObject
    {
        private const int MinShipmentNumber = 1;
        private const int MaxShipmentNumber = 999999;

        [Required(ErrorMessageResourceName = "AddShipmentNumberRequired", ErrorMessageResourceType = typeof(CancelResources))]
        public string NewShipmentNumber { get; set; }

        public int ShipmentNumber
        {
            get
            {
                int result;
                int.TryParse(NewShipmentNumber, out result);
                return result;
            }
        }

        [Display(Name = "Date")]
        [Required(ErrorMessageResourceName = "AddActualDateOfShipmentRequired", ErrorMessageResourceType = typeof(CancelResources))]
        public DateTime? NewActualShipmentDate { get; set; }

        public IList<AddedCancellableMovement> AddedMovements { get; set; }

        public AddViewModel(IEnumerable<AddedCancellableMovement> addedMovements)
        {
            AddedMovements = addedMovements.OrderBy(x => x.Number).ToList();
        }

        public AddViewModel()
        {
            AddedMovements = new List<AddedCancellableMovement>();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            int parsedShipmentNumber;
            if (!int.TryParse(NewShipmentNumber, out parsedShipmentNumber) || parsedShipmentNumber < MinShipmentNumber ||
                parsedShipmentNumber > MaxShipmentNumber)
            {
                yield return new ValidationResult(CancelResources.AddActualDateOfShipmentRequired, new[] { "NewActualShipmentDate" });
            }
        }
    }
}