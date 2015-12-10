namespace EA.Iws.Web.Areas.AdminImportMovement.ViewModels.Home
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.ImportMovement;
    using Core.Shared;
    using Infrastructure.Validation;
    using Prsd.Core.Helpers;
    using Web.ViewModels.Shared;

    public class ReceiptViewModel : IComplete, IValidatableObject
    {
        [Display(Name = "ReceivedDateLabel", ResourceType = typeof(ReceiptViewModelResources))]
        [RequiredDateInput(ErrorMessageResourceName = "ReceivedDateRequired", ErrorMessageResourceType = typeof(ReceiptViewModelResources))]
        public OptionalDateInputViewModel ReceivedDate { get; set; }
        
        [Display(Name = "ActualQuantityLabel", ResourceType = typeof(ReceiptViewModelResources))]
        public decimal? ActualQuantity { get; set; }

        [Display(Name = "RejectionReasonLabel", ResourceType = typeof(ReceiptViewModelResources))]
        public string RejectionReason { get; set; }

        [Display(Name = "RejectionFurtherInformationLabel", ResourceType = typeof(ReceiptViewModelResources))]
        public string RejectionFurtherInformation { get; set; }

        [Display(Name = "WasShipmentAcceptedLabel", ResourceType = typeof(ReceiptViewModelResources))]
        public bool WasAccepted { get; set; }

        [Required(ErrorMessageResourceName = "UnitsRequired", ErrorMessageResourceType = typeof(ReceiptViewModelResources))]
        public ShipmentQuantityUnits? Units { get; set; }

        public IList<ShipmentQuantityUnits> PossibleUnits { get; set; }

        public SelectList UnitSelectList
        {
            get
            {
                return new SelectList(PossibleUnits.Select(u => new KeyValuePair<string, ShipmentQuantityUnits>(EnumHelper.GetDisplayName(u), u)), "Value", "Key");
            }
        }

        public SelectList RejectionReasonsSelectList
        {
            get
            {
                return
                    new SelectList(
                        PossibleUnits.Select(
                            u => new KeyValuePair<string, ShipmentQuantityUnits>(EnumHelper.GetDisplayName(u), u)),
                        "Value", "Key");
            }
        }

        public ReceiptViewModel()
        {
            ReceivedDate = new OptionalDateInputViewModel(true);
            PossibleUnits = new List<ShipmentQuantityUnits>();
            WasAccepted = true;
        }

        public ReceiptViewModel(ImportMovementReceiptData importMovementReceiptData)
        {
            ActualQuantity = importMovementReceiptData.ActualQuantity;
            Units = importMovementReceiptData.ReceiptUnits;
            PossibleUnits = importMovementReceiptData.PossibleUnits;
            RejectionReason = importMovementReceiptData.RejectionReason;
            RejectionFurtherInformation = importMovementReceiptData.RejectionReasonFurtherInformation;
            WasAccepted = string.IsNullOrWhiteSpace(RejectionReason);

            if (importMovementReceiptData.ReceiptDate.HasValue)
            {
                ReceivedDate = new OptionalDateInputViewModel(importMovementReceiptData.ReceiptDate.Value.DateTime, true);
            }
            else
            {
                ReceivedDate = new OptionalDateInputViewModel(true);
            }
        }

        public bool IsComplete()
        {
            if (WasAccepted)
            {
                return ReceivedDate.IsCompleted
                       && ActualQuantity.HasValue
                       && Units.HasValue;
            }

            return !string.IsNullOrWhiteSpace(RejectionReason) && ReceivedDate.IsCompleted;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (WasAccepted)
            {
                if (!ActualQuantity.HasValue)
                {
                    yield return new ValidationResult(ReceiptViewModelResources.QuantityRequired, new[] { "ActualQuantity" });
                }
                if (!Units.HasValue)
                {
                    yield return new ValidationResult(ReceiptViewModelResources.UnitsRequired, new[] { "Units" });
                }
            }

            if (!WasAccepted && string.IsNullOrWhiteSpace(RejectionReason))
            {
                yield return new ValidationResult(ReceiptViewModelResources.RejectReasonRequired, new[] { "RejectionReason" });
            }
        }
    }
}