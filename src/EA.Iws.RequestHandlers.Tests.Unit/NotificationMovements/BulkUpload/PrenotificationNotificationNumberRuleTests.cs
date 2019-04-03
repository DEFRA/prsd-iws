namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using RequestHandlers.NotificationMovements.BulkPrenotification;
    using Xunit;

    public class PrenotificationNotificationNumberRuleTests
    {
        private readonly PrenotificationNotificationNumberRule rule;
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly Guid notificationId;
        private const string NotificationNumber = "GB 0001 123456";

        public PrenotificationNotificationNumberRuleTests()
        {
            notificationId = Guid.NewGuid();

            notificationApplicationRepository = A.Fake<INotificationApplicationRepository>();

            A.CallTo(() => notificationApplicationRepository.GetNumber(notificationId)).Returns(NotificationNumber);

            rule = new PrenotificationNotificationNumberRule(notificationApplicationRepository);
        }

        [Fact]
        public async Task MatchingNotificationNumbers_Success()
        {
            var movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    NotificationNumber = NotificationNumber,
                    ShipmentNumber = 1
                },
                new PrenotificationMovement()
                {
                    NotificationNumber = NotificationNumber,
                    ShipmentNumber = 2
                }
            };

            var result = await rule.GetResult(movements, notificationId);

            Assert.Equal(PrenotificationContentRules.WrongNotificationNumber, result.Rule);
            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }

        [Fact]
        public async Task WrongNotificationNumbers_Error()
        {
            var movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    NotificationNumber = "Test",
                    ShipmentNumber = 1
                },
                new PrenotificationMovement()
                {
                    NotificationNumber = "12345",
                    ShipmentNumber = 2
                }
            };

            var result = await rule.GetResult(movements, notificationId);

            Assert.Equal(PrenotificationContentRules.WrongNotificationNumber, result.Rule);
            Assert.Equal(MessageLevel.Error, result.MessageLevel);
        }
    }
}
