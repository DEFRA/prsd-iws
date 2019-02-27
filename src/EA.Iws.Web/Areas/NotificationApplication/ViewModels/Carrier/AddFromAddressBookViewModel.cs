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
            CarriersList = new SelectList(carriers.OrderBy(order => order.BusinessData.Name)
                .ThenBy(order => order.AddressData.StreetOrSuburb)
                .ThenBy(order => order.AddressData.Address2)
                .ThenBy(order => order.AddressData.TownOrCity)
                .ThenBy(order => order.AddressData.PostalCode)
                .ThenBy(order => order.ContactData.FullName)
                .ThenBy(order => order.BusinessData.RegistrationNumber)
                .Select(c => new SelectListItem()
            {
                Text = c.BusinessData.Name + ", " 
                    + c.AddressData.ToString() + ", " + "\n"
                    + c.ContactData.FullName + ", " + "\n"
                    + c.BusinessData.RegistrationNumber 
                    + (string.IsNullOrEmpty(c.BusinessData.AdditionalRegistrationNumber)
                        ? string.Empty
                        : ", " + c.BusinessData.AdditionalRegistrationNumber),
                Value = c.Id.ToString()
            }), "Value", "Text", SelectedCarrier);
        }

        public Guid? SelectedCarrier { get; set; }

        public SelectList CarriersList { get; set; }

        public List<Guid> SelectedCarriers { get; set; } 
    }
}