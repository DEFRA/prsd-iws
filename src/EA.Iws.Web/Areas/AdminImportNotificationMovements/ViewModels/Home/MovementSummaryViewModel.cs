namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.ViewModels.Home
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.ImportNotificationMovements;
    using Core.Shared;

    public class MovementSummaryViewModel
    {
        public Guid NotificationId { get; set; }

        public string NotificationNumber { get; set; }

        public NotificationType NotificationType { get; set; }

        public IList<MovementSummaryTableRowViewModel> Movements { get; set; }

        public MovementSummaryViewModel()
        {
            Movements = new List<MovementSummaryTableRowViewModel>();
        }

        public MovementSummaryViewModel(Summary data)
        {
            Movements = new List<MovementSummaryTableRowViewModel>(data.Movements.Select(m => new MovementSummaryTableRowViewModel(m)));
            NotificationId = data.Id;
            NotificationNumber = data.NotificationNumber;
            NotificationType = data.NotificationType;
        }
    }
}