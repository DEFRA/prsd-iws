namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.Home
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Movement;
    using Core.NotificationAssessment;
    using Core.Shared;
    using Prsd.Core.Helpers;

    public class MovementSummaryViewModel
    {
        public Guid NotificationId { get; set; }

        public string NotificationNumber { get; set; }

        public NotificationStatus NotificationStatus { get; set; }

        public NotificationType NotificationType { get; set; }

        public int IntendedShipments { get; set; }

        public int UsedShipments { get; set; }

        public string QuantityRemainingTotal { get; set; }

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
                        Text = GetMovementStatusText(s),
                        Value = ((int)s).ToString()
                    }).ToList();

                units.Insert(0, new SelectListItem { Text = "View all", Value = string.Empty });

                return new SelectList(units, "Value", "Text", SelectedMovementStatus);
            }
        }

        public MovementSummaryViewModel(Guid notificationId, NotificationMovementsSummaryAndTable data)
        {
            NotificationId = notificationId;
            NotificationNumber = data.SummaryData.NotificationNumber;
            NotificationType = data.NotificationType;
            IntendedShipments = data.TotalIntendedShipments;
            UsedShipments = data.SummaryData.TotalShipments;
            QuantityRemainingTotal = data.SummaryData.QuantityRemaining.ToString("G29") + " " + EnumHelper.GetDisplayName(data.SummaryData.DisplayUnit);
            QuantityReceivedTotal = data.SummaryData.QuantityReceived.ToString("G29") + " " + EnumHelper.GetDisplayName(data.SummaryData.DisplayUnit);
            ActiveLoadsPermitted = data.SummaryData.ActiveLoadsPermitted;
            ActiveLoadsCurrent = data.SummaryData.CurrentActiveLoads;
            NotificationStatus = data.SummaryData.NotificationStatus;

            TableData = new List<MovementSummaryTableViewModel>(
                data.ShipmentTableData.OrderByDescending(m => m.Number)
                    .Select(p => new MovementSummaryTableViewModel(p)));
        }

        private string GetMovementStatusText(MovementStatus status)
        {
            if (status == MovementStatus.Completed)
            {
                return NotificationType == NotificationType.Disposal ? "Disposed" : "Recovered";
            }

            return EnumHelper.GetDisplayName(status);
        }
    }
}