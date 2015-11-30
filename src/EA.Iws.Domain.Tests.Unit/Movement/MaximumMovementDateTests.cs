namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using System.Threading.Tasks;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using NotificationConsent;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class MaximumMovementDateTests
    {
        private static readonly Guid MovementId = new Guid("A29A6558-6279-4D7A-B79B-47650F6E1B1A");
        private static readonly Guid AnyGuid = new Guid("ABEEF53B-C456-4E64-A552-190CF38502F2");

        private readonly IMovementRepository movementRepository;
        private readonly INotificationConsentRepository consentRepository;
        private readonly ValidMovementDateCalculator calculator;

        public MaximumMovementDateTests()
        {
            var historyRepository = A.Fake<IMovementDateHistoryRepository>();
            var notificationRepository = A.Fake<INotificationApplicationRepository>();
            var originalDate = new OriginalMovementDate(historyRepository);
            var workingDayCalculator = A.Fake<IWorkingDayCalculator>();

            // Adding 10 working days to a working day always spans 2 weekends,
            // so it is sufficient to add 14 days for this test.
            A.CallTo(() =>
                workingDayCalculator
                    .AddWorkingDays(A<DateTime>.Ignored,
                        10,
                        A<bool>.Ignored,
                        A<UKCompetentAuthority>.Ignored))
                    .ReturnsLazily((DateTime start,
                        int days,
                        bool incStartDay,
                        UKCompetentAuthority ca) => start.AddDays(14));

            movementRepository = A.Fake<IMovementRepository>();
            consentRepository = A.Fake<INotificationConsentRepository>();

            calculator = new ValidMovementDateCalculator(movementRepository,
                notificationRepository,
                consentRepository,
                originalDate,
                workingDayCalculator);
        }

        [Fact]
        public async Task ReturnsOriginalDatePlus10WorkingDays()
        {
            var expectedDate = new DateTime(2015, 1, 19);
            A.CallTo(() => consentRepository.GetByNotificationId(A<Guid>.Ignored))
                .Returns(new Consent(AnyGuid, new DateRange(new DateTime(2014, 1, 1), new DateTime(2016, 1, 1)), "AnyString", AnyGuid));
            A.CallTo(() => movementRepository.GetById(MovementId))
                .Returns(new TestableMovement { Date = new DateTime(2015, 1, 5) });

            var result = await calculator.Maximum(MovementId);

            Assert.Equal(expectedDate, result);
        }

        [Fact]
        public async Task ReturnsConsentEndDateIfThisIsCloser()
        {
            var consentEnd = new DateTime(2015, 1, 10);
            A.CallTo(() => consentRepository.GetByNotificationId(A<Guid>.Ignored))
                .Returns(new Consent(AnyGuid, new DateRange(new DateTime(2014, 1, 1), consentEnd), "AnyString", AnyGuid));
            A.CallTo(() => movementRepository.GetById(MovementId))
                .Returns(new TestableMovement { Date = new DateTime(2015, 1, 5) });

            var result = await calculator.Maximum(MovementId);

            Assert.Equal(consentEnd, result);
        }
    }
}