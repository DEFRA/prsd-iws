namespace EA.Iws.Requests.ImportNotification.Facilities
{
    using EA.Iws.Core.Authorization;
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Iws.Core.ImportNotification.Summary;
    using System;
    using EA.Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanEditImportContactDetails)]
    public class SetFacilityDetailsForImportNotification : IRequest<Unit>
    {
        public SetFacilityDetailsForImportNotification(Guid importNotificationId, Facility facilityDetails)
        {
            ImportNotificationId = importNotificationId;
            FacilityDetails = facilityDetails;
        }

        public Guid ImportNotificationId { get; private set; }

        public Facility FacilityDetails { get; private set; }
    }
}
