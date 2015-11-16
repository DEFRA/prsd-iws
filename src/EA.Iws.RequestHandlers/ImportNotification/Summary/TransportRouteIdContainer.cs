namespace EA.Iws.RequestHandlers.ImportNotification.Summary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TransportRouteIdContainer
    {
        private List<Guid> competentAuthorityIds = new List<Guid>();
        private List<Guid> entryOrExitPointIds = new List<Guid>();

        public IEnumerable<Guid> CompetentAuthorityIds
        {
            get { return competentAuthorityIds; }
        }

        public IEnumerable<Guid> EntryOfExitPointIds
        {
            get { return entryOrExitPointIds; }
        }

        public void AddCompetentAuthority(Guid? competentAuthority)
        {
            if (competentAuthority.HasValue)
            {
                competentAuthorityIds.Add(competentAuthority.Value);
            }
        }

        public void AddCompetentAuthorities(IEnumerable<Guid?> competentAuthorities)
        {
            if (competentAuthorities != null)
            {
                competentAuthorityIds.AddRange(competentAuthorities.Where(ca => ca.HasValue).Select(ca => ca.Value));
            }
        }

        public void AddEntryOrExitPoint(Guid? entryOrExitPoint)
        {
            if (entryOrExitPoint.HasValue)
            {
                entryOrExitPointIds.Add(entryOrExitPoint.Value);
            }
        }

        public void AddEntryOrExitPoints(IEnumerable<Guid?> entryOrExitPoints)
        {
            if (entryOrExitPoints != null)
            {
                entryOrExitPointIds.AddRange(entryOrExitPoints.Where(eep => eep.HasValue).Select(eep => eep.Value));
            }
        }
    }
}
