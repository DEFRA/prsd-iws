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

        public MovementSummaryViewModel(Summary data, MovementsSummary movementsSummary, string preNotificationWarnings, string earlyShipmentWarnings, string consentedDateWarnings)
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

            QuantityRemainingValue = data.QuantityRemainingTotal;
            PreNotificationWarnings = preNotificationWarnings;
            EarlyShipmentWarnings = earlyShipmentWarnings;
            ConsentedDateWarnings = consentedDateWarnings;
        }

        public bool ShowShipmentOptions()
        {
            return true;
        }

        public bool ShowShipments()
        {
            return TableData != null && TableData.Count > 0;
        }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public int NumberofShipments { get; set; }

        public decimal QuantityRemainingValue { get; set; }

        public string PreNotificationWarnings { get; set; }

        public string EarlyShipmentWarnings { get; set; }

        public string ConsentedDateWarnings { get; set; }
    }
}