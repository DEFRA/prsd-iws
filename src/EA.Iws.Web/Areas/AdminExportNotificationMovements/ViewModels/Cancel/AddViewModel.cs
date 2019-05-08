namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.Cancel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Core.Movement;
    using Web.ViewModels.Shared;

    public class AddViewModel
    {
        [Required(ErrorMessage = "Please enter a shipment number")]
        public int? NewShipmentNumber { get; set; }

        [Required(ErrorMessage = "Please enter a date of shipment")]
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
    }
}