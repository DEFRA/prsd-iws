namespace EA.Iws.Core.ImportNotification.Draft
{
    using System.Collections.Generic;
    using System.Linq;

    public class FacilityCollection
    {
        public List<Facility> Facilities { get; set; }

        public Facility SiteOfExport
        {
            get
            {
                if (Facilities == null)
                {
                    return null;
                }

                return Facilities.SingleOrDefault(f => f.IsSiteOfExport);
            }
        }

        public FacilityCollection()
        {
            Facilities = new List<Facility>();
        }
    }
}
