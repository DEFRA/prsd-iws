namespace EA.Iws.Requests.AddressBook
{
    using Core.AddressBook;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanReadAddressBook)]
    public class GetUserAddressBookByType : IRequest<AddressBookData>
    {
        public AddressRecordType Type { get; private set; }

        public GetUserAddressBookByType(AddressRecordType type)
        {
            Type = type;
        }
    }
}
