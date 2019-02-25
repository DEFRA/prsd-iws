namespace EA.Iws.Requests.AddressBook
{
    using System;
    using System.Collections.Generic;
    using Core.AddressBook;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanEditAddressBook)]
    public class AddImportAddressBookEntry : IRequest<bool>
    {
        public Guid ImportNotificationId { get; private set; }

        public AddImportAddressBookEntry(Guid importNotificationId)
        {
            this.ImportNotificationId = importNotificationId;
        }
    }
}
