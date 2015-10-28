namespace EA.Iws.Web.Areas.NotificationAssessment.ViewModels.ShipmentSummary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Movement;
    using Core.Shared;
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

        public MovementStatus? SelectedMovementStatus { get; set; }

        public SelectList MovementStatuses
        {
            get
            {
                var units = Enum.GetValues(typeof(MovementStatus))
                    .Cast<MovementStatus>()
                    .Select(s => new SelectListItem
                    {
                        Text = EnumHelper.GetDisplayName(s),
                        Value = ((int)s).ToString()
                    }).ToList();

                units.Insert(0, new SelectListItem { Text = "View all", Value = string.Empty });

                return new SelectList(units, "Value", "Text", SelectedMovementStatus);
            }
        }

        public MovementSummaryViewModel(Guid notificationId, MovementSummaryData data)
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

            TableData = new List<MovementSummaryTableViewModel>(
                data.ShipmentTableData.OrderByDescending(m => m.Number)
                    .Select(p => new MovementSummaryTableViewModel(p)));
        }
    }
}