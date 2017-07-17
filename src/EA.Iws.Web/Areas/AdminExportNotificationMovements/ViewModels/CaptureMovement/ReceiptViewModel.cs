namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.CaptureMovement
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
        public OptionalDateInputViewModel ReceivedDate { get; set; }

        [Display(Name = "WasShipmentAcceptedLabel", ResourceType = typeof(ReceiptViewModelResources))]
        public bool WasShipmentAccepted { get; set; }

        [Display(Name = "ActualQuantityLabel", ResourceType = typeof(ReceiptViewModelResources))]
        [IsValidNumber(14, ErrorMessageResourceName = "MaximumActualQuantity", ErrorMessageResourceType = typeof(ReceiptViewModelResources), IsOptional = true)]
        public decimal? ActualQuantity { get; set; }

        public ShipmentQuantityUnits? Units { get; set; }

        [Display(Name = "RejectionReasonLabel", ResourceType = typeof(ReceiptViewModelResources))]
        public string RejectionReason { get; set; }

        [Display(Name = "RejectionFurtherInformationLabel", ResourceType = typeof(ReceiptViewModelResources))]
        public string RejectionFurtherInformation { get; set; }

        public SelectList RejectionReasonsSelectList
        {
            get
            {
                return new SelectList(new MovementRejectionReasons());
            }
        }

        public SelectList UnitSelectList
        {
            get
            {
                return new SelectList(PossibleUnits.Select(u =>
                new KeyValuePair<string, ShipmentQuantityUnits>(EnumHelper.GetDisplayName(u), u)),
                "Value",
                "Key");
            }
        }

        public IList<ShipmentQuantityUnits> PossibleUnits { get; set; }

        public ReceiptViewModel()
        {
            WasShipmentAccepted = true;
            ReceivedDate = new OptionalDateInputViewModel(true);
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