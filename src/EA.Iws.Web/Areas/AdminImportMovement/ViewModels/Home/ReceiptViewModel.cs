namespace EA.Iws.Web.Areas.AdminImportMovement.ViewModels.Home
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.ImportMovement;
    using Core.Shared;
    using Prsd.Core.Helpers;
    using Web.ViewModels.Shared;

    public class ReceiptViewModel
    {
        [Display(Name = "ReceivedDateLabel", ResourceType = typeof(ReceiptViewModelResources))]
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

        public ReceiptViewModel(ImportMovementReceipt importMovementReceipt)
        {
            ActualQuantity = importMovementReceipt.ActualQuantity;
            Units = importMovementReceipt.ReceiptUnits;
            PossibleUnits = importMovementReceipt.PossibleUnits;
            RejectionReason = importMovementReceipt.RejectionReason;
            RejectionFurtherInformation = importMovementReceipt.RejectionReasonFurtherInformation;
            WasAccepted = string.IsNullOrWhiteSpace(RejectionReason);

            if (importMovementReceipt.ReceiptDate.HasValue)
            {
                ReceivedDate = new OptionalDateInputViewModel(importMovementReceipt.ReceiptDate.Value.DateTime, true);
            }
            else
            {
                ReceivedDate = new OptionalDateInputViewModel(true);
            }
        }
    }
}