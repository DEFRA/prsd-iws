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
    using CompetentAuthorityEnum = Core.Notification.UKCompetentAuthority;

    public class MovementDateValidatorTests : IDisposable
    {
        private static readonly Guid NotificationId = new Guid("C30BAC4B-F01F-4D20-98CB-AF74A651BA15");
        private static readonly Guid AnyGuid = new Guid("68B67470-9237-428F-B57C-8C94001510BA");
        private static readonly DateTime Today = new DateTime(2015, 1, 1);
        private static readonly DateTime ConsentStart = Today.AddMonths(-6);
        private static readonly DateTime ConsentEnd = Today.AddMonths(6);

        private const string AnyString = "test";

        private readonly MovementDateValidator dateValidator;
        private readonly UpdatedMovementDateValidator updatedDateValidator;
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

            dateValidator = new MovementDateValidator(consentRepository);
            originalMovementDate = new OriginalMovementDate(historyRepository);
            updatedDateValidator = new UpdatedMovementDateValidator(dateValidator,
                originalMovementDate,
                workingDayCalculator,
                notificationRepository);

            SystemTime.Freeze(Today.AddHours(5));

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
                    CompetentAuthorityEnum.England,
                    10));

            A.CallTo(() => historyRepository.GetByMovementId(A<Guid>.Ignored))
                .Returns(new MovementDateHistory[0]);

            A.CallTo(() => workingDayCalculator.AddWorkingDays(A<DateTime>.Ignored,
                A<int>.Ignored,
                A<bool>.Ignored,
                A<CompetentAuthorityEnum>.Ignored))
                    .ReturnsLazily((DateTime inputDate,
                        int inputDays,
                        bool includeStartDay,
                        CompetentAuthorityEnum ca) =>
                            //A very simple working day formula that ignores bank holidays taken from http://stackoverflow.com/a/279370
                            inputDate.AddDays(inputDays
                                + ((inputDays / 5) * 2)
                                + ((((int)inputDate.DayOfWeek + (inputDays % 5)) >= 5) ? 2 : 0)));
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
                updatedDateValidator.EnsureDateValid(movement, validDate));
        }

        [Fact]
        public async Task DateInPast_Throws()
        {
            var pastDate = Today.AddDays(-1);

            await Assert.ThrowsAsync<MovementDateException>(() =>
                dateValidator.EnsureDateValid(NotificationId, pastDate));
        }

        [Fact]
        public async Task DateOutsideConsentRange_Throws()
        {
            var dateOutsideConstentRange = ConsentEnd.AddDays(1);

            await Assert.ThrowsAsync<MovementDateOutsideConsentPeriodException>(() =>
                dateValidator.EnsureDateValid(NotificationId, dateOutsideConstentRange));
        }

        [Fact]
        public async Task DateOver30DaysInFuture_Throws()
        {
            var date = Today.AddDays(31);

            await Assert.ThrowsAsync<MovementDateOutOfRangeException>(() =>
                dateValidator.EnsureDateValid(NotificationId, date));
        }

        [Fact]
        public async Task DateOf30DaysInFuture_DoesNotThrow()
        {
            var date = Today.AddDays(30);

            await dateValidator.EnsureDateValid(NotificationId, date);

            //No assert required
        }

        [Fact]
        public async Task Today_DoesNotThrow()
        {
            var date = Today;

            await dateValidator.EnsureDateValid(NotificationId, date);

            //No assert required
        }

        [Fact]
        public async Task DateIsStartOfConsentPeriod_DoesNotThrow()
        {
            var consentStart = Today.AddDays(4);

            A.CallTo(() => consentRepository.GetByNotificationId(NotificationId))
                .Returns(new Consent(
                    NotificationId,
                    new DateRange(consentStart, ConsentEnd),
                    AnyString,
                    AnyGuid));

            var date = consentStart;

            await dateValidator.EnsureDateValid(NotificationId, date);

            //No assert required
        }

        [Fact]
        public async Task Date10WorkingsDaysGreaterThanOriginalDate_Throws()
        {
            var movement = new Movement(1, NotificationId, Today);
            ObjectInstantiator<Movement>.SetProperty(x => x.Status, MovementStatus.Submitted, movement);

            //10 workings days after 1 Jan 2015 is 14 calendar days (only counting weekends)...
            var newDate = Today.AddDays(15);

            await Assert.ThrowsAsync<MovementDateOutOfRangeOfOriginalDateException>(() =>
                updatedDateValidator.EnsureDateValid(movement, newDate));
        }

        [Fact]
        public async Task ValidDate_DoesNotThrow()
        {
            var movement = new Movement(1, NotificationId, Today);
            ObjectInstantiator<Movement>.SetProperty(x => x.Status, MovementStatus.Submitted, movement);

            var validDate = Today.AddDays(3);

            await updatedDateValidator.EnsureDateValid(movement, validDate);

            // No assert required
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }
    }
}