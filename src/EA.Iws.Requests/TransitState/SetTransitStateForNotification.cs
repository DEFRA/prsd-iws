namespace EA.Iws.Requests.TransitState
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;
    using Security;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class SetTransitStateForNotification : IRequest<Guid>
    {
        public Guid? TransitStateId { get; private set; }

        public Guid NotificationId { get; private set; }

        public Guid CountryId { get; private set; }

        public Guid EntryPointId { get; private set; }

        public Guid ExitPointId { get; set; }

        public Guid CompetentAuthorityId { get; private set; }

        public int? OrdinalPosition { get; set; }

        public SetTransitStateForNotification(Guid notificationId, 
            Guid countryId, 
            Guid entryPointId, 
            Guid exitPointId, 
            Guid competentAuthorityId,
            Guid? transitStateId,
            int? ordinalPosition)
        {
            NotificationId = notificationId;
            CountryId = countryId;
            EntryPointId = entryPointId;
            ExitPointId = exitPointId;
            CompetentAuthorityId = competentAuthorityId;
            TransitStateId = transitStateId;
            OrdinalPosition = ordinalPosition;
        }
    }
}
