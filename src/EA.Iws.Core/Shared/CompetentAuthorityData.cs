namespace EA.Iws.Core.Shared
{
    using System;

    public class CompetentAuthorityData
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Abbreviation { get; set; }

        public bool IsSystemUser { get; set; }

        public string Code { get; set; }

        public Guid CountryId { get; set; }
    }
}