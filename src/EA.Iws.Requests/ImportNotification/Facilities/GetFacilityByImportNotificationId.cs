namespace EA.Iws.Requests.ImportNotification.Facilities
{
    using EA.Iws.Core.Authorization;
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Iws.Core.ImportNotification.Summary;
    using EA.Prsd.Core.Mediator;
    using System;

    [RequestAuthorization(ImportNotificationPermissions.CanReadImportNotification)]
    public class GetFacilityByImportNotificationId : IRequest<Facility>
    {
        public Guid ImportNotificationId { get; private set; }

        public GetFacilityByImportNotificationId(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }
    }
}
