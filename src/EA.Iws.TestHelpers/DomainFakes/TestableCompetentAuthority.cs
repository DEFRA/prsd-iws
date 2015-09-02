namespace EA.Iws.TestHelpers.DomainFakes
{
    using System;
    using Domain;

    public class TestableCompetentAuthority : CompetentAuthority
    {
        public new Guid Id
        {
            get { return base.Id; }
            set { base.Id = value; }
        }

        public new virtual Country Country
        {
            get { return base.Country; }
            set { base.Country = value; }
        }

        public new string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        public new string Abbreviation
        {
            get { return base.Abbreviation; }
            set { base.Abbreviation = value; }
        }

        public new string Code
        {
            get { return base.Code; }
            set { base.Code = value; }
        }

        public new bool IsSystemUser
        {
            get { return base.IsSystemUser; }
            set { base.IsSystemUser = value; }
        }

        public static CompetentAuthority EnvironmentAgency
        {
            get
            {
                return new TestableCompetentAuthority
                {
                    Abbreviation = "EA",
                    Code = "GB01",
                    Country = TestableCountry.UnitedKingdom,
                    Id = new Guid("A720D22F-FC3A-42D4-9795-BFF543E65BAE"),
                    IsSystemUser = false,
                    Name = "Environment Agency"
                };
            }
        }

        public static CompetentAuthority NaturalResourcesWales
        {
            get
            {
                return new TestableCompetentAuthority
                {
                    Abbreviation = "NRW",
                    Code = "GB04",
                    Country = TestableCountry.UnitedKingdom,
                    Id = new Guid("B3E1E1A6-3B80-4683-B56A-F76A4BADD2AA"),
                    IsSystemUser = false,
                    Name = "Natural Resources Wales"
                };
            }
        }

        public static CompetentAuthority FrenchAuthorityArdeche
        {
            get
            {
                return new TestableCompetentAuthority
                {
                    Code = "FR07",
                    Country = TestableCountry.France,
                    Id = new Guid("09B5C858-2C09-4F00-B19C-1138936233C6"),
                    IsSystemUser = false,
                    Name = "Ardeche"
                };
            }
        }

        public static CompetentAuthority FrenchAuthorityLoiret
        {
            get
            {
                return new TestableCompetentAuthority
                {
                    Code = "FR45",
                    Country = TestableCountry.France,
                    Id = new Guid("1518FF89-F0D8-48D1-9128-2D2A9403F610"),
                    IsSystemUser = false,
                    Name = "Loiret"
                };
            }
        }

        public static CompetentAuthority SwissAuthority
        {
            get
            {
                return new TestableCompetentAuthority
                {
                    Code = "CH",
                    Country = TestableCountry.Switzerland,
                    Id = new Guid("99BCEF4D-1EDA-4AF4-81D2-67CFD5ACCEEB"),
                    IsSystemUser = false,
                    Name = "Eidgenossisches Departement fur Umwelt, Verkehr, Energie und kommunikation UVEK"
                };
            }
        }
    }
}
