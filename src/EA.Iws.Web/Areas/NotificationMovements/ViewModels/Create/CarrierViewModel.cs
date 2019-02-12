namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Create
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.AddressBook;
    using Core.Carriers;

    public class CarrierViewModel
    {
        public CarrierViewModel()
        {
            SelectedCarriers = new List<CarrierList>();
            SelectedCarriersId = new List<Guid>();
        }
        public void SetCarriers(IEnumerable<CarrierData> carriers)
        {
            CarriersList = new SelectList(carriers.Select(c => new SelectListItem()
            {
                Text = c.Business.Name + ", " + c.Address.ToString(),
                Value = c.Id.ToString()
            }), "Value", "Text", SelectedCarriersId);
        }

        [Required(ErrorMessage = "Select a carrier from the list")]
        public Guid SelectedCarrier { get; set; }

        public SelectList CarriersList { get; set; }

        public List<CarrierList> SelectedCarriers { get; set; }

        public List<Guid> SelectedCarriersId { get; set; }
        public IEnumerable<Guid> MovementIds { get; set; }
    }
}