namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.ViewModels.Capture
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

    public class ReceiptViewModel : IComplete
    {
        [Display(Name = "ReceivedDateLabel", ResourceType = typeof(ReceiptViewModelResources))]
        public MaskedDateInputViewModel ReceivedDate { get; set; }

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
                return new SelectList(PossibleUnits.OrderBy(u => (int)u).Select(u => new KeyValuePair<string, ShipmentQuantityUnits>(EnumHelper.GetDisplayName(u), u)), "Value", "Key");
            }
        }

        public SelectList RejectionReasonsSelectList
        {
            get
            {
                return new SelectList(new MovementRejectionReasons());
            }
        }

        public ReceiptViewModel()
        {
            ReceivedDate = new MaskedDateInputViewModel();
            PossibleUnits = new List<ShipmentQuantityUnits>();
            WasAccepted = true;
        }

        public ReceiptViewModel(ImportMovementReceiptData importMovementReceiptData)
        {
            ActualQuantity = importMovementReceiptData.ActualQuantity;
            Units = importMovementReceiptData.ReceiptUnits ?? importMovementReceiptData.NotificationUnit;
            PossibleUnits = importMovementReceiptData.PossibleUnits;
            RejectionReason = importMovementReceiptData.RejectionReason;
            RejectionFurtherInformation = importMovementReceiptData.RejectionReasonFurtherInformation;
            WasAccepted = string.IsNullOrWhiteSpace(RejectionReason);

            if (importMovementReceiptData.ReceiptDate.HasValue)
            {
                ReceivedDate = new MaskedDateInputViewModel(importMovementReceiptData.ReceiptDate.Value.DateTime);
            }
            else
            {
                ReceivedDate = new MaskedDateInputViewModel();
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
    }
}