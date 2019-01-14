namespace EA.Iws.Requests.Notification
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanViewSearchResults)]
    public class GetNotificationUpdateHistory : IRequest<IList<NotificationUpdateHistory>>
    {
        public GetNotificationUpdateHistory(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}
