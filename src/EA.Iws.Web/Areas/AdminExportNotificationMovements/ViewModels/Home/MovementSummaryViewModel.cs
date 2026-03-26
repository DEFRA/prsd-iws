namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.Home
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Core.FinancialGuarantee;
    using Core.Movement;
    using Core.NotificationAssessment;
    using Core.Shared;
    using EA.Iws.Web.Areas.NotificationMovements.Views.Create;
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

        public FinancialGuaranteeStatus FgStatus { get; set; }

        public bool CanDeleteMovement { get; set; }

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

                units.Insert(0, new SelectListItem { Text = "View all shipment status'", Value = string.Empty });

                return new SelectList(units, "Value", "Text", SelectedMovementStatus);
            }
        }

        public MovementSummaryViewModel(Guid notificationId, NotificationMovementsSummaryAndTable data, KeyDatesSummaryData keyDates)
        {
            NotificationId = notificationId;
            NotificationNumber = data.SummaryData.NotificationNumber;
            NotificationType = data.NotificationType;
            IntendedShipments = data.TotalIntendedShipments;
            UsedShipments = data.SummaryData.ShipmentsUsed;
            QuantityRemainingTotal = data.SummaryData.QuantityRemaining.ToString("G29") + " " + EnumHelper.GetDisplayName(data.SummaryData.DisplayUnit);
            QuantityReceivedTotal = data.SummaryData.QuantityReceived.ToString("G29") + " " + EnumHelper.GetDisplayName(data.SummaryData.DisplayUnit);
            ActiveLoadsPermitted = data.SummaryData.ActiveLoadsPermitted;
            ActiveLoadsCurrent = data.SummaryData.CurrentActiveLoads;
            NotificationStatus = data.SummaryData.NotificationStatus;
            FgStatus = data.SummaryData.FinancialGuaranteeStatus;

            TableData = new List<MovementSummaryTableViewModel>(
                data.ShipmentTableData.OrderByDescending(m => m.Number)
                    .Select(p => new MovementSummaryTableViewModel(p)));

            PageSize = data.PageSize;
            PageNumber = data.PageNumber;
            NumberofShipments = data.NumberOfShipments;

            Decisions = new List<NotificationAssessmentDecision>(
                keyDates.DecisionHistory.Where(d => d.Status == NotificationStatus.Consented));

            QuantityRemainingValue = data.SummaryData.QuantityRemaining;
            PreNotificationWarnings = GetPreNotificationWarnings(TableData);
            EarlyShipmentWarnings = GetEarlyShipmentWarnings(TableData);
            ConsentedDateWarnings = GetConsentedDateExceededWarnings(Decisions, TableData);
        }

        private string GetMovementStatusText(MovementStatus status)
        {
            if (status == MovementStatus.Completed)
            {
                return NotificationType == NotificationType.Disposal ? "Disposed" : "Recovered";
            }

            return EnumHelper.GetDisplayName(status);
        }

        private string GetPreNotificationWarnings(List<MovementSummaryTableViewModel> tableData)
        {
            var warnings = new StringBuilder();

            foreach (var row in tableData)
            {
                DateTime? preNotDate = row.PreNotification;
                DateTime? shipDate = row.ShipmentDate;

                if (preNotDate.HasValue && shipDate.HasValue)
                {
                    var difference = (shipDate.Value.Date - preNotDate.Value.Date).Days;

                    if (difference < 3)
                    {
                        warnings.Append(", " + row.Number.ToString());
                    }
                }
            }

            if (warnings.Length == 0)
            {
                return string.Empty;
            }
            else
            {
                return " for shipments: " + warnings.ToString().Remove(0, 2);
            }
        }

        private string GetEarlyShipmentWarnings(List<MovementSummaryTableViewModel> tableData)
        {
            var warnings = new StringBuilder();

            foreach (var row in tableData)
            {
                DateTime? shipDate = row.ShipmentDate;
                DateTime? receivedDate = row.Received;

                if (shipDate.HasValue && receivedDate.HasValue)
                {
                    if (DateTime.Compare((DateTime)shipDate, (DateTime)receivedDate) > 0)
                    {
                        warnings.Append(", " + row.Number.ToString());
                    }
                }
            }

            if (warnings.Length == 0)
            {
                return string.Empty;
            }
            else
            {
                return " for shipments: " + warnings.ToString().Remove(0, 2);
            }
        }

        private string GetConsentedDateExceededWarnings(List<NotificationAssessmentDecision> decisions, List<MovementSummaryTableViewModel> tableData)
        {
            var warnings = new StringBuilder();

            if (decisions != null && decisions.Any())
            {
                var mostRecentConsentedDecision = decisions.OrderByDescending(d => d.Date).FirstOrDefault(d => d.Status == NotificationStatus.Consented);
                var mostRecentConsentedDate = mostRecentConsentedDecision.Date;

                foreach (var row in tableData)
                {
                    DateTime? shipDate = row.ShipmentDate;

                    if (shipDate.HasValue)
                    {
                        if (DateTime.Compare((DateTime)shipDate, (DateTime)mostRecentConsentedDate) > 0)
                        {
                            warnings.Append(", " + row.Number.ToString());
                        }
                    }
                }
            }

            if (warnings.Length == 0)
            {
                return string.Empty;
            }
            else
            {
                return " for shipments: " + warnings.ToString().Remove(0, 2);
            }
        }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public int NumberofShipments { get; set; }

        public List<NotificationAssessmentDecision> Decisions { get; set; }

        public decimal QuantityRemainingValue { get; set; }

        public string PreNotificationWarnings { get; set; }

        public string EarlyShipmentWarnings { get; set; }

        public string ConsentedDateWarnings { get; set; }
    }
}