namespace EA.Iws.Requests.TechnologyEmployed
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.TechnologyEmployed;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetTechnologyEmployed : IRequest<TechnologyEmployedData>
    {
        public GetTechnologyEmployed(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}