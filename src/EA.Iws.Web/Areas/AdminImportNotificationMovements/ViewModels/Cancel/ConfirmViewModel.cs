namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.ViewModels.Cancel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.ImportMovement;
    using Core.Movement;

    public class ConfirmViewModel
    {
        public ConfirmViewModel()
        {
        }

        public ConfirmViewModel(Guid notificationId, IEnumerable<ImportCancelMovementData> result, IEnumerable<AddedCancellableMovement> addedMovements)
        {
            NotificationId = notificationId;
            var shipmentNumbers = result.Select(m => m.Number).ToList();
            shipmentNumbers.AddRange(addedMovements.Select(x => x.Number));

            SelectedShipmentNumbers = shipmentNumbers.OrderBy(x => x);
        }

        public Guid NotificationId { get; set; }

        public IEnumerable<int> SelectedShipmentNumbers { get; set; }

        public IEnumerable<ImportCancelMovementData> SelectedMovements { get; set; }
    }
}