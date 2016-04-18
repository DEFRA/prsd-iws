namespace EA.Iws.Requests.AddressBook
{
    using System;
    using Core.AddressBook;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanReadAddressBook)]
    public class GetAddressBookRecordById : IRequest<AddressBookRecordData>
    {
        public Guid Id { get; private set; }

        public AddressRecordType Type { get; private set; }

        public GetAddressBookRecordById(Guid id, AddressRecordType type)
        {
            Id = id;
            Type = type;
        }
    }
}
