namespace EA.Iws.Requests.AddressBook
{
    using Core.AddressBook;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanEditAddressBook)]
    public class AddAddressBookEntry : IRequest<bool>
    {
        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public BusinessInfoData Business { get; set; }

        public AddressRecordType Type { get; set; }
    }
}
