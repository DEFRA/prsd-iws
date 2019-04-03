namespace EA.Iws.Web.Areas.AddressBook.ViewModels
{
    using System;
    using Core.AddressBook;
    using Core.Shared;
    using Requests.AddressBook;

    public class AddAddressViewModel
    {
        public AddAddressViewModel()
        {
            Address = new AddressData();

            Contact = new ContactData();

            Business = new AddressBusinessTypeViewModel();
        }

        public Guid UserId { get; set; }

        public AddressRecordType Type { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public AddressBusinessTypeViewModel Business { get; set; }
    }
}