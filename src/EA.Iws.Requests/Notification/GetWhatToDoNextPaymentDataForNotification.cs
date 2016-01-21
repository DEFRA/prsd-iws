namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetWhatToDoNextPaymentDataForNotification : IRequest<WhatToDoNextPaymentData>
    {
        public Guid Id { get; private set; }

        public GetWhatToDoNextPaymentDataForNotification(Guid id)
        {
            Id = id;
        }
    }
}
