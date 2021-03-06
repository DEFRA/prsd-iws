﻿namespace EA.Iws.Requests.Carriers
{
    using System;
    using Authorization;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class AddCarrierToNotification : IRequest<Guid>
    {
        public Guid NotificationId { get; set; }

        public BusinessInfoData Business { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }
    }
}