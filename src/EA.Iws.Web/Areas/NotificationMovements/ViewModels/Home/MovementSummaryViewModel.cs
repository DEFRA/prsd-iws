namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Home
{
    using System;
    using System.Collections.Generic;
    using Core.Movement;
    using Core.Shared;
    using Home;
    using Prsd.Core.Helpers;

    public class MovementSummaryViewModel
    {
        public Guid NotificationId { get; set; }

        public string NotificationNumber { get; set; }

        public NotificationType NotificationType { get; set; }

        public int IntendedShipments { get; set; }

        public int UsedShipments { get; set; }

        public string QuantityIntendedTotal { get; set; }

        public string QuantityReceivedTotal { get; set; }

        public int ActiveLoadsPermitted { get; set; }

        public int ActiveLoadsCurrent { get; set; }

        public List<MovementSummaryTableViewModel> TableData { get; set; }

        public MovementSummaryViewModel(Guid notificationId, MovementSummaryData data, List<MovementSummaryTableViewModel> tableData)
        {
            NotificationId = notificationId;
            NotificationNumber = data.NotificationNumber;
            NotificationType = data.NotificationType;
            IntendedShipments = data.IntendedShipments;
            UsedShipments = data.UsedShipments;
            QuantityIntendedTotal = data.IntendedQuantityTotal.ToString("G29") + " " + EnumHelper.GetDisplayName(data.DisplayUnits);
            QuantityReceivedTotal = data.ReceivedQuantityTotal.ToString("G29") + " " + EnumHelper.GetDisplayName(data.DisplayUnits);
            ActiveLoadsPermitted = data.ActiveLoadsPermitted;
            ActiveLoadsCurrent = data.ActiveLoadsCurrent;
            TableData = tableData;
        }
    }
}