namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Create
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
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
            CarriersList = new SelectList(carriers.OrderBy(order => order.Business.Name)
                .ThenBy(order => order.Address.StreetOrSuburb)
                .ThenBy(order => order.Address.Address2)
                .ThenBy(order => order.Address.TownOrCity)
                .ThenBy(order => order.Address.PostalCode)
                .ThenBy(order => order.Contact.FullName)
                .ThenBy(order => order.Business.RegistrationNumber)
                .Select(c => new SelectListItem()
                {
                    Text = c.Business.Name + ", "
                           + c.Address.ToString() + ", " + "\n"
                           + c.Contact.FullName + ", " + "\n"
                           + c.Business.RegistrationNumber
                           + (string.IsNullOrEmpty(c.Business.AdditionalRegistrationNumber)
                               ? string.Empty
                               : ", " + c.Business.AdditionalRegistrationNumber),
                    Value = c.Id.ToString()
                }), "Value", "Text", SelectedCarrier);
        }

        [Required(ErrorMessage = "Select a carrier from the list")]
        public Guid? SelectedCarrier { get; set; }

        public SelectList CarriersList { get; set; }

        public List<CarrierList> SelectedCarriers { get; set; }

        public List<Guid> SelectedCarriersId { get; set; }

        public IEnumerable<Guid> MovementIds { get; set; }
    }
}