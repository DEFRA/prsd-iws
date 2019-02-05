namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.BulkPrenotification;
    using Core.PackagingType;
    using Core.Rules;
    using Core.Shared;
    using Domain.Movement.BulkUpload;
    using FakeItEasy;
    using Prsd.Core;
    using Prsd.Core.Mapper;
    using RequestHandlers.NotificationMovements.BulkPrenotification;
    using Requests.Movement;
    using Xunit;

    public class PerformBulkUploadContentValidationHandlerTests
    {
        private readonly PerformBulkUploadContentValidationHandler handler;
        private readonly IEnumerable<IPrenotificationContentRule> contentRules;
        private readonly IMap<DataTable, List<PrenotificationMovement>> mapper;
        private readonly IDraftMovementRepository repository;
        private readonly IPrenotificationContentRule contentRule;
        private const int MaxShipments = 50;

        public PerformBulkUploadContentValidationHandlerTests()
        {
            mapper = A.Fake<IMap<DataTable, List<PrenotificationMovement>>>();
            repository = A.Fake<IDraftMovementRepository>();
            contentRule = A.Fake<IPrenotificationContentRule>();

            contentRules = new List<IPrenotificationContentRule>(1)
            {
                contentRule
            };

            handler = new PerformBulkUploadContentValidationHandler(contentRules, mapper, repository);
        }

        [Fact]
        public async Task ExceedsMaxRows_ContentRulesFailed()
        {
            var notificationId = Guid.NewGuid();
            var summary = new BulkMovementRulesSummary();
            var message = new PerformBulkUploadContentValidation(summary, notificationId, new DataTable(), "Test", false);

            A.CallTo(() => mapper.Map(A<DataTable>.Ignored))
                .Returns(A.CollectionOfFake<PrenotificationMovement>(MaxShipments + 1).ToList());

            var response = await handler.HandleAsync(message);

            Assert.False(response.IsContentRulesSuccess);
        }

        [Fact]
        public async Task MissingShipmentNumber_ContentRulesFailed()
        {
            var notificationId = Guid.NewGuid();
            var summary = new BulkMovementRulesSummary();
            var message = new PerformBulkUploadContentValidation(summary, notificationId, new DataTable(), "Test", false);

            var movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    ShipmentNumber = null,
                    MissingShipmentNumber = true
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
            var summary = new BulkMovementRulesSummary();
            var message = new PerformBulkUploadContentValidation(summary, notificationId, new DataTable(), "Test", false);

            var movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    MissingNotificationNumber = true
                }
            };

            A.CallTo(() => mapper.Map(A<DataTable>.Ignored)).Returns(movements);

            var response = await handler.HandleAsync(message);

            Assert.False(response.IsContentRulesSuccess);
        }

        [Fact]
        public async Task MissingData_ContentRulesFailed()
        {
            var notificationId = Guid.NewGuid();
            var summary = new BulkMovementRulesSummary();
            var message = new PerformBulkUploadContentValidation(summary, notificationId, new DataTable(), "Test", false);

            var movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    NotificationNumber = "GB 0001 123456",
                    ShipmentNumber = 1,
                    MissingQuantity = true,
                    MissingDateOfShipment = true
                }
            };

            A.CallTo(() => mapper.Map(A<DataTable>.Ignored)).Returns(movements);

            var response = await handler.HandleAsync(message);

            Assert.False(response.IsContentRulesSuccess);
        }

        [Fact]
        public async Task ContentRulesFailed_DoesNotSaveToDraft()
        {
            var notificationId = Guid.NewGuid();
            var summary = new BulkMovementRulesSummary();
            var message = new PerformBulkUploadContentValidation(summary, notificationId, new DataTable(), "Test", false);

            A.CallTo(() => mapper.Map(A<DataTable>.Ignored)).Returns(A.CollectionOfFake<PrenotificationMovement>(5).ToList());
            A.CallTo(() => contentRule.GetResult(A<List<PrenotificationMovement>>.Ignored, notificationId))
                .Returns(new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.MissingData,
                    MessageLevel.Error, "Missing data"));

            var response = await handler.HandleAsync(message);

            Assert.False(response.IsContentRulesSuccess);
            A.CallTo(() => repository.Add(A<Guid>.Ignored, A<List<PrenotificationMovement>>.Ignored, "Test")).MustNotHaveHappened();
        }

        [Fact]
        public async Task ContentRulesSuccess_SavesToDraft()
        {
            var notificationId = Guid.NewGuid();
            var summary = new BulkMovementRulesSummary();
            var message = new PerformBulkUploadContentValidation(summary, notificationId, new DataTable(), "Test", false);

            var movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    NotificationNumber = "GB 0001 123456",
                    ShipmentNumber = 1,
                    Quantity = 1m,
                    Unit = ShipmentQuantityUnits.Tonnes,
                    PackagingTypes = new List<PackagingType>() { PackagingType.Drum, PackagingType.Bag },
                    ActualDateOfShipment = SystemTime.UtcNow
                }
            };

            A.CallTo(() => mapper.Map(A<DataTable>.Ignored)).Returns(movements);
            A.CallTo(() => contentRule.GetResult(A<List<PrenotificationMovement>>.Ignored, notificationId))
                .Returns(new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.MissingData,
                    MessageLevel.Success, "Test"));

            var response = await handler.HandleAsync(message);

            Assert.True(response.IsContentRulesSuccess);
            A.CallTo(() => repository.Add(A<Guid>.Ignored, A<List<PrenotificationMovement>>.Ignored, "Test"))
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
