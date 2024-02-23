namespace EA.Iws.Requests.Notification
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using EA.Iws.Core.Notification.AdditionalCharge;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetNotificationAdditionalCharges : IRequest<IList<NotificationAdditionalChargeForDisplay>>
    {
        public GetNotificationAdditionalCharges(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}
