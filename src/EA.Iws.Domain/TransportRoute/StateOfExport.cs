namespace EA.Iws.Domain.TransportRoute
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class StateOfExport : Entity, IState, IExitPoint
    {
        public virtual Country Country { get; protected set; }

        public virtual CompetentAuthority CompetentAuthority { get; protected set; }

        public virtual EntryOrExitPoint ExitPoint { get; protected set; }

        protected StateOfExport()
        {
        }

        public StateOfExport(Country country, CompetentAuthority competentAuthority, EntryOrExitPoint exitPoint)
        {
            Guard.ArgumentNotNull(() => country, country);
            Guard.ArgumentNotNull(() => competentAuthority, competentAuthority);
            Guard.ArgumentNotNull(() => exitPoint, exitPoint);

            if (country.Id != competentAuthority.Country.Id ||
                country.Id != exitPoint.Country.Id)
            {
                var message =
                    string.Format(
                        @"The country of the export state must be the same as the competent authority country and exit point country.
                                Country: {0} Competent Authority: {1} Exit Point: {2}", country.Id, competentAuthority.Country.Id, exitPoint.Country.Id);
                throw new InvalidOperationException(message);
            }

            Country = country;
            CompetentAuthority = competentAuthority;
            ExitPoint = exitPoint;
        }

        public void Update(Country country, CompetentAuthority competentAuthority, EntryOrExitPoint exitPoint)
        {
            Guard.ArgumentNotNull(() => country, country);
            Guard.ArgumentNotNull(() => competentAuthority, competentAuthority);
            Guard.ArgumentNotNull(() => exitPoint, exitPoint);

            if (country.Id != competentAuthority.Country.Id || country.Id != exitPoint.Country.Id)
            {
                var message =
                    string.Format(
                        @"The country of the export state must be the same as the competent authority country and exit point country.
                                Country: {0} Competent Authority: {1} Exit Point: {2}", country.Id, competentAuthority.Country.Id, exitPoint.Country.Id);
                throw new InvalidOperationException(message);
            }

            Country = country;
            CompetentAuthority = competentAuthority;
            ExitPoint = exitPoint;
        }
    }
}