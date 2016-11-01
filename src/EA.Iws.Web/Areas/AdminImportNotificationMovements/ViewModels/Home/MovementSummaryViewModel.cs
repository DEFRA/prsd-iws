namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.ViewModels.Home
{
    using System;
    using System.Collections.Generic;
    using Core.ImportNotificationMovements;
    using Core.NotificationAssessment;
    using Core.Shared;
    using Prsd.Core.Helpers;

    public class MovementSummaryViewModel
    {
        public Guid NotificationId { get; set; }

        public string NotificationNumber { get; set; }

        public NotificationType NotificationType { get; set; }

        public int IntendedShipments { get; set; }

        public int UsedShipments { get; set; }

        public string QuantityRemainingTotal { get; set; }

        public string QuantityReceivedTotal { get; set; }

        public int ActiveLoadsPermitted { get; set; }

        public int ActiveLoadsCurrent { get; set; }

        public List<MovementsSummaryTableViewModel> TableData { get; set; }

        public MovementSummaryViewModel()
        {
        }

        public MovementSummaryViewModel(Summary data)
        {
            NotificationId = data.Id;
            NotificationNumber = data.NotificationNumber;
            NotificationType = data.NotificationType;
            IntendedShipments = data.IntendedShipments;
            UsedShipments = data.UsedShipments;
            QuantityReceivedTotal = data.QuantityReceivedTotal.ToString("G29") + " " + EnumHelper.GetDisplayName(data.DisplayUnit);
            QuantityRemainingTotal = data.QuantityRemainingTotal.ToString("G29") + " " + EnumHelper.GetDisplayName(data.DisplayUnit);
            TableData = new List<MovementsSummaryTableViewModel>();
        }

        public bool ShowShipmentOptions()
        {
            return true;
        }

        public bool ShowShipments()
        {
            return TableData != null && TableData.Count > 0;
        }
    }
}