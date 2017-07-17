namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.CaptureMovement
{
    using AdminExportNotificationsMovements.ViewModels.CaptureMovement;
    using Core.Shared;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Web.ViewModels.Shared;
   
    public class CreateViewModel : IValidatableObject
    {
        public int? LatestCurrentMovementNumber { get; set; }

        [Required(ErrorMessageResourceName = "NumberRequired", ErrorMessageResourceType = typeof(CreateViewModelResources))]
        [Display(Name = "Number", ResourceType = typeof(CreateViewModelResources))]
        [Range(1, int.MaxValue, ErrorMessage = null, ErrorMessageResourceName = "NumberIsInt", ErrorMessageResourceType = typeof(CreateViewModelResources))]
        public int? Number { get; set; }

        public Guid NotificationId { get; set; }

        public ReceiptViewModel Receipt { get; set; }

        public RecoveryViewModel Recovery { get; set; }

        [Display(Name = "PrenotificationDateLabel", ResourceType = typeof(CreateViewModelResources))]
        public OptionalDateInputViewModel PrenotificationDate { get; set; }

        [Display(Name = "ActualDateLabel", ResourceType = typeof(CreateViewModelResources))]
        public OptionalDateInputViewModel ActualShipmentDate { get; set; }

        [Display(Name = "HasNoPrenotification", ResourceType = typeof(CreateViewModelResources))]
        public bool HasNoPrenotification { get; set; }

        public DateTime ActualDate { get; set; }

        public NotificationType NotificationType { get; set; }

        public bool IsReceived { get; set; }

        public bool IsSaved { get; set; }

        public bool IsOperationCompleted { get; set; }

        public CreateViewModel()
        {
            PrenotificationDate = new OptionalDateInputViewModel(true);
            ActualShipmentDate = new OptionalDateInputViewModel(true);
            Receipt = new ReceiptViewModel();
            Recovery = new RecoveryViewModel();                
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ActualShipmentDate.IsCompleted)
            {
                yield return new ValidationResult(CreateViewModelResources.ActualDateRequired, new[] { "ActualShipmentDate" });
            }

            if (!HasNoPrenotification && !PrenotificationDate.IsCompleted)
            {
                yield return new ValidationResult(CreateViewModelResources.PrenotificationDateRequired, 
                    new[] { "PrenotificationDate" });
            }
          
            if ((!Receipt.ReceivedDate.IsCompleted && Receipt.ActualQuantity.HasValue) || (!Receipt.ReceivedDate.IsCompleted && !string.IsNullOrWhiteSpace(Receipt.RejectionReason)))
            {
                yield return new ValidationResult(CreateViewModelResources.ReceivedDateRequired, new[] { "Receipt.ReceivedDate.Day" });
            }

            if (!Receipt.ActualQuantity.HasValue && Receipt.ReceivedDate.IsCompleted)
            {
                yield return new ValidationResult(CreateViewModelResources.QuantityRequired, new[] { "Receipt.ActualQuantity" });
            }

            if (!Receipt.WasShipmentAccepted && string.IsNullOrWhiteSpace(Receipt.RejectionReason))
            {
                yield return new ValidationResult(CreateViewModelResources.RejectReasonRequired, new[] { "Receipt.RejectionReason" });
            }

            if (Recovery.IsComplete() && !Receipt.IsComplete())
            {
                yield return new ValidationResult(string.Format(CreateViewModelResources.ReceiptMustBeCompletedFirst, NotificationType),
                    new[] { "Recovery.RecoveryDate.Day" });
            }

            if (Receipt.IsComplete() && !Receipt.WasShipmentAccepted && Recovery.IsComplete())
            {
                yield return new ValidationResult(string.Format(CreateViewModelResources.RecoveryDateCannotBeEnteredForRejected, NotificationType),
                    new[] { "Recovery.RecoveryDate.Day" });
            }
        }

        public bool ShowReceiptAndRecoveryAsReadOnly()
        {
            return IsReceived && IsOperationCompleted;
        }

        public bool ShowReceiptDataAsReadOnly()
        {
            return IsReceived;
        }

        public bool ShowRecoveryDataAsReadOnly()
        {
            return IsOperationCompleted;
        }
    }
}