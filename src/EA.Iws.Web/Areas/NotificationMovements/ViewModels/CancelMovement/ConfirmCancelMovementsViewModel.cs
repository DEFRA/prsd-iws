namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.CancelMovement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ConfirmCancelMovementsViewModel
    {
        public ConfirmCancelMovementsViewModel()
        {
        }

        public ConfirmCancelMovementsViewModel(Guid notificationId, IEnumerable<CancelMovementsList> result)
        {
            NotificationId = notificationId;
            ListForCancelMovement = result.OrderBy(m => m.Number).ToList();
        }

        public Guid NotificationId { get; set; }

        public IEnumerable<CancelMovementsList> ListForCancelMovement { get; set; }
    }
}