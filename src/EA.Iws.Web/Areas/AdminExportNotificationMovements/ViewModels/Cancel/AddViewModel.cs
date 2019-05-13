namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.Cancel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Core.Movement;
    using Infrastructure.Validation;

    public class AddViewModel
    {
        private const int MinShipmentNumber = 1;
        private const int MaxShipmentNumber = 999999;

        [Required(ErrorMessageResourceName = "ShipmentNumberRequired", ErrorMessageResourceType = typeof(CancelViewModelResources))]
        [Range(MinShipmentNumber, MaxShipmentNumber, ErrorMessageResourceName = "ShipmentNumberInvalid", ErrorMessageResourceType = typeof(CancelViewModelResources))]
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
        [Required(ErrorMessageResourceName = "ActualDateOfShipmentRequired", ErrorMessageResourceType = typeof(CancelViewModelResources))]
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
    }
}