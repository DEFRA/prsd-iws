namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.Cancel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Movement;

    public class ConfirmViewModel
    {
        public ConfirmViewModel()
        {
        }

        public ConfirmViewModel(Guid notificationId, IEnumerable<MovementData> selectedMovements,
            IEnumerable<AddedCancellableMovement> addedMovements)
        {
            NotificationId = notificationId;

            var shipmentNumbers = selectedMovements.Select(m => m.Number).ToList();
            shipmentNumbers.AddRange(addedMovements.Select(x => x.Number));

            SelectedShipmentNumbers = shipmentNumbers.OrderBy(x => x);
        }

        public Guid NotificationId { get; set; }

        public IEnumerable<int> SelectedShipmentNumbers { get; set; }
    }
}