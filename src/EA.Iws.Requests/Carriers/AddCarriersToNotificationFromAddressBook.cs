namespace EA.Iws.Requests.Carriers
{
    using System;
    using Authorization;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class AddCarriersToNotificationFromAddressBook : IRequest<Unit>
    {
        public AddCarriersToNotificationFromAddressBook(Guid notificationId, Guid[] addressBookItemIds)
        {
            NotificationId = notificationId;
            AddressBookItemIds = addressBookItemIds;
        }

        public Guid NotificationId { get; private set; }

        public Guid[] AddressBookItemIds { get; private set; }
    }
}