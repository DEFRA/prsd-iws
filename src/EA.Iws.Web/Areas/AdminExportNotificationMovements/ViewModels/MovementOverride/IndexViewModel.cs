namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.MovementOverride
{
    using Core.Movement;
    using Core.Shared;
    using EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.CaptureMovement;
    using EA.Prsd.Core;
    using Infrastructure.Validation;
    using Prsd.Core.Helpers;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    public class IndexViewModel : IValidatableObject
    {
        public int ShipmentNumber { get; set; }

        public NotificationType NotificationType { get; set; }

        [Display(Name = "PrenotificationDateLabel", ResourceType = typeof(IndexViewModelResources))]       
        public DateTime? PrenotificationDate { get; set; }

        [Display(Name = "ActualDateLabel", ResourceType = typeof(IndexViewModelResources))]
        public DateTime? ActualShipmentDate { get; set; }

        [Display(Name = "HasNoPrenotification", ResourceType = typeof(IndexViewModelResources))]
        public bool HasNoPrenotification { get; set; }

        [Display(Name = "ReceivedDateLabel", ResourceType = typeof(IndexViewModelResources))]
        public DateTime? ReceivedDate { get; set; }

        [Display(Name = "WasShipmentAcceptedLabel", ResourceType = typeof(IndexViewModelResources))]
        public bool WasShipmentAccepted { get; set; }

        [Display(Name = "ActualQuantityLabel", ResourceType = typeof(IndexViewModelResources))]
        [IsValidNumber(14, ErrorMessageResourceName = "MaximumActualQuantity", ErrorMessageResourceType = typeof(IndexViewModelResources), IsOptional = true)]
        public decimal? ActualQuantity { get; set; }

        public ShipmentQuantityUnits? Units { get; set; }

        [Display(Name = "RejectionReasonLabel", ResourceType = typeof(IndexViewModelResources))]
        public string RejectionReason { get; set; }

        [Display(Name = "RejectedQuantityLabel", ResourceType = typeof(ReceiptViewModelResources))]
        [IsValidNumber(14, ErrorMessageResourceName = "MaximumActualQuantity", ErrorMessageResourceType = typeof(ReceiptViewModelResources), IsOptional = true)]
        public decimal? RejectedQuantity { get; set; }

        public ShipmentQuantityUnits? RejectedUnits { get; set; }

        public SelectList UnitSelectList
        {
            get
            {
                return new SelectList(PossibleUnits.OrderBy(u => (int)u).Select(u =>
                new KeyValuePair<string, ShipmentQuantityUnits>(EnumHelper.GetDisplayName(u), u)),
                "Value",
                "Key");
            }
        }

        public IList<ShipmentQuantityUnits> PossibleUnits { get; set; }

        public bool IsReceived { get; set; }

        public bool IsRejected { get; set; }

        public bool IsPartiallyRejected { get; set; }

        public bool IsOperationCompleted { get; set; }

        public DateTime? Date { get; set; }

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

        //Floating Summary
        public string NotificationNumber { get; set; }

        public int IntendedShipments { get; set; }

        public int UsedShipments { get; set; }

        public int ActiveLoads { get; set; }

        public string QuantityRemainingTotal { get; set; }

        public string QuantityReceivedTotal { get; set; }

        public string AverageTonnage { get; set; }

        [Display(Name = "ShipmentNumber", ResourceType = typeof(IndexViewModelResources))]
        [Range(1, int.MaxValue, ErrorMessage = null, ErrorMessageResourceName = "NumberIsInt", ErrorMessageResourceType = typeof(IndexViewModelResources))]
        public int? NewShipmentNumber { get; set; }

        public IndexViewModel()
        {
        }

        public IndexViewModel(MovementReceiptAndRecoveryData data)
        {
            ActualShipmentDate = data.ActualDate;
            if (data.PrenotificationDate.HasValue)
            {
                PrenotificationDate = data.PrenotificationDate.Value;
            }
            else
            {
                HasNoPrenotification = true;
            }
            
            ShipmentNumber = data.Number;

            Comments = data.Comments;
            StatsMarking = data.StatsMarking;

            if (!string.IsNullOrWhiteSpace(data.Comments) || !string.IsNullOrWhiteSpace(data.StatsMarking))
            {
                HasComments = true;
            }

            NotificationType = data.NotificationType;
            IsRejected = data.IsRejected;
            IsReceived = data.IsReceived;
            IsOperationCompleted = data.IsOperationCompleted;
            ActualQuantity = data.ActualQuantity;
            ReceivedDate = data.ReceiptDate;
            Units = data.ReceiptUnits ?? data.NotificationUnits;
            WasShipmentAccepted = string.IsNullOrWhiteSpace(data.RejectionReason);
            RejectionReason = data.RejectionReason;
            PossibleUnits = data.PossibleUnits;
            NotificationType = data.NotificationType;
            Date = data.OperationCompleteDate;
            RejectedQuantity = data.RejectedQuantity;
            RejectedUnits = data.RejectedUnit;
            IsPartiallyRejected = data.IsPartiallyRejected;

            if (!data.IsReceived && !data.IsRejected && !data.IsPartiallyRejected)
            {
                IsReceived = true;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PrenotificationDate > SystemTime.UtcNow.Date)
            {
                yield return new ValidationResult(CaptureViewModelResources.PrenotifictaionDateInfuture, new[] { "PrenotificationDate" });
            }

            if (!HasNoPrenotification && !PrenotificationDate.HasValue)
            {
                yield return new ValidationResult(CaptureViewModelResources.PrenotificationDateRequired, new[] { "PrenotificationDate" });
            }

            if (!ActualShipmentDate.HasValue)
            {
                yield return new ValidationResult(IndexViewModelResources.ActualDateRequired, new[] { "ActualShipmentDate" });
            }

            if (ReceivedDate.HasValue && WasShipmentAccepted && !ActualQuantity.HasValue)
            {
                yield return new ValidationResult(IndexViewModelResources.QuantityRequired, new[] { "ActualQuantity" });
            }

            if (IsPartiallyRejected == true && !ActualQuantity.HasValue)
            {
                yield return new ValidationResult(IndexViewModelResources.QuantityRequired, new[] { "ActualQuantity" });
            }

            if ((IsPartiallyRejected == true || IsRejected == true) && !RejectedQuantity.HasValue)
            {
                yield return new ValidationResult(IndexViewModelResources.RejectedQuantityRequired, new[] { "RejectedQuantity" });
            }

            if ((IsPartiallyRejected == true || IsRejected == true) && string.IsNullOrEmpty(RejectionReason))
            {
                yield return new ValidationResult(IndexViewModelResources.RejectionReasonRequired, new[] { "RejectionReason" });
            }

            if ((IsPartiallyRejected == true || IsRejected == true) && string.IsNullOrWhiteSpace(StatsMarking))
            {
                yield return new ValidationResult(CaptureViewModelResources.StatsMarkingRequired, new[] { "StatsMarking" });
            }
        }

        public void SetSummaryData(InternalMovementSummary summaryData)
        {
            NotificationNumber = summaryData.NotificationNumber;
            IntendedShipments = summaryData.TotalIntendedShipments;
            ActiveLoads = summaryData.ActiveLoadsPermitted;
            AverageTonnage = summaryData.AverageTonnage.ToString("G29") + " " + EnumHelper.GetShortName(summaryData.AverageDataUnit);
            UsedShipments = summaryData.TotalShipments;
            QuantityRemainingTotal = summaryData.QuantityRemaining.ToString("G29") + " " + EnumHelper.GetShortName(summaryData.DisplayUnit);
            QuantityReceivedTotal = summaryData.QuantityReceived.ToString("G29") + " " + EnumHelper.GetShortName(summaryData.DisplayUnit);
        }
    }
}