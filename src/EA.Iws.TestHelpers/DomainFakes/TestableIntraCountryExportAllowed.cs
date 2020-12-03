namespace EA.Iws.TestHelpers.DomainFakes
{
    using System;
    using Domain;
    using EA.Iws.Core.Notification;

    public class TestableIntraCountryExportAllowed : IntraCountryExportAllowed
    {
        public new UKCompetentAuthority ExportCompetentAuthority
        {
            get { return base.ExportCompetentAuthority; }
            set { base.ExportCompetentAuthority = value; }
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
                    ExportCompetentAuthority = UKCompetentAuthority.England,
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
                    ExportCompetentAuthority = UKCompetentAuthority.England,
                    ImportCompetentAuthorityId = new Guid("09B5C858-2C09-4F00-B19C-1138936233C6"),
                };
            }
        }
    }
}
