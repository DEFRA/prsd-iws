namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using Domain.ImportNotificationAssessment.Transactions;
    using FakeItEasy;
    using ImportNotificationAssessment;
    using Requests.ImportNotificationAssessment;
    using Xunit;

    public class UpdateImportAccountManagementCommentsHandlerTests
    {
        private UpdateImportAccountManagementCommentsHandler handler;
        private UpdateImportNotificationAssesmentComments message;

        private readonly Guid transactionId = new Guid("688CA6BB-63EF-4D5E-A887-7EC952B9810D");
        private readonly IImportNotificationTransactionRepository repo;

        public UpdateImportAccountManagementCommentsHandlerTests()
        {
            repo = A.Fake<IImportNotificationTransactionRepository>();

            handler = new UpdateImportAccountManagementCommentsHandler(repo);
        }

        [Fact]
        public async Task UpdateComment()
        {
            message = new UpdateImportNotificationAssesmentComments(transactionId, "something");
            var result = await handler.HandleAsync(message);

            Assert.Equal(true, result);

            A.CallTo(() => repo.UpdateById(transactionId, "something")).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
