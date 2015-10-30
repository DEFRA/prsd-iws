namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.Shipment
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Core.Shared;
    using Web.ViewModels.Shared;

    public class ShipmentViewModel
    {
        public ShipmentViewModel()
        {
            UnitsSelectList = new SelectList(Prsd.Core.Helpers.EnumHelper.GetValues(typeof(ShipmentQuantityUnits)), "Key", "Value");
            StartDate = new OptionalDateInputViewModel();
            EndDate = new OptionalDateInputViewModel();
        }

        [Display(Name = "NumberOfShipments", ResourceType = typeof(ShipmentViewModelResources))]
        public int? TotalShipments { get; set; }

        [Display(Name = "TotalIntendedQuantity", ResourceType = typeof(ShipmentViewModelResources))]
        public decimal? TotalQuantity { get; set; }

        public ShipmentQuantityUnits? Units { get; set; }
        public SelectList UnitsSelectList { get; set; }
        public OptionalDateInputViewModel StartDate { get; set; }
        public OptionalDateInputViewModel EndDate { get; set; }
    }
}