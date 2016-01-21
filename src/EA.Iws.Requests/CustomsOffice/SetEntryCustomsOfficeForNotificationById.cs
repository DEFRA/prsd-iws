namespace EA.Iws.Requests.CustomsOffice
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;
    using Security;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class SetEntryCustomsOfficeForNotificationById : IRequest<CustomsOfficeCompletionStatus>
    {
        public Guid Id { get; private set; }
        public Guid CountryId { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }

        public SetEntryCustomsOfficeForNotificationById(Guid id, string name, string address, Guid countryId)
        {
            Id = id;
            Name = name;
            Address = address;
            CountryId = countryId;
        }
    }
}
