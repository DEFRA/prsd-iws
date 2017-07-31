namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.ViewModels.Capture
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Core.ImportMovement;
    using Core.Shared;
    using Prsd.Core;
    using Prsd.Core.Helpers;
    using Web.ViewModels.Shared;

    public class CaptureViewModel : IValidatableObject
    {
        [Required(ErrorMessageResourceType = typeof(SearchViewModelResources), ErrorMessageResourceName = "Required")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(SearchViewModelResources), ErrorMessageResourceName = "Range")]
        [Display(Name = "ShipmentNumber", ResourceType = typeof(CaptureViewModelResources))]
        public int? ShipmentNumber { get; set; }

        [Display(Name = "ActualShipmentDate", ResourceType = typeof(CaptureViewModelResources))]
        public MaskedDateInputViewModel ActualShipmentDate { get; set; }

        [Display(Name = "PrenotificationDate", ResourceType = typeof(CaptureViewModelResources))]
        public MaskedDateInputViewModel PrenotificationDate { get; set; }

        [Display(Name = "HasNoPrenotification", ResourceType = typeof(CaptureViewModelResources))]
        public bool HasNoPrenotification { get; set; }

        public ReceiptViewModel Receipt { get; set; }

        public RecoveryViewModel Recovery { get; set; }

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

        public NotificationType NotificationType { get; set; }
 
        public string NotificationNumber { get; set; }

        public int IntendedShipments { get; set; }

        public int UsedShipments { get; set; }

        public string QuantityRemainingTotal { get; set; }

        public string QuantityReceivedTotal { get; set; }

        public string AverageTonnage { get; set; }
        public CaptureViewModel()
        {
            ActualShipmentDate = new MaskedDateInputViewModel();
            PrenotificationDate = new MaskedDateInputViewModel();
            Receipt = new ReceiptViewModel();
            Recovery = new RecoveryViewModel();
        }
  	public void UpdateSummaryViewModel(ImportInternalMovementSummary floatingSummary)
        {
            IntendedShipments = floatingSummary.SummaryData.IntendedShipments;
            AverageTonnage = floatingSummary.AverageTonnage + EnumHelper.GetShortName(floatingSummary.AverageDataUnit);
            UsedShipments = floatingSummary.SummaryData.UsedShipments;
            QuantityRemainingTotal = floatingSummary.SummaryData.QuantityRemainingTotal.ToString("G29") + EnumHelper.GetShortName(floatingSummary.SummaryData.DisplayUnit);
            QuantityReceivedTotal = floatingSummary.SummaryData.QuantityReceivedTotal.ToString("G29") + EnumHelper.GetShortName(floatingSummary.SummaryData.DisplayUnit);
        }
        public CaptureViewModel(ImportMovementSummaryData data)
        {
            ShipmentNumber = data.Data.Number;
            ActualShipmentDate = new MaskedDateInputViewModel(data.Data.ActualDate.DateTime);

            if (data.Data.PreNotificationDate.HasValue)
            {
                PrenotificationDate = new MaskedDateInputViewModel(data.Data.PreNotificationDate.Value.DateTime);
            }
            else
            {
                PrenotificationDate = new MaskedDateInputViewModel();
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
            IsOperationCompleted = data.RecoveryData.IsOperationCompleted;
            IsRejected = data.ReceiptData.IsRejected;

            Receipt = new ReceiptViewModel
            {
                ActualQuantity = data.ReceiptData.ActualQuantity,
                ReceivedDate = data.ReceiptData.ReceiptDate.HasValue ? new MaskedDateInputViewModel(data.ReceiptData.ReceiptDate.Value.DateTime) : new MaskedDateInputViewModel(),
                Units = data.ReceiptData.ReceiptUnits ?? data.ReceiptData.NotificationUnit,
                WasAccepted = string.IsNullOrWhiteSpace(data.ReceiptData.RejectionReason),
                RejectionReason = data.ReceiptData.RejectionReason,
                PossibleUnits = data.ReceiptData.PossibleUnits
            };

            Recovery = new RecoveryViewModel
            {
                NotificationType = data.Data.NotificationType,
                RecoveryDate = data.RecoveryData.OperationCompleteDate.HasValue ? new MaskedDateInputViewModel(data.RecoveryData.OperationCompleteDate.Value.DateTime) : new MaskedDateInputViewModel()
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!HasNoPrenotification && !PrenotificationDate.IsCompleted)
            {
                yield return new ValidationResult(CaptureViewModelResources.PrenotificationDateRequired,
                    new[] { "PrenotificationDate" });
            }

            if (!ActualShipmentDate.IsCompleted)
            {
                yield return new ValidationResult(CaptureViewModelResources.ActualShipmentDateRequired,
                   new[] { "ActualShipmentDate" });
            }

            if ((!Receipt.ReceivedDate.IsCompleted && Receipt.ActualQuantity.HasValue) || (!Receipt.ReceivedDate.IsCompleted && !string.IsNullOrWhiteSpace(Receipt.RejectionReason)))
            {
                yield return new ValidationResult(ReceiptViewModelResources.ReceivedDateRequired, new[] { "Receipt.ReceivedDate" });
            }

            if (!Receipt.ActualQuantity.HasValue && Receipt.ReceivedDate.IsCompleted && Receipt.WasAccepted)
            {
                yield return new ValidationResult(ReceiptViewModelResources.QuantityRequired, new[] { "Receipt.ActualQuantity" });
            }

            if (!Receipt.WasAccepted && string.IsNullOrWhiteSpace(Receipt.RejectionReason))
            {
                yield return new ValidationResult(ReceiptViewModelResources.RejectReasonRequired, new[] { "Receipt.RejectionReason" });
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
                    yield return new ValidationResult(String.Format(CaptureViewModelResources.RecoveredDateBeforeReceivedDate, GetNotificationTypeVerb(Recovery.NotificationType)), new[] { "Recovery.RecoveryDate" });
                }
                if (Recovery.RecoveryDate.Date > SystemTime.UtcNow.Date)
                {
                    yield return new ValidationResult(String.Format(CaptureViewModelResources.RecoveredDateInfuture, GetNotificationTypeVerb(Recovery.NotificationType)), new[] { "Recovery.RecoveryDate" });
                }
            }

            if (Receipt.IsComplete() && !Receipt.WasAccepted && Recovery.IsComplete())
            {
                yield return new ValidationResult(string.Format(CaptureViewModelResources.RecoveryDateCannotBeEnteredForRejected, NotificationType),
                    new[] { "Recovery.RecoveryDate" });
            }
        }

        private static string GetNotificationTypeVerb(NotificationType displayedType)
        {
            return displayedType == NotificationType.Recovery ? "recovered" : "disposed of";
        }
    }
}