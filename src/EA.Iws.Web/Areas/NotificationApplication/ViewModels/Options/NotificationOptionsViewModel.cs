namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Options
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Core.FinancialGuarantee;
    using Core.Movement;
    using Core.Notification;
    using Core.NotificationAssessment;
    using Core.Shared;
    using Prsd.Core.Helpers;

    public class NotificationOptionsViewModel
    {
        public CompetentAuthority CompetentAuthority { get; set; }

        public NotificationStatus NotificationStatus { get; set; }

        public Guid NotificationId { get; set; }

        public string NotificationNumber { get; set; }

        public NotificationType NotificationType { get; set; }

        public int IntendedShipments { get; set; }

        public int UsedShipments { get; set; }

        public string QuantityRemainingTotal { get; set; }

        public string QuantityReceivedTotal { get; set; }

        public int ActiveLoadsPermitted { get; set; }

        public int ActiveLoadsCurrent { get; set; }

        public FinancialGuaranteeStatus FinancialGuaranteeStatus { get; set; }

        public List<ShipmentDatesTableViewModel> TableData { get; set; }

        public MovementStatus? SelectedMovementStatus { get; set; }

        public bool IsInterimNotification { get; set; }

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

        public NotificationOptionsViewModel(Guid notificationId, NotificationMovementsSummaryAndTable data)
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

            CompetentAuthority = data.SummaryData.CompetentAuthority;
            NotificationStatus = data.SummaryData.NotificationStatus;
            FinancialGuaranteeStatus = data.SummaryData.FinancialGuaranteeStatus;
            IsInterimNotification = data.IsInterimNotification;

            TableData = new List<ShipmentDatesTableViewModel>(
                data.ShipmentTableData.OrderByDescending(m => m.Number)
                    .Select(p => new ShipmentDatesTableViewModel(p)));
        }

        private string GetMovementStatusText(MovementStatus status)
        {
            if (status == MovementStatus.Completed)
            {
                return NotificationType == NotificationType.Disposal ? "Disposed" : "Recovered";
            }

            return EnumHelper.GetDisplayName(status);
        }

        public bool ShowShipmentOptions
        {
            get
            {
                return (NotificationStatus == NotificationStatus.Consented ||
                        NotificationStatus == NotificationStatus.ConsentWithdrawn)
                       &&
                       (FinancialGuaranteeStatus == FinancialGuaranteeStatus.Approved ||
                        FinancialGuaranteeStatus == FinancialGuaranteeStatus.Released);
            }
        }

        public bool ShowShipmentKeyDates
        {
            get
            {
                return NotificationStatus == NotificationStatus.Consented ||
                       NotificationStatus == NotificationStatus.ConsentWithdrawn;
            }
        }

        public bool ShowGenerateInterimDocumentLink
        {
            get
            {
                return (NotificationStatus == NotificationStatus.Consented &&
                       FinancialGuaranteeStatus == FinancialGuaranteeStatus.Approved &&
                       IsInterimNotification);
            }
        }
    }
}