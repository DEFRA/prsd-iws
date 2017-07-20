namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.ViewModels.Capture
{
    using AdminImportNotificationMovements.ViewModels.Capture;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Infrastructure.Validation;
    using Web.ViewModels.Shared;

    public class CreateViewModel : IValidatableObject
    {
        [Required(ErrorMessageResourceType = typeof(SearchViewModelResources), ErrorMessageResourceName = "Required")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(SearchViewModelResources), ErrorMessageResourceName = "Range")]
        [Display(Name = "ShipmentNumber", ResourceType = typeof(CreateViewModelResources))]
        public int? Number { get; set; }

        public int? LatestCurrentMovementNumber { get; set; }

        public Guid NotificationId { get; set; }

        [RequiredDateInput(ErrorMessageResourceName = "ActualShipmentDateRequired",
            ErrorMessageResourceType = typeof(CreateViewModelResources))]
        [Display(Name = "ActualShipmentDate", ResourceType = typeof(CreateViewModelResources))]
        public MaskedDateInputViewModel ActualShipmentDate { get; set; }

        [Display(Name = "PrenotificationDate", ResourceType = typeof(CreateViewModelResources))]
        public MaskedDateInputViewModel PrenotificationDate { get; set; }

        [Display(Name = "HasNoPrenotification", ResourceType = typeof(CreateViewModelResources))]
        public bool HasNoPrenotification { get; set; }

        public ReceiptViewModel Receipt { get; set; }

        public RecoveryViewModel Recovery { get; set; }

        public bool IsReceived { get; set; }

        public bool IsSaved { get; set; }

        public bool IsOperationCompleted { get; set; }

        public CreateViewModel()
        {
            ActualShipmentDate = new MaskedDateInputViewModel();
            PrenotificationDate = new MaskedDateInputViewModel();
            Receipt = new ReceiptViewModel();
            Recovery = new RecoveryViewModel();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!HasNoPrenotification && !PrenotificationDate.IsCompleted)
            {
                yield return new ValidationResult(CreateViewModelResources.PrenotificationDateRequired,
                    new[] { "PrenotificationDate" });
            }

            if ((!Receipt.ReceivedDate.IsCompleted && Receipt.ActualQuantity.HasValue) || (!Receipt.ReceivedDate.IsCompleted && !string.IsNullOrWhiteSpace(Receipt.RejectionReason)))
            {
                yield return new ValidationResult(ReceiptViewModelResources.ReceivedDateRequired, new[] { "Receipt.ReceivedDate.Date" });
            }

            if (!Receipt.ActualQuantity.HasValue && Receipt.ReceivedDate.IsCompleted)
            {
                yield return new ValidationResult(ReceiptViewModelResources.QuantityRequired, new[] { "Receipt.ActualQuantity" });
            }

            if (!Receipt.WasAccepted && string.IsNullOrWhiteSpace(Receipt.RejectionReason))
            {
                yield return new ValidationResult(ReceiptViewModelResources.RejectReasonRequired, new[] { "Receipt.RejectionReason" });
            }

            if (!Receipt.WasAccepted && string.IsNullOrWhiteSpace(Receipt.RejectionFurtherInformation))
            {
                yield return new ValidationResult(ReceiptViewModelResources.RejectionFurtherInformationRequired, new[] { "Receipt.RejectionFurtherInformation" });
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