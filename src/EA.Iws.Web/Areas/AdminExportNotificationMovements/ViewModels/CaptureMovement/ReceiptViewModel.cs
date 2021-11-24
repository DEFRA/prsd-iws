namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.CaptureMovement
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Shared;
    using Infrastructure.Validation;
    using Prsd.Core.Helpers;
    using Web.ViewModels.Shared;
    
    public class ReceiptViewModel
    {
        [Display(Name = "ReceivedDateLabel", ResourceType = typeof(ReceiptViewModelResources))]
        public MaskedDateInputViewModel ReceivedDate { get; set; }

        [Display(Name = "ActualQuantityLabel", ResourceType = typeof(ReceiptViewModelResources))]
        [IsValidNumber(14, ErrorMessageResourceName = "MaximumActualQuantity", ErrorMessageResourceType = typeof(ReceiptViewModelResources), IsOptional = true)]
        public decimal? ActualQuantity { get; set; }

        public ShipmentQuantityUnits? ActualUnits { get; set; }

        [Display(Name = "RejectionReasonLabel", ResourceType = typeof(ReceiptViewModelResources))]
        public string RejectionReason { get; set; }

        [Display(Name = "FullyRejectionReasonInfomationLabel1", ResourceType = typeof(ReceiptViewModelResources))]
        public string FullyRejectionReasonInfomation1 { get; set; }

        [Display(Name = "FullyRejectionReasonInfomationLabel2", ResourceType = typeof(ReceiptViewModelResources))]
        public string FullyRejectionReasonInfomation2 { get; set; }

        [Display(Name = "PartiallyRejectionReasonInfomationLabel1", ResourceType = typeof(ReceiptViewModelResources))]
        public string PartiallyRejectionReasonInfomation1 { get; set; }

        [Display(Name = "ShipmentRejectedQuantityLabel", ResourceType = typeof(ReceiptViewModelResources))]
        public string ShipmentRejectedQuantity { get; set; }

        [Display(Name = "PartiallyRejectionReasonInfomationLabel2", ResourceType = typeof(ReceiptViewModelResources))]
        public string PartiallyRejectionReasonInfomation2 { get; set; }

        [Display(Name = "WasShipmentRejectedLabel", ResourceType = typeof(ReceiptViewModelResources))]
        public bool WasShipmentRejected { get; set; }

        [Display(Name = "RejectedQuantityLabel", ResourceType = typeof(ReceiptViewModelResources))]
        [IsValidNumber(14, ErrorMessageResourceName = "MaximumActualQuantity", ErrorMessageResourceType = typeof(ReceiptViewModelResources), IsOptional = true)]
        public decimal? RejectedQuantity { get; set; }

        public ShipmentQuantityUnits? RejectedUnits { get; set; }

        [Display(Name = "RejectedQuantityInfoLabel", ResourceType = typeof(ReceiptViewModelResources))]
        public bool RejectedQuantityInfo { get; set; }

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

        public ShipmentType ShipmentTypes { get; set; }

        public ReceiptViewModel()
        {
            ShipmentTypes = ShipmentType.Accepted;
            ReceivedDate = new MaskedDateInputViewModel();
            PossibleUnits = new List<ShipmentQuantityUnits>();
        }

        public bool IsComplete()
        {
            if (ShipmentTypes == ShipmentType.Accepted)
            {
                return ReceivedDate.IsCompleted
                       && ActualQuantity.HasValue
                       && ActualUnits.HasValue;
            }
            else if (ShipmentTypes == ShipmentType.Rejected || ShipmentTypes == ShipmentType.Partially)
            {
                return !string.IsNullOrWhiteSpace(RejectionReason) && ReceivedDate.IsCompleted;
            }

            return false;
        }
    }
}