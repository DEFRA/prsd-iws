namespace EA.Iws.Requests.Admin.EntryOrExitPoints
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core;
    using Prsd.Core.Mediator;

    [RequestAuthorization(SystemConfigurationPermissions.CanAddNewEntryOrExitPoint)]
    public class CheckEntryOrExitPointUnique : IRequest<bool>
    {
        public Guid CountryId { get; private set; }

        public string Name { get; private set; }

        public CheckEntryOrExitPointUnique(Guid countryId, string name)
        {
            Guard.ArgumentNotNullOrEmpty(() => name, name);

            CountryId = countryId;
            Name = name;
        }
    }
}
