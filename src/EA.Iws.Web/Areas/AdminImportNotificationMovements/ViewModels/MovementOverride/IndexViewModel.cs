namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.ViewModels.MovementOverride
{
    using Core.ImportMovement;
    using Core.ImportNotificationMovements;
    using Core.Shared;
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

        public IndexViewModel()
        {
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

            NotificationType = data.Data.NotificationType;
            Date = data.RecoveryData.OperationCompleteDate.HasValue ? data.RecoveryData.OperationCompleteDate.Value.DateTime : (DateTime?)null;
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
            if (!ActualShipmentDate.HasValue)
            {
                yield return new ValidationResult(IndexViewModelResources.ActualShipmentDateRequired, new[] { "ActualShipmentDate" });
            }

            if (ReceivedDate.HasValue && WasAccepted && !ActualQuantity.HasValue)
            {
                yield return new ValidationResult(IndexViewModelResources.QuantityRequired, new[] { "ActualQuantity" });
            }
        }
    }
}