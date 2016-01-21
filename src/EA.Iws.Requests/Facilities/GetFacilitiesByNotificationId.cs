namespace EA.Iws.Requests.Facilities
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Facilities;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetFacilitiesByNotificationId : IRequest<IList<FacilityData>>
    {
        public Guid NotificationId { get; set; }

        public GetFacilitiesByNotificationId(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}