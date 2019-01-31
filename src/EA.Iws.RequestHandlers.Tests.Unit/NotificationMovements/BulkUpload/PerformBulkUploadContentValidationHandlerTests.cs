namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;
    using Domain.Movement.BulkUpload;
    using FakeItEasy;
    using Prsd.Core.Mapper;
    using RequestHandlers.NotificationMovements.BulkUpload;
    using Requests.Movement;
    using Xunit;

    public class PerformBulkUploadContentValidationHandlerTests
    {
        private readonly PerformBulkUploadContentValidationHandler handler;
        private readonly IEnumerable<IBulkMovementPrenotificationContentRule> contentRules;
        private readonly IMap<DataTable, List<PrenotificationMovement>> mapper;
        private readonly IDraftMovementRepository repository;
        private readonly IBulkMovementPrenotificationContentRule contentRule;

        public PerformBulkUploadContentValidationHandlerTests()
        {
            mapper = A.Fake<IMap<DataTable, List<PrenotificationMovement>>>();
            repository = A.Fake<IDraftMovementRepository>();
            contentRule = A.Fake<IBulkMovementPrenotificationContentRule>();

            contentRules = new List<IBulkMovementPrenotificationContentRule>(1)
            {
                contentRule
            };

            handler = new PerformBulkUploadContentValidationHandler(contentRules, mapper, repository);
        }

        [Fact]
        public async Task ContentRulesFailed_DoesNotSaveToDraft()
        {
            var notificationId = Guid.NewGuid();
            var summary = new BulkMovementRulesSummary();
            var message = new PerformBulkUploadContentValidation(summary, notificationId, new DataTable(), "Test");

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
            var message = new PerformBulkUploadContentValidation(summary, notificationId, new DataTable(), "Test");

            A.CallTo(() => mapper.Map(A<DataTable>.Ignored)).Returns(A.CollectionOfFake<PrenotificationMovement>(5).ToList());
            A.CallTo(() => contentRule.GetResult(A<List<PrenotificationMovement>>.Ignored, notificationId))
                .Returns(new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.MissingData,
                    MessageLevel.Success, "Missing data"));

            var response = await handler.HandleAsync(message);

            Assert.True(response.IsContentRulesSuccess);
            A.CallTo(() => repository.Add(A<Guid>.Ignored, A<List<PrenotificationMovement>>.Ignored, "Test"))
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
