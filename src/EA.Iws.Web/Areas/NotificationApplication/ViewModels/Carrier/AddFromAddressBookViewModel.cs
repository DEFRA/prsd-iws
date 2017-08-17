namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Carrier
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.AddressBook;

    public class AddFromAddressBookViewModel
    {
        public AddFromAddressBookViewModel()
        {
            SelectedCarriers = new List<Guid>();
        }

        public void SetCarriers(IEnumerable<AddressBookRecordData> carriers)
        {
            CarriersList = new SelectList(carriers.Select(c => new SelectListItem()
            {
                Text = c.BusinessData.Name + ", " + c.AddressData.ToString(),
                Value = c.Id.ToString()
            }), "Value", "Text", SelectedCarrier);
        }

        public Guid? SelectedCarrier { get; set; }

        public SelectList CarriersList { get; set; }

        public List<Guid> SelectedCarriers { get; set; } 
    }
}