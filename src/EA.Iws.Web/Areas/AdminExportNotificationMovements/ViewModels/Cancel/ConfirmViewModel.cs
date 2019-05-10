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
            SelectedShipmentNumbers =
                (selectedMovements.Select(x => x.Number).Concat(addedMovements.Select(x => x.Number))).OrderBy(x => x);
        }

        public Guid NotificationId { get; set; }

        public IEnumerable<int> SelectedShipmentNumbers { get; set; }
    }
}