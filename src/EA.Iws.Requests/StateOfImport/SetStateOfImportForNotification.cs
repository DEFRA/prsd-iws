namespace EA.Iws.Requests.StateOfImport
{
    using System;
    using Authorization;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class SetStateOfImportForNotification : IRequest<Guid>
    {
        public Guid NotificationId { get; private set; }

        public Guid CountryId { get; private set; }

        public Guid EntryOrExitPointId { get; private set; }

        public Guid CompetentAuthorityId { get; private set; }

        public SetStateOfImportForNotification(Guid notificationId, Guid countryId, Guid entryOrExitPointId, Guid competentAuthorityId)
        {
            NotificationId = notificationId;
            CountryId = countryId;
            EntryOrExitPointId = entryOrExitPointId;
            CompetentAuthorityId = competentAuthorityId;
        }
    }
}