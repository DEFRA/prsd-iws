namespace EA.Iws.RequestHandlers.Tests.Unit
{
    using System;
    using TestHelpers.DomainFakes;

    public abstract class TestBase
    {
        private const string SeedGuid = "AF38892D-8936-4519-B0B4-AA6BA2899222";

        protected static readonly Guid UserId = new Guid("2C39454C-CD3B-4155-A280-D06E2BA7273C");
        protected static readonly Guid NotificationId = new Guid("29449B13-0A9A-47FF-984F-FB5AB13DC806");
        protected static readonly Guid NotificationAssessmentId = new Guid("E101DF37-5BCE-4A0C-B1B7-DDE36DB8636B");
        protected static readonly Guid FinancialGuaranteeId = new Guid("AF628805-0B42-45ED-B82C-4422A6CD03A2");
        protected static readonly Guid MovementId = new Guid("1A47AF8C-D103-4464-AB2C-A98F03CA5338");

        protected const string AnyString = "test";
        protected const string TestString = "Paddington Bear";
        protected const int AnyInt = 7;
        protected const int TestInt = 3;
        protected const decimal AnyDecimal = 6.9m;
        protected const decimal TestDecimal = 52.0m;

        protected static readonly DateTime OldestDate = new DateTime(2015, 2, 5);
        protected static readonly DateTime MiddleDate = new DateTime(2015, 7, 2);
        protected static readonly DateTime NewestDate = new DateTime(2015, 9, 2);

        internal readonly TestIwsContext Context;
        internal readonly TestUserContext UserContext;
        protected readonly TestableNotificationApplication NotificationApplication;
        protected readonly TestableMovement Movement;

        protected TestBase()
        {
            UserContext = new TestUserContext(UserId);
            Context = new TestIwsContext(UserContext);

            NotificationApplication = new TestableNotificationApplication
            {
                Id = NotificationId,
                UserId = UserId
            };

            Context.Countries.AddRange(new[]
            {
                TestableCountry.France,
                TestableCountry.Switzerland,
                TestableCountry.UnitedKingdom
            });

            Movement = new TestableMovement
            {
                Id = MovementId,
                NotificationApplication = NotificationApplication,
                NotificationApplicationId = NotificationId
            };
        }

        protected static Guid DeterministicGuid(int seed)
        {
            var seedString = (seed < 0) ? (seed * -1).ToString("D") :
                seed.ToString("D");

            var thisGuid = SeedGuid.Remove(SeedGuid.Length - seedString.Length)
                + seedString;

            return new Guid(thisGuid);
        }
    }
}
