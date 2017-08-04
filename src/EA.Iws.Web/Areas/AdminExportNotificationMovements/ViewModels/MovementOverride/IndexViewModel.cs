namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.MovementOverride
{
    using Core.Movement;
    using Core.Shared;
    using Infrastructure.Validation;
    using Prsd.Core;
    using Prsd.Core.Helpers;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Web.ViewModels.Shared;

    public class IndexViewModel : IValidatableObject
    {
        public Int32 ShipmentNumber { get; set; }

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

        public DateTime? RecoveryDate { get; set; }

        [Display(Name = "HasComments", ResourceType = typeof(IndexViewModelResources))]
        public bool HasComments { get; set; }

        [Display(Name = "Comments", ResourceType = typeof(IndexViewModelResources))]
        public string Comments { get; set; }

        [Display(Name = "StatsMarking", ResourceType = typeof(IndexViewModelResources))]
        public string StatsMarking { get; set; }

        public SelectList StatsMarkingSelectList
        {
            //Wriet this in core
            get
            {
                return new SelectList(new[]
                {
                    "Illegal Shipment (WSR Table 5)",
                    "Did not proceed as intended (Basel Table 9)",
                    "Accident occurred during transport (Basel Table 10)"
                });
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
            ActualQuantity = data.ActualQuantity;
            ReceivedDate = data.ReceiptDate;
            Units = data.ReceiptUnits ?? data.NotificationUnits;
            WasShipmentAccepted = string.IsNullOrWhiteSpace(data.RejectionReason);
            RejectionReason = data.RejectionReason;
            PossibleUnits = data.PossibleUnits;

            NotificationType = data.NotificationType;
            
            RecoveryDate = data.OperationCompleteDate;         
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ActualShipmentDate.HasValue)
            {
                yield return new ValidationResult(IndexViewModelResources.ActualDateRequired, new[] { "ActualShipmentDate" });
            }

            if (ReceivedDate.HasValue && !ActualQuantity.HasValue)
            {
                yield return new ValidationResult(IndexViewModelResources.QuantityRequired, new[] { "Receipt.ActualQuantity" });
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