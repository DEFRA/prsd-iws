namespace EA.Iws.Web.Areas.Movement.ViewModels.InternalCapture
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Shared;
    using Prsd.Core.Helpers;
    using Web.ViewModels.Shared;

    public class ReceiptViewModel : IComplete, IValidatableObject
    {
        [Display(Name = "ReceivedDateLabel", ResourceType = typeof(ReceiptViewModelResources))]
        public OptionalDateInputViewModel ReceivedDate { get; set; }

        [Display(Name = "WasShipmentAcceptedLabel", ResourceType = typeof(ReceiptViewModelResources))]
        public bool WasShipmentAccepted { get; set; }

        [Required(ErrorMessageResourceName = "QuantityRequired", ErrorMessageResourceType = typeof(ReceiptViewModelResources))]
        [Display(Name = "ActualQuantityLabel", ResourceType = typeof(ReceiptViewModelResources))]
        public decimal? ActualQuantity { get; set; }

        [Required(ErrorMessageResourceName = "UnitsRequired", ErrorMessageResourceType = typeof(ReceiptViewModelResources))]
        public ShipmentQuantityUnits? Units { get; set; }

        [Display(Name = "RejectionReasonLabel", ResourceType = typeof(ReceiptViewModelResources))]
        public string RejectionReason { get; set; }

        [Display(Name = "RejectionFurtherInformationLabel", ResourceType = typeof(ReceiptViewModelResources))]
        public string RejectionFurtherInformation { get; set; }

        public SelectList RejectionReasonsSelectList
        {
            get
            {
                return new SelectList(new[]
                {
                    "Illegal shipment",
                    "Unplanned shipment",
                    "An accident",
                    "Site inspection",
                    "Rejected by consignee",
                    "Refused entry by competent authority",
                    "Unauthorised shipment",
                    "Waste not as specified",
                    "Abandoned waste",
                    "Unrecovered waste"
              });
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
            ReceivedDate = new OptionalDateInputViewModel();
            PossibleUnits = new List<ShipmentQuantityUnits>();
        }

        public bool IsComplete()
        {
            var rejectionComplete = WasShipmentAccepted || !string.IsNullOrWhiteSpace(RejectionReason);

            return ReceivedDate.IsCompleted
                   && ActualQuantity.HasValue
                   && Units.HasValue
                   && rejectionComplete;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ReceivedDate.IsCompleted)
            {
                yield return new ValidationResult(ReceiptViewModelResources.ReceivedDateRequired, new[] { "ReceivedDate.Day" });
            }

            if (!WasShipmentAccepted && string.IsNullOrWhiteSpace(RejectionReason))
            {
                yield return new ValidationResult(ReceiptViewModelResources.RejectReasonRequired, new[] { "RejectionReason" });
            }
        }
    }
}