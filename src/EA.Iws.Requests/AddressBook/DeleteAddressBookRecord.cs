namespace EA.Iws.Requests.AddressBook
{
    using System;
    using Core.AddressBook;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanEditAddressBook)]
    public class DeleteAddressBookRecord : IRequest<bool>
    {
        public Guid Id { get; private set; }

        public AddressRecordType Type { get; private set; }

        public DeleteAddressBookRecord(Guid id, AddressRecordType type)
        {
            Id = id;
            Type = type;
        }
    }
}
