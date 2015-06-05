namespace EA.Iws.Requests.TransportRoute
{
    using System;
    using Shared;

    public class StateOfImportData
    {
        public Guid CountryId { get; set; }

        public string CountryName { get; set; }

        public Guid CompetentAuthorityId { get; set; }

        public CompetentAuthorityData CompetentAuthority { get; set; }

        public Guid EntryPointId { get; set; }

        public string EntryPointName { get; set; }
    }
}