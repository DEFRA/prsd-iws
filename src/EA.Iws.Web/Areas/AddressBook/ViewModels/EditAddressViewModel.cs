namespace EA.Iws.Web.Areas.AddressBook.ViewModels
{
    using System;
    using Core.AddressBook;
    using Core.Shared;
    using Requests.AddressBook;

    public class EditAddressViewModel : AddAddressViewModel
    {
        public Guid AddressBookRecordId { get; set; }
        public int PageNumber { get; set; }

        public EditAddressViewModel()
        {
            Address = new AddressData();

            Contact = new ContactData();

            Business = new AddressBusinessTypeViewModel();
        }

        public EditAddressViewModel(AddressBookRecordData addressbook, AddressRecordType type, int page)
        {
            AddressBookRecordId = addressbook.Id;
            Address = addressbook.AddressData;
            Contact = addressbook.ContactData;
            Business = new AddressBusinessTypeViewModel(addressbook.BusinessData);
            Type = type;
            PageNumber = page;
        }     
    }
}