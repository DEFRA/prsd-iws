namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using Domain.Movement;
    using FakeItEasy;
    using RequestHandlers.NotificationMovements.BulkReceiptRecovery;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class ReceiptRecoveryReceiptDateValidationRuleTests
    {
        private readonly ReceiptRecoveryReceiptDateValidationRule rule;
        private readonly IMovementRepository repository;
        private readonly Guid notificationId;

        public ReceiptRecoveryReceiptDateValidationRuleTests()
        {
            repository = A.Fake<IMovementRepository>();
            rule = new ReceiptRecoveryReceiptDateValidationRule(repository);
            notificationId = Guid.NewGuid();
        }

        [Theory]
        [InlineData(true, true)]
        public async Task GetResult_ReceiptDateValidation_Success(bool isExistingMovementValid, bool isReceiptRecoveryValid)
        {
            A.CallTo(() => repository.GetAllMovements(notificationId)).Returns(GetTestData_ExistingMovements(isExistingMovementValid));

            var result = await rule.GetResult(GetTestData_ReceiptRecoveryMovements(isReceiptRecoveryValid), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.ReceiptDateValidation, result.Rule);
            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public async Task GetResult_ReceiptDateValidation_Error(bool isExistingMovementValid, bool isReceiptRecoveryValid)
        {
            A.CallTo(() => repository.GetAllMovements(notificationId)).Returns(GetTestData_ExistingMovements(isExistingMovementValid));

            var result = await rule.GetResult(GetTestData_ReceiptRecoveryMovements(isReceiptRecoveryValid), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.ReceiptDateValidation, result.Rule);
            Assert.Equal(MessageLevel.Error, result.MessageLevel);
        }

        private List<Movement> GetTestData_ExistingMovements(bool isSuccess)
        {
            int dateModifier = isSuccess ? -2 : 2;
            return new List<Movement>()
            {
                new TestableMovement()
                {
                    Number = 1,
                    Date = DateTime.UtcNow.AddDays(dateModifier)
                }
            };
        }

        private List<ReceiptRecoveryMovement> GetTestData_ReceiptRecoveryMovements(bool isSuccess)
        {
            int dateModifier = isSuccess ? -1 : 1;
            return new List<ReceiptRecoveryMovement>()
            {
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = 1,
                    ReceivedDate = DateTime.UtcNow.AddDays(dateModifier)
                }
            };
        }
    }
}