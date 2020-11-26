namespace EA.Iws.TestHelpers.DomainFakes
{
    using System;
    using Domain;

    public class TestableIntraCountryExportAllowed : IntraCountryExportAllowed
    {
        public new Guid ExportCompetentAuthorityId
        {
            get { return base.ExportCompetentAuthorityId; }
            set { base.ExportCompetentAuthorityId = value; }
        }
        public new Guid ImportCompetentAuthorityId
        {
            get { return base.ImportCompetentAuthorityId; }
            set { base.ImportCompetentAuthorityId = value; }
        }

        public static IntraCountryExportAllowed EAToFrance
        {
            get
            {
                return new TestableIntraCountryExportAllowed
                {
                    ExportCompetentAuthorityId = new Guid("A720D22F-FC3A-42D4-9795-BFF543E65BAE"),
                    ImportCompetentAuthorityId = new Guid("09B5C858-2C09-4F00-B19C-1138936233C6"),
                };
            }
        }

        public static IntraCountryExportAllowed NRWToFrance
        {
            get
            {
                return new TestableIntraCountryExportAllowed
                {
                    ExportCompetentAuthorityId = new Guid("B3E1E1A6-3B80-4683-B56A-F76A4BADD2AA"),
                    ImportCompetentAuthorityId = new Guid("09B5C858-2C09-4F00-B19C-1138936233C6"),
                };
            }
        }
    }
}
