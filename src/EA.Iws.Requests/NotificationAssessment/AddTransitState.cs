namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanAddRemoveTransitState)]
    public class AddTransitState : IRequest<Unit>
    {
        public AddTransitState(Guid notificationId,
            Guid countryId,
            Guid entryPointId,
            Guid exitPointId,
            Guid competentAuthorityId)
        {
            NotificationId = notificationId;
            CountryId = countryId;
            EntryPointId = entryPointId;
            ExitPointId = exitPointId;
            CompetentAuthorityId = competentAuthorityId;
        }

        public Guid NotificationId { get; private set; }

        public Guid CountryId { get; private set; }

        public Guid EntryPointId { get; private set; }

        public Guid ExitPointId { get; private set; }

        public Guid CompetentAuthorityId { get; private set; }
    }
}