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

        [Display(Name = "WasShipmentAcceptedLabel", ResourceType = typeof(ReceiptViewModelResources))]
        public bool WasShipmentAccepted { get; set; }

        [Display(Name = "ActualQuantityLabel", ResourceType = typeof(ReceiptViewModelResources))]
        [IsValidNumber(14, ErrorMessageResourceName = "MaximumActualQuantity", ErrorMessageResourceType = typeof(ReceiptViewModelResources), IsOptional = true)]
        public decimal? ActualQuantity { get; set; }

        public ShipmentQuantityUnits? Units { get; set; }

        [Display(Name = "RejectionReasonLabel", ResourceType = typeof(ReceiptViewModelResources))]
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

        public ReceiptViewModel()
        {
            WasShipmentAccepted = true;
            ReceivedDate = new MaskedDateInputViewModel();
            PossibleUnits = new List<ShipmentQuantityUnits>();
        }

        public bool IsComplete()
        {
            if (WasShipmentAccepted)
            {
                return ReceivedDate.IsCompleted
                       && ActualQuantity.HasValue
                       && Units.HasValue;
            }

            return !string.IsNullOrWhiteSpace(RejectionReason) && ReceivedDate.IsCompleted;
        }       
    }
}