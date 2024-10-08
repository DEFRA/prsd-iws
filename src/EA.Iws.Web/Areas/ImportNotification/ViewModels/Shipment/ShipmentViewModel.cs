﻿namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.Shipment
{
    using Core.ImportNotification.Draft;
    using Core.Shared;
    using Prsd.Core.Helpers;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Web.ViewModels.Shared;

    public class ShipmentViewModel
    {
        public ShipmentViewModel()
        {
            UnitsSelectList = new SelectList(EnumHelper.GetValues(typeof(ShipmentQuantityUnits)), "Key", "Value");
            StartDate = new OptionalDateInputViewModel();
            EndDate = new OptionalDateInputViewModel();
        }

        public ShipmentViewModel(Shipment data)
        {
            UnitsSelectList = new SelectList(EnumHelper.GetValues(typeof(ShipmentQuantityUnits)), "Key", "Value");
            StartDate = new OptionalDateInputViewModel(data.StartDate);
            EndDate = new OptionalDateInputViewModel(data.EndDate);
            TotalQuantity = data.Quantity;
            Units = data.Unit;
            TotalShipments = data.TotalShipments;
        }

        [Display(Name = "NumberOfShipments", ResourceType = typeof(ShipmentViewModelResources))]
        public int? TotalShipments { get; set; }

        [Display(Name = "TotalIntendedQuantity", ResourceType = typeof(ShipmentViewModelResources))]
        public decimal? TotalQuantity { get; set; }

        public ShipmentQuantityUnits? Units { get; set; }

        public SelectList UnitsSelectList { get; set; }

        [Display(Name = "StartDateDisplay", ResourceType = typeof(ShipmentViewModelResources))]
        public OptionalDateInputViewModel StartDate { get; set; }

        [Display(Name = "EndDateDisplay", ResourceType = typeof(ShipmentViewModelResources))]
        public OptionalDateInputViewModel EndDate { get; set; }
    }
}