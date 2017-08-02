namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.CaptureMovement
{
    using Core.Movement;
    using Core.Shared;
    using Prsd.Core;
    using Prsd.Core.Helpers;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Web.ViewModels.Shared;

    public class CaptureViewModel : IValidatableObject
    {
        [Required(ErrorMessageResourceName = "NumberRequired", ErrorMessageResourceType = typeof(CaptureViewModelResources))]
        [Display(Name = "Number", ResourceType = typeof(CaptureViewModelResources))]
        [Range(1, int.MaxValue, ErrorMessage = null, ErrorMessageResourceName = "NumberIsInt", ErrorMessageResourceType = typeof(CaptureViewModelResources))]
        public int? ShipmentNumber { get; set; }

        [Display(Name = "NewShipmentNumber", ResourceType = typeof(CaptureViewModelResources))]
        [Range(1, int.MaxValue, ErrorMessage = null, ErrorMessageResourceName = "NumberIsInt", ErrorMessageResourceType = typeof(CaptureViewModelResources))]
        public int? NewShipmentNumber { get; set; }

        public ReceiptViewModel Receipt { get; set; }

        public RecoveryViewModel Recovery { get; set; }

        [Display(Name = "PrenotificationDateLabel", ResourceType = typeof(CaptureViewModelResources))]
        public MaskedDateInputViewModel PrenotificationDate { get; set; }

        [Display(Name = "ActualDateLabel", ResourceType = typeof(CaptureViewModelResources))]
        public MaskedDateInputViewModel ActualShipmentDate { get; set; }

        [Display(Name = "HasNoPrenotification", ResourceType = typeof(CaptureViewModelResources))]
        public bool HasNoPrenotification { get; set; }

        public NotificationType NotificationType { get; set; }

        public bool IsReceived { get; set; }

        public bool IsOperationCompleted { get; set; }

        public bool IsRejected { get; set; }

        [Display(Name = "HasComments", ResourceType = typeof(CaptureViewModelResources))]
        public bool HasComments { get; set; }

        [Display(Name = "Comments", ResourceType = typeof(CaptureViewModelResources))]
        public string Comments { get; set; }

        [Display(Name = "StatsMarking", ResourceType = typeof(CaptureViewModelResources))]
        public string StatsMarking { get; set; }

        public SelectList StatsMarkingSelectList
        {
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

        public Guid NotificationId { get; set; }

        public string NotificationNumber { get; set; }

        public int IntendedShipments { get; set; }

        public int UsedShipments { get; set; }

        public int ActiveLoads { get; set; }

        public string QuantityRemainingTotal { get; set; }

        public string QuantityReceivedTotal { get; set; }

        public string AverageTonnage { get; set; }

        public CaptureViewModel()
        {
            PrenotificationDate = new MaskedDateInputViewModel();
            ActualShipmentDate = new MaskedDateInputViewModel();
            Receipt = new ReceiptViewModel();
            Recovery = new RecoveryViewModel();
        }

        public CaptureViewModel(MovementReceiptAndRecoveryData data)
        {
            ActualShipmentDate = new MaskedDateInputViewModel(data.ActualDate);
            if (data.PrenotificationDate.HasValue)
            {
                PrenotificationDate = new MaskedDateInputViewModel(data.PrenotificationDate.Value);
            }
            else
            {
                PrenotificationDate = new MaskedDateInputViewModel();
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
            NotificationId = data.NotificationId;
            IsReceived = data.IsReceived;
            IsOperationCompleted = data.IsOperationCompleted;
            IsRejected = data.IsRejected;

            Receipt = new ReceiptViewModel
            {
                ActualQuantity = data.ActualQuantity,
                ReceivedDate = new MaskedDateInputViewModel(data.ReceiptDate),
                Units = data.ReceiptUnits ?? data.NotificationUnits,
                WasShipmentAccepted = string.IsNullOrWhiteSpace(data.RejectionReason),
                RejectionReason = data.RejectionReason,
                PossibleUnits = data.PossibleUnits
            };

            Recovery = new RecoveryViewModel
            {
                NotificationType = data.NotificationType,
                RecoveryDate = new MaskedDateInputViewModel(data.OperationCompleteDate)
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ActualShipmentDate.IsCompleted)
            {
                yield return new ValidationResult(CaptureViewModelResources.ActualDateRequired, new[] { "ActualShipmentDate" });
            }

            if (!HasNoPrenotification && !PrenotificationDate.IsCompleted)
            {
                yield return new ValidationResult(CaptureViewModelResources.PrenotificationDateRequired, 
                    new[] { "PrenotificationDate" });
            }
          
            if ((!Receipt.ReceivedDate.IsCompleted && Receipt.ActualQuantity.HasValue) || (!Receipt.ReceivedDate.IsCompleted && !string.IsNullOrWhiteSpace(Receipt.RejectionReason)))
            {
                yield return new ValidationResult(CaptureViewModelResources.ReceivedDateRequired, new[] { "Receipt.ReceivedDate" });
            }

            if (!Receipt.ActualQuantity.HasValue && Receipt.ReceivedDate.IsCompleted && Receipt.WasShipmentAccepted)
            {
                yield return new ValidationResult(CaptureViewModelResources.QuantityRequired, new[] { "Receipt.ActualQuantity" });
            }

            if (!Receipt.WasShipmentAccepted && string.IsNullOrWhiteSpace(Receipt.RejectionReason))
            {
                yield return new ValidationResult(CaptureViewModelResources.RejectReasonRequired, new[] { "Receipt.RejectionReason" });
            }

            if (Recovery.IsComplete() && !Receipt.IsComplete())
            {
                yield return new ValidationResult(string.Format(CaptureViewModelResources.ReceiptMustBeCompletedFirst, NotificationType),
                    new[] { "Recovery.RecoveryDate" });
            }

            if (Receipt.IsComplete() && !Receipt.WasShipmentAccepted && Recovery.IsComplete())
            {
                yield return new ValidationResult(string.Format(CaptureViewModelResources.RecoveryDateCannotBeEnteredForRejected, NotificationType),
                    new[] { "Recovery.RecoveryDate" });
            }

            if (PrenotificationDate.IsCompleted && PrenotificationDate.Date > SystemTime.UtcNow.Date)
            {
                yield return new ValidationResult(CaptureViewModelResources.PrenotifictaionDateInfuture,
                   new[] { "PrenotificationDate" });
            }

            if (ActualShipmentDate.IsCompleted && PrenotificationDate.IsCompleted)
            {
                DateTime preNotificateDate = PrenotificationDate.Date.Value;

                if (ActualShipmentDate.Date < preNotificateDate)
                {
                    yield return new ValidationResult(CaptureViewModelResources.ActualDateBeforePrenotification, new[] { "ActualShipmentDate" });
                }

                if (ActualShipmentDate.Date > preNotificateDate.AddDays(60))
                {
                    yield return new ValidationResult(CaptureViewModelResources.ActualDateGreaterthanSixtyDays, new[] { "ActualShipmentDate" });
                }
            }

            if (Receipt.IsComplete())
            {
                if (Receipt.ReceivedDate.Date < ActualShipmentDate.Date)
                {
                    yield return new ValidationResult(CaptureViewModelResources.ReceivedDateBeforeActualDate, new[] { "Receipt.ReceivedDate" });
                }
                if (Receipt.ReceivedDate.Date > SystemTime.UtcNow.Date)
                {
                    yield return new ValidationResult(CaptureViewModelResources.ReceivedDateInfuture, new[] { "Receipt.ReceivedDate" });
                }
            }

            if (Recovery.IsComplete())
            {
                if (Recovery.RecoveryDate.Date < Receipt.ReceivedDate.Date)
                {
                    yield return new ValidationResult(string.Format(CaptureViewModelResources.RecoveredDateBeforeReceivedDate, GetNotificationTypeVerb(Recovery.NotificationType)), new[] { "Recovery.RecoveryDate" });
                }
                if (Recovery.RecoveryDate.Date > SystemTime.UtcNow.Date)
                {
                    yield return new ValidationResult(string.Format(CaptureViewModelResources.RecoveredDateInfuture, GetNotificationTypeVerb(Recovery.NotificationType)), new[] { "Recovery.RecoveryDate" });
                }
            }
        }

        private static string GetNotificationTypeVerb(NotificationType displayedType)
        {
            return displayedType == NotificationType.Recovery ? "recovered" : "disposed of";
        }

        public void SetSummaryData(InternalMovementSummary summaryData)
        {
            NotificationNumber = summaryData.NotificationNumber;
            IntendedShipments = summaryData.TotalIntendedShipments;
            ActiveLoads = summaryData.ActiveLoadsPermitted;
            AverageTonnage = summaryData.AverageTonnage + EnumHelper.GetShortName(summaryData.AverageDataUnit);
            UsedShipments = summaryData.TotalShipments;
            QuantityRemainingTotal = summaryData.QuantityRemaining.ToString("G29") + " " + EnumHelper.GetShortName(summaryData.DisplayUnit);
            QuantityReceivedTotal = summaryData.QuantityReceived.ToString("G29") + " " + EnumHelper.GetShortName(summaryData.DisplayUnit);
        }
    }
}