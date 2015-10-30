namespace EA.Iws.Requests.Admin.EntryOrExitPoints
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Mediator;

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
