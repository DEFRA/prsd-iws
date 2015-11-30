namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Shared;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using NotificationConsent;
    using Prsd.Core;
    using TestHelpers.Helpers;
    using Xunit;

    public class MovementDateValidatorTests : IDisposable
    {
        private static readonly Guid NotificationId = new Guid("C30BAC4B-F01F-4D20-98CB-AF74A651BA15");
        private static readonly Guid AnyGuid = new Guid("68B67470-9237-428F-B57C-8C94001510BA");
        private static readonly DateTime Today = new DateTime(2015, 1, 1);
        private static readonly DateTime ConsentStart = Today.AddMonths(-6);
        private static readonly DateTime ConsentEnd = Today.AddMonths(6);

        private const string AnyString = "test";

        private readonly MovementDateValidator validator;
        private readonly INotificationConsentRepository consentRepository;
        private readonly OriginalMovementDate originalMovementDate;
        private readonly IWorkingDayCalculator workingDayCalculator;
        private readonly INotificationApplicationRepository notificationRepository;
        private readonly IMovementDateHistoryRepository historyRepository;

        public MovementDateValidatorTests()
        {
            consentRepository = A.Fake<INotificationConsentRepository>();
            notificationRepository = A.Fake<INotificationApplicationRepository>();
            historyRepository = A.Fake<IMovementDateHistoryRepository>();
            workingDayCalculator = A.Fake<IWorkingDayCalculator>();

            originalMovementDate = new OriginalMovementDate(historyRepository);
            validator = new MovementDateValidator(consentRepository,
                originalMovementDate,
                workingDayCalculator,
                notificationRepository);

            SystemTime.Freeze(Today);

            A.CallTo(() => consentRepository.GetByNotificationId(NotificationId))
                .Returns(new Consent(
                    NotificationId,
                    new DateRange(ConsentStart, ConsentEnd),
                    AnyString,
                    AnyGuid));

            A.CallTo(() => notificationRepository.GetByMovementId(A<Guid>.Ignored))
                .Returns(new NotificationApplication(
                    AnyGuid,
                    NotificationType.Recovery,
                    UKCompetentAuthority.England,
                    10));

            A.CallTo(() => historyRepository.GetByMovementId(A<Guid>.Ignored))
                .Returns(new MovementDateHistory[0]);
        }

        [Theory]
        [InlineData(MovementStatus.New)]
        [InlineData(MovementStatus.Cancelled)]
        [InlineData(MovementStatus.Completed)]
        [InlineData(MovementStatus.Received)]
        [InlineData(MovementStatus.Rejected)]
        public async Task MovementStatusNotSubmitted_Throws(MovementStatus status)
        {
            var movement = new Movement(1, NotificationId, Today);
            ObjectInstantiator<Movement>.SetProperty(x => x.Status, status, movement);

            var validDate = Today.AddDays(3);

            await Assert.ThrowsAsync<MovementDateException>(() =>
                validator.EnsureDateValid(movement, validDate));
        }

        [Fact]
        public async Task DateInPast_Throws()
        {
            var movement = new Movement(1, NotificationId, Today);
            ObjectInstantiator<Movement>.SetProperty(x => x.Status, MovementStatus.Submitted, movement);

            var pastDate = Today.AddDays(-1);

            await Assert.ThrowsAsync<MovementDateException>(() =>
                validator.EnsureDateValid(movement, pastDate));
        }

        [Fact]
        public async Task DateOutsideConsentRange_Throws()
        {
            var movement = new Movement(1, NotificationId, ConsentEnd.AddDays(-1));
            ObjectInstantiator<Movement>.SetProperty(x => x.Status, MovementStatus.Submitted, movement);
            var dateOutsideConstentRange = ConsentEnd.AddDays(1);

            await Assert.ThrowsAsync<MovementDateException>(() =>
                validator.EnsureDateValid(movement, dateOutsideConstentRange));
        }

        [Fact]
        public async Task Date10WorkingsDaysGreaterThanOriginalDate_Throws()
        {
            var movement = new Movement(1, NotificationId, Today);
            ObjectInstantiator<Movement>.SetProperty(x => x.Status, MovementStatus.Submitted, movement);

            var newDate = Today.AddDays(15);

            // Adding 10 working days is equivalent to adding 14 calendar days (ignoring bank holidays etc.)
            A.CallTo(() => workingDayCalculator.AddWorkingDays(Today, 10, true, A<UKCompetentAuthority>.Ignored))
                .Returns(Today.AddDays(14));

            await Assert.ThrowsAsync<MovementDateException>(() =>
                validator.EnsureDateValid(movement, newDate));
        }

        [Fact]
        public async Task ValidDate_DoesNotThrow()
        {
            var movement = new Movement(1, NotificationId, Today);
            ObjectInstantiator<Movement>.SetProperty(x => x.Status, MovementStatus.Submitted, movement);

            var validDate = Today.AddDays(3);

            // Adding 10 working days is equivalent to adding 14 calendar days (ignoring bank holidays etc.)
            A.CallTo(() => workingDayCalculator.AddWorkingDays(Today, 10, false, A<UKCompetentAuthority>.Ignored))
                .Returns(Today.AddDays(14));

            await validator.EnsureDateValid(movement, validDate);

            // No assert required
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }
    }
}