namespace EA.Iws.TestHelpers.DomainFakes
{
    using Domain;
    using Helpers;

    public class TestableAddress : Address
    {
        public new string Address1
        {
            get { return base.Address1; }
            set { ObjectInstantiator<Address>.SetProperty(x => x.Address1, value, this); }
        }

        public new string Address2
        {
            get { return base.Address2; }
            set { ObjectInstantiator<Address>.SetProperty(x => x.Address2, value, this); }
        }

        public new string TownOrCity
        {
            get { return base.TownOrCity; }
            set { ObjectInstantiator<Address>.SetProperty(x => x.TownOrCity, value, this); }
        }

        public new string Region
        {
            get { return base.Region; }
            set { ObjectInstantiator<Address>.SetProperty(x => x.Region, value, this); }
        }

        public new string PostalCode
        {
            get { return base.PostalCode; }
            set { ObjectInstantiator<Address>.SetProperty(x => x.PostalCode, value, this); }
        }

        public new string Country
        {
            get { return base.Country; }
            set { ObjectInstantiator<Address>.SetProperty(x => x.Country, value, this); }
        }

        public static Address SouthernHouse
        {
            get
            {
                return new TestableAddress
                {
                    Address1 = "Southern House",
                    Address2 = "Station Approach",
                    Country = "United Kingdom",
                    TownOrCity = "Woking",
                    PostalCode = "GU22 7UY",
                    Region = "Surrey"
                };
            }
        }

        public static Address AddlestoneAddress
        {
            get
            {
                return new TestableAddress
                {
                    Address1 = "Export House",
                    Address2 = "High St",
                    TownOrCity = "Addlestone",
                    Country = "United Kingdom",
                    PostalCode = "KT15 1TM",
                    Region = "Surrey"
                };
            }
        }

        public static Address WitneyAddress
        {
            get
            {
                return new TestableAddress
                {
                    Address1 = "1 Lower Street",
                    TownOrCity = "Witney",
                    Country = "United Kingdom",
                    PostalCode = "OX28 6DG"
                };
            }
        }

        public static Address FrenchAddress
        {
            get
            {
                return new TestableAddress
                {
                    Address1 = "127 Rue Saint-Mart\u00E9n",
                    PostalCode = "28100",
                    TownOrCity = "Dreux",
                    Country = "France"
                };
            }
        }
    }
}
