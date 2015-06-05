namespace EA.Iws.Requests.TransportRoute
{
    using System;
    using Prsd.Core.Mediator;

    public class AddStateOfExportToNotification : IRequest<Guid>
    {
        public Guid NotificationId { get; private set; }

        public Guid CountryId { get; private set; }

        public Guid EntryOrExitPointId { get; private set; }

        public Guid CompetentAuthorityId { get; private set; }

        public AddStateOfExportToNotification(Guid notificationId, Guid countryId, Guid entryOrExitPointId, Guid competentAuthorityId)
        {
            NotificationId = notificationId;
            CountryId = countryId;
            EntryOrExitPointId = entryOrExitPointId;
            CompetentAuthorityId = competentAuthorityId;
        }
    }
}
