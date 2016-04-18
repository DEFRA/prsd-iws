namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.CancelMovement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Movement;

    public class ConfirmCancelMovementsViewModel
    {
        public ConfirmCancelMovementsViewModel()
        {
        }

        public ConfirmCancelMovementsViewModel(Guid notificationId, IEnumerable<MovementData> result)
        {
            NotificationId = notificationId;
            SelectedMovements = result.OrderBy(m => m.Number).ToList();
        }

        public Guid NotificationId { get; set; }

        public IEnumerable<MovementData> SelectedMovements { get; set; }
    }
}