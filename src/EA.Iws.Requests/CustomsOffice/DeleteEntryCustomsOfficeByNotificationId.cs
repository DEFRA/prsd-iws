namespace EA.Iws.Requests.CustomsOffice
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class DeleteEntryCustomsOfficeByNotificationId : IRequest<bool>
    {
        public DeleteEntryCustomsOfficeByNotificationId(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}
