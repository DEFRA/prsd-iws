namespace EA.Iws.Web.Areas.AdminMovement.ViewModels.InternalCapture
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.Movement;
    using Core.Shared;
    using Web.ViewModels.Shared;

    public class CaptureViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        public DateTime ActualDate { get; set; }

        public DateTime? PrenotificationDate { get; set; }

        public ReceiptViewModel Receipt { get; set; }

        public RecoveryViewModel Recovery { get; set; }

        public bool IsReceived { get; set; }

        public bool IsOperationCompleted { get; set; }

        public NotificationType NotificationType { get; set; }

        public int MovementNumber { get; set; }

        public CaptureViewModel()
        {
            Receipt = new ReceiptViewModel();
            Recovery = new RecoveryViewModel();
        }

        public CaptureViewModel(MovementReceiptAndRecoveryData data)
        {
            MovementNumber = data.Number;
            ActualDate = data.ActualDate;
            PrenotificationDate = data.PrenotificationDate;
            NotificationType = data.NotificationType;
            IsReceived = data.IsReceived;
            IsOperationCompleted = data.IsOperationCompleted;
            NotificationId = data.NotificationId;
            Receipt = new ReceiptViewModel
            {
                ActualQuantity = data.ActualQuantity,
                ReceivedDate = new OptionalDateInputViewModel(data.ReceiptDate, true),
                Units = data.ReceiptUnits,
                WasShipmentAccepted = string.IsNullOrWhiteSpace(data.RejectionReason),
                RejectionReason = data.RejectionReason,
                RejectionFurtherInformation = data.RejectionReasonFurtherInformation,
                PossibleUnits = data.PossibleUnits
            };
            Recovery = new RecoveryViewModel
            {
                NotificationType = data.NotificationType,
                RecoveryDate = new OptionalDateInputViewModel(data.OperationCompleteDate, true)
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Recovery.IsComplete() && !Receipt.IsComplete())
            {
                yield return new ValidationResult(string.Format(CaptureViewModelResources.ReceiptMustBeCompletedFirst, NotificationType), 
                    new[] { "Recovery.RecoveryDate.Day" });
            }

            if (Receipt.IsComplete() && !Receipt.WasShipmentAccepted && Recovery.IsComplete())
            {
                yield return new ValidationResult(string.Format(CaptureViewModelResources.RecoveryDateCannotBeEnteredForRejected, NotificationType), 
                    new[] { "Recovery.RecoveryDate.Day" });
            }
        }
    }
}