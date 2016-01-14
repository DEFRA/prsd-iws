namespace EA.Iws.Requests.Admin.UserAdministration
{
    using System;
    using Core.Admin;
    using Prsd.Core.Mediator;

    public class GetUserByExportNotificationId : IRequest<ChangeUserData>
    {
        public Guid NotificationId { get; private set; }

        public GetUserByExportNotificationId(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
