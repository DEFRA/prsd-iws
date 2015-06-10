namespace EA.Iws.Domain.TransportRoute
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class StateOfImport : Entity, IState, IEntryPoint
    {
        public virtual Country Country { get; protected set; }

        public virtual CompetentAuthority CompetentAuthority { get; protected set; }

        public virtual EntryOrExitPoint EntryPoint { get; protected set; }

        protected StateOfImport()
        {
        }

        public StateOfImport(Country country, CompetentAuthority competentAuthority, EntryOrExitPoint entryPoint)
        {
            Guard.ArgumentNotNull(() => country, country);
            Guard.ArgumentNotNull(() => competentAuthority, competentAuthority);
            Guard.ArgumentNotNull(() => entryPoint, entryPoint);

            if (country.Id != competentAuthority.Country.Id ||
                country.Id != entryPoint.Country.Id)
            {
                var message =
                    string.Format(
                        @"The country of the import state must be the same as the competent authority country and entry point country.
                                Country: {0} Competent Authority: {1} Exit Point: {2}", country.Id, competentAuthority.Country.Id, entryPoint.Country.Id);
                throw new InvalidOperationException(message);
            }

            Country = country;
            CompetentAuthority = competentAuthority;
            EntryPoint = entryPoint;
        }
    }
}