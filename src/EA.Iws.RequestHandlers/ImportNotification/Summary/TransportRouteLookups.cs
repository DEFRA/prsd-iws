namespace EA.Iws.RequestHandlers.ImportNotification.Summary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain;
    using Domain.TransportRoute;

    internal class TransportRouteLookups
    {
        public IEnumerable<Country> Countries { get; private set; }

        public IEnumerable<CompetentAuthority> CompetentAuthorities { get; private set; }

        public IEnumerable<EntryOrExitPoint> EntryOrExitPoints { get; private set; }

        public TransportRouteLookups(IEnumerable<Country> countries,
            IEnumerable<CompetentAuthority> competentAuthorities,
            IEnumerable<EntryOrExitPoint> entryOrExitPoints)
        {
            Countries = countries;
            CompetentAuthorities = competentAuthorities;
            EntryOrExitPoints = entryOrExitPoints;
        }

        public Country GetCountry(Guid? id)
        {
            GuardId(id);
            return Countries.Single(c => c.Id == id.Value);
        }

        public CompetentAuthority GetCompetentAuthority(Guid? id)
        {
            GuardId(id);
            return CompetentAuthorities.Single(ca => ca.Id == id.Value);
        }

        public EntryOrExitPoint GetEntryOrExitPoint(Guid? id)
        {
            GuardId(id);
            return EntryOrExitPoints.Single(eep => eep.Id == id.Value);
        }

        private static void GuardId(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ArgumentNullException("Id cannot be null");
            }
        }
    }
}
