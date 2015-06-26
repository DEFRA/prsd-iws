namespace EA.Iws.Domain.TransportRoute
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class TransitState : Entity, IState, IExitPoint, IEntryPoint
    {
        public virtual Country Country { get; protected set; }

        public virtual CompetentAuthority CompetentAuthority { get; protected set; }

        public virtual EntryOrExitPoint ExitPoint {get; protected set; }

        public virtual EntryOrExitPoint EntryPoint { get; protected set; }

        public int OrdinalPosition { get; protected set; }

        protected TransitState()
        {
        }

        public TransitState(Country country, CompetentAuthority competentAuthority, EntryOrExitPoint entryPoint, EntryOrExitPoint exitPoint, int ordinalPosition)
        {
            Guard.ArgumentNotNull(() => country, country);
            Guard.ArgumentNotNull(() => competentAuthority, competentAuthority);
            Guard.ArgumentNotNull(() => entryPoint, entryPoint);
            Guard.ArgumentNotNull(() => exitPoint, exitPoint);
            Guard.ArgumentNotZeroOrNegative(() => OrdinalPosition, ordinalPosition);

            if (country.Id != competentAuthority.Country.Id 
                || country.Id != entryPoint.Country.Id
                || country.Id != exitPoint.Country.Id)
            {
                throw new InvalidOperationException(string.Format("Transit State Competent Authority, Entry and Exit Point must all have the same country. Competent Authority: {0}. Entry: {1}. Exit: {2}. Country: {3}",
                    competentAuthority.Id,
                    entryPoint.Id,
                    exitPoint.Id,
                    country.Name));
            }

            if (entryPoint.Id == exitPoint.Id)
            {
                throw new InvalidOperationException(string.Format("Transit State cannot have same Entry and Exit Point. Entry: {0} Exit: {1}.", entryPoint.Id, exitPoint.Id));
            }

            Country = country;
            CompetentAuthority = competentAuthority;
            ExitPoint = exitPoint;
            EntryPoint = entryPoint;
            OrdinalPosition = ordinalPosition;
        }

        public void UpdateTransitState(Country country, 
            CompetentAuthority competentAuthority, 
            EntryOrExitPoint entryPoint,
            EntryOrExitPoint exitPoint, 
            int? ordinalPosition)
        {
            Guard.ArgumentNotNull(() => country, country);
            Guard.ArgumentNotNull(() => competentAuthority, competentAuthority);
            Guard.ArgumentNotNull(() => entryPoint, entryPoint);
            Guard.ArgumentNotNull(() => exitPoint, exitPoint);

            this.CompetentAuthority = competentAuthority;
            this.Country = country;
            this.EntryPoint = entryPoint;
            this.ExitPoint = exitPoint;
            this.OrdinalPosition = ordinalPosition ?? this.OrdinalPosition;
        }
    }
}