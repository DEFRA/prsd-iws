namespace EA.Iws.Web.Areas.ImportNotification.ViewModels
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

        [Display(Name = "Number of shipments")]
        public int? TotalShipments { get; set; }

        [Display(Name = "Total intended quantity")]
        public decimal? TotalQuantity { get; set; }

        public ShipmentQuantityUnits? Units { get; set; }
        public SelectList UnitsSelectList { get; set; }
        public OptionalDateInputViewModel StartDate { get; set; }
        public OptionalDateInputViewModel EndDate { get; set; }
    }
}