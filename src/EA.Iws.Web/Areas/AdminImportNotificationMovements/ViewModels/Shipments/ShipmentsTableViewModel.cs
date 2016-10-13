namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.ViewModels.Shipments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.ImportNotificationMovements;
    using Core.Shared;

    public class ShipmentsTableViewModel
    {
        public ShipmentsTableViewModel(MovementsSummary data)
        {
            ImportNotificationId = data.ImportNotificationId;
            NotificationType = data.NotificationType;
            TableData = data.TableData.Select(d => new TableDataViewModel(d)).ToList();
        }

        public Guid ImportNotificationId { get; set; }

        public NotificationType NotificationType { get; set; }

        public List<TableDataViewModel> TableData { get; set; }

        public bool ShowShipments()
        {
            return true;
        }
    }
}