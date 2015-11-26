namespace EA.Iws.Web.Areas.AdminNotificationMovements.ViewModels.Cancel
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

        public ConfirmViewModel(Guid notificationId, IEnumerable<MovementData> result)
        {
            NotificationId = notificationId;
            SelectedMovements = result.OrderBy(m => m.Number).ToList();
        }

        public Guid NotificationId { get; set; }

        public IEnumerable<MovementData> SelectedMovements { get; set; }
    }
}