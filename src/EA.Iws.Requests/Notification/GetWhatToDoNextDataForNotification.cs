namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetWhatToDoNextDataForNotification : IRequest<WhatToDoNextData>
    {
        public Guid Id { get; private set; }

        public GetWhatToDoNextDataForNotification(Guid id)
        {
            Id = id;
        }
    }
}
