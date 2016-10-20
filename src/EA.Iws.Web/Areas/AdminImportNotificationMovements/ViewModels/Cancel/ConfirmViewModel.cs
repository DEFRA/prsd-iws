namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.ViewModels.Cancel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.ImportMovement;

    public class ConfirmViewModel
    {
        public ConfirmViewModel()
        {
        }

        public ConfirmViewModel(Guid notificationId, IEnumerable<ImportCancelMovementData> result)
        {
            NotificationId = notificationId;
            SelectedMovements = result.OrderBy(m => m.Number).ToList();
        }

        public Guid NotificationId { get; set; }

        public IEnumerable<ImportCancelMovementData> SelectedMovements { get; set; }
    }
}