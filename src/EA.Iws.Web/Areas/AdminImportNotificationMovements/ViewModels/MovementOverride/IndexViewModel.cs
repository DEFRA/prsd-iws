namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.ViewModels.MovementOverride
{
    using Core.ImportMovement;
    using Core.ImportNotificationMovements;
    using Core.Shared;
    using EA.Iws.Web.Areas.AdminImportNotificationMovements.ViewModels.Capture;
    using EA.Iws.Web.Infrastructure.Validation;
    using EA.Prsd.Core;
    using Prsd.Core.Helpers;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    public class IndexViewModel : IValidatableObject
    {
        public int ShipmentNumber { get; set; }

        [Display(Name = "NewShipmentNumber", ResourceType = typeof(IndexViewModelResources))]
        [Range(1, int.MaxValue, ErrorMessage = null, ErrorMessageResourceName = "NumberIsInt", ErrorMessageResourceType = typeof(IndexViewModelResources))]
        public int? NewShipmentNumber { get; set; }

        [Display(Name = "ActualShipmentDate", ResourceType = typeof(IndexViewModelResources))]
        public DateTime? ActualShipmentDate { get; set; }

        [Display(Name = "PrenotificationDate", ResourceType = typeof(IndexViewModelResources))]
        public DateTime? PrenotificationDate { get; set; }

        [Display(Name = "HasNoPrenotification", ResourceType = typeof(IndexViewModelResources))]
        public bool HasNoPrenotification { get; set; }

        [Display(Name = "ReceivedDateLabel", ResourceType = typeof(IndexViewModelResources))]
        public DateTime? ReceivedDate { get; set; }

        [Display(Name = "ActualQuantityLabel", ResourceType = typeof(IndexViewModelResources))]
        [IsValidNumber(14, ErrorMessageResourceName = "MaximumActualQuantity", ErrorMessageResourceType = typeof(ReceiptViewModelResources), IsOptional = true)]
        public decimal? ActualQuantity { get; set; }

        [Display(Name = "RejectionReasonLabel", ResourceType = typeof(IndexViewModelResources))]
        public string RejectionReason { get; set; }

        [Display(Name = "WasShipmentAcceptedLabel", ResourceType = typeof(IndexViewModelResources))]
        public bool WasAccepted { get; set; }

        public bool IsOperationCompleted { get; set; }

        public ShipmentQuantityUnits? Units { get; set; }

        public IList<ShipmentQuantityUnits> PossibleUnits { get; set; }

        public SelectList UnitSelectList
        {
            get
            {
                return new SelectList(PossibleUnits.OrderBy(u => (int)u).Select(u => new KeyValuePair<string, ShipmentQuantityUnits>(EnumHelper.GetDisplayName(u), u)), "Value", "Key");
            }
        }
        public DateTime? Date { get; set; }

        public NotificationType NotificationType { get; set; }

        public bool IsReceived { get; set; }

        public bool IsRejected { get; set; }

        public bool IsPartiallyRejected { get; set; }

        // Nullable to allow unanswered state
        public ShipmentType? ShipmentTypes { get; set; }

        [Display(Name = "HasComments", ResourceType = typeof(IndexViewModelResources))]
        public bool HasComments { get; set; }

        [Display(Name = "Comments", ResourceType = typeof(IndexViewModelResources))]
        public string Comments { get; set; }

        [Display(Name = "StatsMarking", ResourceType = typeof(IndexViewModelResources))]
        public string StatsMarking { get; set; }

        public SelectList StatsMarkingSelectList
        {
            get
            {
                return new SelectList(EnumHelper.GetValues(typeof(StatsMarking)), dataTextField: "Value", dataValueField: "Value");
            }
        }

        public string NotificationNumber { get; set; }

        public int IntendedShipments { get; set; }

        public int UsedShipments { get; set; }

        public string QuantityRemainingTotal { get; set; }

        public string QuantityReceivedTotal { get; set; }

        public string AverageTonnage { get; set; }

        [Display(Name = "RejectedQuantityLabel", ResourceType = typeof(ReceiptViewModelResources))]
        [IsValidNumber(14, ErrorMessageResourceName = "MaximumActualQuantity", ErrorMessageResourceType = typeof(ReceiptViewModelResources), IsOptional = true)]
        public decimal? RejectedQuantity { get; set; }

        public ShipmentQuantityUnits? RejectedUnits { get; set; }

        public IndexViewModel()
        {
            PossibleUnits = new List<ShipmentQuantityUnits>();
        }

        public IndexViewModel(ImportMovementSummaryData data)
        {
            ShipmentNumber = data.Data.Number;
            ActualShipmentDate = data.Data.ActualDate.DateTime;

            if (data.Data.PreNotificationDate.HasValue)
            {
                PrenotificationDate = data.Data.PreNotificationDate.Value.DateTime;
            }
            else
            {
                HasNoPrenotification = true;
            }

            Comments = data.Comments;
            StatsMarking = data.StatsMarking;

            if (!string.IsNullOrWhiteSpace(data.Comments) || !string.IsNullOrWhiteSpace(data.StatsMarking))
            {
                HasComments = true;
            }

            NotificationType = data.Data.NotificationType;
            IsReceived = data.ReceiptData.IsReceived;
            IsRejected = data.ReceiptData.IsRejected;
            IsOperationCompleted = data.RecoveryData.IsOperationCompleted;
            ActualQuantity = data.ReceiptData.ActualQuantity;
            ReceivedDate = data.ReceiptData.ReceiptDate.HasValue ? data.ReceiptData.ReceiptDate.Value.DateTime : (DateTime?)null;
            Units = data.ReceiptData.ReceiptUnits ?? data.ReceiptData.NotificationUnit;
            WasAccepted = string.IsNullOrWhiteSpace(data.ReceiptData.RejectionReason);
            RejectionReason = data.ReceiptData.RejectionReason;
            PossibleUnits = data.ReceiptData.PossibleUnits;
            IsPartiallyRejected = data.IsPartiallyRejected;
            RejectedQuantity = data.RejectedQuantity;
            RejectedUnits = data.RejectedUnit;

            // Set ShipmentTypes based on current status
            if (data.IsReceived)
            {
                ShipmentTypes = ShipmentType.Accepted;
            }
            else if (data.IsPartiallyRejected)
            {
                ShipmentTypes = ShipmentType.Partially;
            }
            else if (data.IsRejected)
            {
                ShipmentTypes = ShipmentType.Rejected;
            }
            
            NotificationType = data.Data.NotificationType;
            Date = data.RecoveryData.OperationCompleteDate.HasValue ? data.RecoveryData.OperationCompleteDate.Value : (DateTime?)null;
        }

        public void SetSummaryData(Summary summaryData)
        {
            IntendedShipments = summaryData.IntendedShipments;
            AverageTonnage = summaryData.AverageTonnage.ToString("G29") + " " + EnumHelper.GetShortName(summaryData.AverageDataUnit);
            UsedShipments = summaryData.UsedShipments;
            QuantityRemainingTotal = summaryData.QuantityRemainingTotal.ToString("G29") + " " + EnumHelper.GetShortName(summaryData.DisplayUnit);
            QuantityReceivedTotal = summaryData.QuantityReceivedTotal.ToString("G29") + " " + EnumHelper.GetShortName(summaryData.DisplayUnit);
            NotificationNumber = summaryData.NotificationNumber;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PrenotificationDate > SystemTime.UtcNow.Date)
            {
                yield return new ValidationResult(IndexViewModelResources.PrenotifictaionDateInfuture, new[] { "PrenotificationDate" });
            }

            if (!HasNoPrenotification && !PrenotificationDate.HasValue)
            {
                yield return new ValidationResult(IndexViewModelResources.PrenotificationDateRequired, new[] { "PrenotificationDate" });
            }

            if (!ActualShipmentDate.HasValue)
            {
                yield return new ValidationResult(IndexViewModelResources.ActualShipmentDateRequired, new[] { "ActualShipmentDate" });
            }

            // Add validation for ReceivedDate when Rejected/Partially selected
            if ((ShipmentTypes == ShipmentType.Rejected || ShipmentTypes == ShipmentType.Partially) && !ReceivedDate.HasValue)
            {
                yield return new ValidationResult("Please provide the date when the waste was received", new[] { "ReceivedDate" });
            }

            if (ShipmentTypes == ShipmentType.Partially && !ActualQuantity.HasValue)
            {
                yield return new ValidationResult(IndexViewModelResources.QuantityRequired, new[] { "ActualQuantity" });
            }

            if ((ShipmentTypes == ShipmentType.Partially || ShipmentTypes == ShipmentType.Rejected) && !RejectedQuantity.HasValue)
            {
                yield return new ValidationResult(IndexViewModelResources.RejectedQuantityRequired, new[] { "RejectedQuantity" });
            }

            if ((ShipmentTypes == ShipmentType.Partially || ShipmentTypes == ShipmentType.Rejected) && string.IsNullOrEmpty(RejectionReason))
            {
                yield return new ValidationResult(IndexViewModelResources.RejectReasonRequired, new[] { "RejectionReason" });
            }

            if ((ShipmentTypes == ShipmentType.Partially || ShipmentTypes == ShipmentType.Rejected) && string.IsNullOrWhiteSpace(StatsMarking))
            {
                yield return new ValidationResult(CaptureViewModelResources.StatsMarkingRequired, new[] { "StatsMarking" });
            }

            // Leaving "Was the shipment accepted?" unanswered (null) is valid - it lets an internal
            // user edit other fields on a prenotified movement without marking it as received.
            // Only when Accepted is explicitly chosen do we require the full receipt detail.
            if (ShipmentTypes == ShipmentType.Accepted)
            {
                if (!ReceivedDate.HasValue)
                {
                    yield return new ValidationResult("Please provide the received date", new[] { "ReceivedDate" });
                }

                if (!ActualQuantity.HasValue)
                {
                    yield return new ValidationResult(IndexViewModelResources.QuantityRequired, new[] { "ActualQuantity" });
                }

                if (!Units.HasValue)
                {
                    yield return new ValidationResult("Please select the units for the quantity received", new[] { "Units" });
                }
            }
        }
    }
}