namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using Core.Shared;
    using Domain.Movement;
    using Domain.Movement.BulkUpload;
    using FakeItEasy;
    using Prsd.Core;
    using Prsd.Core.Mapper;
    using RequestHandlers.NotificationMovements.BulkReceiptRecovery;
    using Requests.NotificationMovements.BulkUpload;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class PerformReceiptRecoveryContentValidationHandlerTests
    {
        private readonly PerformReceiptRecoveryContentValidationHandler handler;
        private readonly IEnumerable<IReceiptRecoveryContentRule> contentRules;
        private readonly IMap<DataTable, List<ReceiptRecoveryMovement>> mapper;
        private readonly IReceiptRecoveryContentRule contentRule;
        private readonly IDraftMovementRepository repository;
        private readonly IMovementRepository movementRepository;
        private const int MaxShipments = 50;

        public PerformReceiptRecoveryContentValidationHandlerTests()
        {
            mapper = A.Fake<IMap<DataTable, List<ReceiptRecoveryMovement>>>();
            contentRule = A.Fake<IReceiptRecoveryContentRule>();
            repository = A.Fake<IDraftMovementRepository>();
            movementRepository = A.Fake<IMovementRepository>();

            contentRules = new List<IReceiptRecoveryContentRule>(1)
            {
                contentRule
            };

            handler = new PerformReceiptRecoveryContentValidationHandler(contentRules, mapper, repository,
                movementRepository);
        }

        [Fact]
        public async Task ExceedsMaxRows_ContentRulesFailed()
        {
            var notificationId = Guid.NewGuid();
            var summary = new ReceiptRecoveryRulesSummary();
            var message = new PerformReceiptRecoveryContentValidation(summary, notificationId, new DataTable(), "Test", false);

            A.CallTo(() => mapper.Map(A<DataTable>.Ignored))
                .Returns(A.CollectionOfFake<ReceiptRecoveryMovement>(MaxShipments + 1).ToList());

            var response = await handler.HandleAsync(message);

            Assert.False(response.IsContentRulesSuccess);
        }

        [Fact]
        public async Task MissingShipmentNumber_ContentRulesFailed()
        {
            var notificationId = Guid.NewGuid();
            var summary = new ReceiptRecoveryRulesSummary();
            var message = new PerformReceiptRecoveryContentValidation(summary, notificationId, new DataTable(), "Test", false);

            var movements = new List<ReceiptRecoveryMovement>()
            {
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = null,
                    MissingShipmentNumber = true,
                    NotificationNumber = "GB 0001 001234",
                    MissingNotificationNumber = false
                }
            };

            A.CallTo(() => mapper.Map(A<DataTable>.Ignored)).Returns(movements);

            var response = await handler.HandleAsync(message);

            Assert.False(response.IsContentRulesSuccess);
        }

        [Fact]
        public async Task MissingNotificationNumber_ContentRulesFailed()
        {
            var notificationId = Guid.NewGuid();
            var summary = new ReceiptRecoveryRulesSummary();
            var message = new PerformReceiptRecoveryContentValidation(summary, notificationId, new DataTable(), "Test", false);

            var movements = new List<ReceiptRecoveryMovement>()
            {
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = 1,
                    MissingShipmentNumber = false,
                    MissingNotificationNumber = true
                }
            };

            A.CallTo(() => mapper.Map(A<DataTable>.Ignored)).Returns(movements);

            var response = await handler.HandleAsync(message);

            Assert.False(response.IsContentRulesSuccess);
        }

        [Fact]
        public async Task InvalidNotificationNumber_ContentRulesFailed()
        {
            var notificationId = Guid.NewGuid();
            var summary = new ReceiptRecoveryRulesSummary();
            var message = new PerformReceiptRecoveryContentValidation(summary, notificationId, new DataTable(), "Test", false);

            var movements = new List<ReceiptRecoveryMovement>()
            {
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = 1,
                    MissingShipmentNumber = false,
                    NotificationNumber = null,
                    MissingNotificationNumber = false
                }
            };

            A.CallTo(() => mapper.Map(A<DataTable>.Ignored)).Returns(movements);

            var response = await handler.HandleAsync(message);

            Assert.False(response.IsContentRulesSuccess);
        }

        [Fact]
        public async Task MissingReceiptAndRecoveryData_ContentRulesFailed()
        {
            var movements = new List<ReceiptRecoveryMovement>()
            {
                new ReceiptRecoveryMovement()
                {
                    NotificationNumber = "GB 0001 123456",
                    ShipmentNumber = 1,
                    MissingReceivedDate = true,
                    MissingRecoveredDisposedDate = true,
                    MissingQuantity = false,
                    MissingUnits = false
                },
                new ReceiptRecoveryMovement()
                {
                    NotificationNumber = "GB 0001 123456",
                    ShipmentNumber = 2,
                    MissingReceivedDate = true,
                    MissingRecoveredDisposedDate = true,
                    MissingQuantity = true,
                    MissingUnits = false
                },
                new ReceiptRecoveryMovement()
                {
                    NotificationNumber = "GB 0001 123456",
                    ShipmentNumber = 3,
                    MissingReceivedDate = false,
                    MissingQuantity = false,
                    MissingUnits = true
                }
            };

            var notificationId = Guid.NewGuid();
            var summary = new ReceiptRecoveryRulesSummary();
            var message = new PerformReceiptRecoveryContentValidation(summary, notificationId, new DataTable(), "Test", false);

            A.CallTo(() => mapper.Map(A<DataTable>.Ignored)).Returns(movements);

            var response = await handler.HandleAsync(message);

            Assert.False(response.IsContentRulesSuccess);
        }

        [Fact]
        public async Task ContentRulesFailed_DoesNotSaveToDraft()
        {
            var notificationId = Guid.NewGuid();
            var summary = new ReceiptRecoveryRulesSummary();
            var message = new PerformReceiptRecoveryContentValidation(summary, notificationId, new DataTable(), "Test", false);

            A.CallTo(() => mapper.Map(A<DataTable>.Ignored)).Returns(A.CollectionOfFake<ReceiptRecoveryMovement>(5).ToList());
            A.CallTo(() => contentRule.GetResult(A<List<ReceiptRecoveryMovement>>.Ignored, notificationId))
                .Returns(new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.MaximumShipments,
                    MessageLevel.Error, "Missing data", 0));

            var response = await handler.HandleAsync(message);

            Assert.False(response.IsContentRulesSuccess);
            A.CallTo(() => repository.AddReceiptRecovery(A<Guid>.Ignored, A<List<ReceiptRecoveryMovement>>.Ignored, "Test")).MustNotHaveHappened();
        }

        [Fact]
        public async Task ContentRulesSuccess_SavesToDraft()
        {
            var notificationId = Guid.NewGuid();
            var summary = new ReceiptRecoveryRulesSummary();
            var message = new PerformReceiptRecoveryContentValidation(summary, notificationId, new DataTable(), "Test", false);

            var movements = new List<ReceiptRecoveryMovement>()
            {
                new ReceiptRecoveryMovement()
                {
                    NotificationNumber = "GB 0001 123456",
                    ShipmentNumber = 1,
                    Quantity = 1m,
                    Unit = ShipmentQuantityUnits.Tonnes,
                    ReceivedDate = SystemTime.UtcNow,
                    RecoveredDisposedDate = SystemTime.UtcNow
                }
            };

            A.CallTo(() => movementRepository.GetAllMovements(notificationId)).Returns(GetRepoMovements());
            A.CallTo(() => mapper.Map(A<DataTable>.Ignored)).Returns(movements);
            A.CallTo(() => contentRule.GetResult(A<List<ReceiptRecoveryMovement>>.Ignored, notificationId))
                .Returns(new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.MaximumShipments,
                    MessageLevel.Success, "Test", 0));

            var response = await handler.HandleAsync(message);

            Assert.True(response.IsContentRulesSuccess);
            A.CallTo(() => repository.AddReceiptRecovery(A<Guid>.Ignored, A<List<ReceiptRecoveryMovement>>.Ignored, "Test"))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        private List<Movement> GetRepoMovements()
        {
            var notificationId = Guid.NewGuid();

            return new List<Movement>()
            {
                new TestableMovement()
                {
                    NotificationId = notificationId,
                    Status = Core.Movement.MovementStatus.Submitted,
                    Date = SystemTime.UtcNow.AddDays(-10),
                    Number = 1
                },

                new TestableMovement()
                {
                    NotificationId = notificationId,
                    Status = Core.Movement.MovementStatus.Submitted,
                    Date = SystemTime.UtcNow.AddDays(-5),
                    Number = 2
                }
            };
        }
    }
}
