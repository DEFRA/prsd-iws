namespace EA.Iws.Requests.StateOfImport
{
    using System;
    using Prsd.Core.Mediator;

    public class AddStateOfImportToNotification : IRequest<Guid>
    {
        public Guid NotificationId { get; private set; }

        public Guid CountryId { get; private set; }

        public Guid EntryOrExitPointId { get; private set; }

        public Guid CompetentAuthorityId { get; private set; }

        public AddStateOfImportToNotification(Guid notificationId, Guid countryId, Guid entryOrExitPointId, Guid competentAuthorityId)
        {
            NotificationId = notificationId;
            CountryId = countryId;
            EntryOrExitPointId = entryOrExitPointId;
            CompetentAuthorityId = competentAuthorityId;
        }
    }
}
