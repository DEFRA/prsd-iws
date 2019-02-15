namespace EA.Iws.Requests.AddressBook
{
    using System;
    using Core.AddressBook;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;
    [RequestAuthorization(GeneralPermissions.CanEditAddressBook)]
    public class EditAddressBookEntry : IRequest<bool>
    {
        public Guid Id { get; private set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public BusinessInfoData Business { get; set; }

        public AddressRecordType Type { get; set; }

        public EditAddressBookEntry(Guid id, AddressRecordType type, AddressData address, BusinessInfoData business, ContactData contact)
        {
            Id = id;
            Type = type;
            Business = business;
            Address = address;
            Contact = contact;
        }
    }
}
