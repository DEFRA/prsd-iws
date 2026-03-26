namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.ViewModels.Home
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Core.ImportNotificationAssessment;
    using Core.ImportNotificationMovements;
    using Core.Shared;
    using EA.Iws.Core.NotificationAssessment;
    using EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.Home;
    using Prsd.Core.Helpers;

    public class MovementSummaryViewModel
    {
        public Guid NotificationId { get; set; }

        public NotificationType NotificationType { get; set; }

        public ImportNotificationStatus NotificationStatus { get; set; }

        public int IntendedShipments { get; set; }

        public int UsedShipments { get; set; }

        public string QuantityRemainingTotal { get; set; }

        public string QuantityReceivedTotal { get; set; }

        public int ActiveLoadsPermitted { get; set; }

        public int ActiveLoadsCurrent { get; set; }

        public List<MovementsSummaryTableViewModel> TableData { get; set; }

        public bool CanDeleteMovement { get; set; }

        public MovementSummaryViewModel()
        {
        }

        public MovementSummaryViewModel(Summary data, MovementsSummary movementsSummary, KeyDatesData keyDates)
        {
            NotificationId = data.Id;
            IntendedShipments = data.IntendedShipments;
            UsedShipments = data.UsedShipments;
            QuantityReceivedTotal = data.QuantityReceivedTotal.ToString("G29") + " " + EnumHelper.GetDisplayName(data.DisplayUnit);
            QuantityRemainingTotal = data.QuantityRemainingTotal.ToString("G29") + " " + EnumHelper.GetDisplayName(data.DisplayUnit);
            TableData = movementsSummary.TableData.OrderByDescending(d => d.Number).Select(d => new MovementsSummaryTableViewModel(d)).ToList();
            NotificationStatus = data.NotificationStatus;
            NotificationType = data.NotificationType;
            PageSize = movementsSummary.PageSize;
            PageNumber = movementsSummary.PageNumber;
            NumberofShipments = movementsSummary.NumberofShipments;

            Decisions = new List<NotificationAssessmentDecision>(
                keyDates.DecisionHistory.Where(d => d.Status == EA.Iws.Core.NotificationAssessment.NotificationStatus.Consented));

            QuantityRemainingValue = data.QuantityRemainingTotal;
            PreNotificationWarnings = GetPreNotificationWarnings(TableData);
            EarlyShipmentWarnings = GetEarlyShipmentWarnings(TableData);
            ConsentedDateWarnings = GetConsentedDateExceededWarnings(Decisions, TableData);
        }

        public bool ShowShipmentOptions()
        {
            return true;
        }

        public bool ShowShipments()
        {
            return TableData != null && TableData.Count > 0;
        }

        private string GetPreNotificationWarnings(List<MovementsSummaryTableViewModel> tableData)
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

        private string GetEarlyShipmentWarnings(List<MovementsSummaryTableViewModel> tableData)
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

        private string GetConsentedDateExceededWarnings(List<NotificationAssessmentDecision> decisions, List<MovementsSummaryTableViewModel> tableData)
        {
            var warnings = new StringBuilder();

            if (decisions != null && decisions.Any())
            {
                var mostRecentConsentedDecision = decisions.OrderByDescending(d => d.Date).FirstOrDefault(d => d.Status == EA.Iws.Core.NotificationAssessment.NotificationStatus.Consented);
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