namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Notification;
    using Core.Rules;
    using Domain;
    using Domain.NotificationApplication;
    using Domain.NotificationConsent;
    using FakeItEasy;
    using RequestHandlers.NotificationMovements.BulkUpload;
    using Xunit;

    public class PrenotificationContentThreeWorkingDaysRuleTests
    {
        private readonly PrenotificationContentThreeWorkingDaysConsentDateRule rule;
        private readonly INotificationConsentRepository consentRepository;
        private readonly IWorkingDayCalculator workingDayCalculator;
        private readonly Guid notificationId;

        public PrenotificationContentThreeWorkingDaysRuleTests()
        {
            consentRepository = A.Fake<INotificationConsentRepository>();
            workingDayCalculator = A.Fake<IWorkingDayCalculator>();
            var notificationApplicationRepository = A.Fake<INotificationApplicationRepository>();

            rule = new PrenotificationContentThreeWorkingDaysConsentDateRule(consentRepository, workingDayCalculator,
                notificationApplicationRepository);

            notificationId = Guid.NewGuid();
        }

        [Fact]
        public async Task GetResult_ConsentExpired_Success()
        {
            // Set the consent end date to be yesterday.
            A.CallTo(() => consentRepository.GetByNotificationId(notificationId))
                .Returns(new Consent(notificationId, new DateRange(DateTime.Now.AddMonths(-12), DateTime.Now.AddDays(-1)),
                    string.Empty, Guid.NewGuid()));

            var result = await rule.GetResult(GetTestData(), notificationId);

            Assert.Equal(BulkMovementContentRules.ThreeWorkingDaysToConsentDate, result.Rule);
            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }

        [Fact]
        public async Task GetResult_ConsentNotExpired_ShipmentDateMoreThanThreeDays_Sucess()
        {
            // Set the consent end date to be 12 months from now.
            A.CallTo(() => consentRepository.GetByNotificationId(notificationId))
                .Returns(new Consent(notificationId, new DateRange(DateTime.Now, DateTime.Now.AddMonths(12)),
                    string.Empty, Guid.NewGuid()));

            A.CallTo(
                () =>
                    workingDayCalculator.GetWorkingDays(A<DateTime>.Ignored, A<DateTime>.Ignored, true,
                        A<UKCompetentAuthority>.Ignored)).Returns(4);

            var result = await rule.GetResult(GetTestData(), notificationId);

            Assert.Equal(BulkMovementContentRules.ThreeWorkingDaysToConsentDate, result.Rule);
            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }

        [Fact]
        public async Task GetResult_ConsentNotExpired_ShipmentWithinThreeDays_Error()
        {
            // Set the consent end date to be 12 months from now.
            A.CallTo(() => consentRepository.GetByNotificationId(notificationId))
                .Returns(new Consent(notificationId, new DateRange(DateTime.Now, DateTime.Now.AddMonths(12)),
                    string.Empty, Guid.NewGuid()));

            A.CallTo(
                () =>
                    workingDayCalculator.GetWorkingDays(A<DateTime>.Ignored, A<DateTime>.Ignored, true,
                        A<UKCompetentAuthority>.Ignored)).Returns(2);

            var result = await rule.GetResult(GetTestData(), notificationId);

            Assert.Equal(MessageLevel.Error, result.MessageLevel);
        }

        private static List<PrenotificationMovement> GetTestData()
        {
            return new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    NotificationNumber = "GB 0001 001234",
                    ShipmentNumber = 1,
                    ActualDateOfShipment = DateTime.Now.AddMonths(1)
                },
                new PrenotificationMovement()
                {
                    NotificationNumber = "GB 0001 001234",
                    ShipmentNumber = 2,
                    ActualDateOfShipment = DateTime.Now.AddMonths(2)
                },
                new PrenotificationMovement()
                {
                    NotificationNumber = "GB 0001 001234",
                    ShipmentNumber = 3,
                    ActualDateOfShipment = DateTime.Now.AddMonths(3)
                }
            };
        }
    }
}
