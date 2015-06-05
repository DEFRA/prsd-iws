namespace EA.Iws.Requests.TransportRoute
{
    using System;
    using Shared;

    public class StateOfExportData
    {
        public Guid CountryId { get; set; }

        public string CountryName { get; set; }

        public Guid CompetentAuthorityId { get; set; }

        public CompetentAuthorityData CompetentAuthority { get; set; }

        public Guid ExitPointId { get; set; }

        public string ExitPointName { get; set; }
    }
}