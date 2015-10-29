namespace EA.Iws.TestHelpers.DomainFakes
{
    using System;
    using Domain;
    using Domain.TransportRoute;
    using Helpers;

    public class TestableEntryOrExitPoint : EntryOrExitPoint
    {
        public new Guid Id
        {
            get { return base.Id; }
            set { ObjectInstantiator<EntryOrExitPoint>.SetProperty(x => x.Id, value, this); }
        }

        public new string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        public new Country Country
        {
            get { return base.Country; }
            set { base.Country = value; }
        }

        public static EntryOrExitPoint Dover
        {
            get
            {
                return new TestableEntryOrExitPoint
                {
                    Id = new Guid("59D4E707-98B9-4650-B859-5700EA5AE5E6"),
                    Name = "Dover",
                    Country = TestableCountry.UnitedKingdom
                };
            }
        }

        public static EntryOrExitPoint Hull
        {
            get
            {
                return new TestableEntryOrExitPoint
                {
                    Id = new Guid("970C80BF-947E-4F56-AC5F-3214556B4298"),
                    Name = "Hull",
                    Country = TestableCountry.UnitedKingdom
                };
            }
        }

        public static EntryOrExitPoint Calais
        {
            get
            {
                return new TestableEntryOrExitPoint
                {
                    Id = new Guid("BB6891AC-6F35-42C8-A580-37F9EB899E44"),
                    Name = "Calais",
                    Country = TestableCountry.France
                };
            }
        }

        public static EntryOrExitPoint Cherbourg
        {
            get
            {
                return new TestableEntryOrExitPoint
                {
                    Id = new Guid("DEF593F6-66DD-4626-B563-30D60095A64C"),
                    Name = "Cherbourg",
                    Country = TestableCountry.France
                };
            }
        }

        public static EntryOrExitPoint Basel
        {
            get
            {
                return new TestableEntryOrExitPoint
                {
                    Id = new Guid("6B79DE96-DB21-48F5-A98F-0CA27D6A6417"),
                    Name = "Basel",
                    Country = TestableCountry.Switzerland
                };
            }
        }
    }
}
