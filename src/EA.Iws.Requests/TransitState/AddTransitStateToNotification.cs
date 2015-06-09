namespace EA.Iws.Requests.TransitState
{
    using System;
    using Prsd.Core.Mediator;

    public class AddTransitStateToNotification : IRequest<Guid>
    {
        public Guid NotificationId { get; private set; }

        public Guid CountryId { get; private set; }

        public Guid EntryPointId { get; private set; }

        public Guid ExitPointId { get; set; }

        public Guid CompetentAuthorityId { get; private set; }

        public AddTransitStateToNotification(Guid notificationId, Guid countryId, Guid entryPointId, Guid exitPointId, Guid competentAuthorityId)
        {
            NotificationId = notificationId;
            CountryId = countryId;
            EntryPointId = entryPointId;
            ExitPointId = exitPointId;
            CompetentAuthorityId = competentAuthorityId;
        }
    }
}
