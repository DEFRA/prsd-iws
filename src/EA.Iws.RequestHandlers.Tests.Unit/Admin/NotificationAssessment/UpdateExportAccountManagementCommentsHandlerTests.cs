namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using Domain.NotificationAssessment;
    using FakeItEasy;
    using RequestHandlers.NotificationAssessment;
    using Requests.NotificationAssessment;
    using Xunit;

    public class UpdateExportAccountManagementCommentsHandlerTests
    {
        private UpdateExportAccountManagementCommentsHandler handler;
        private UpdateExportNotificationAssementComments message;

        private readonly Guid transactionId = new Guid("688CA6BB-63EF-4D5E-A887-7EC952B9810D");
        private readonly INotificationTransactionRepository repo;

        public UpdateExportAccountManagementCommentsHandlerTests()
        {
            repo = A.Fake<INotificationTransactionRepository>();

            handler = new UpdateExportAccountManagementCommentsHandler(repo);
        }

        [Fact]
        public async Task UpdateComment()
        {
            message = new UpdateExportNotificationAssementComments(transactionId, "something");
            var result = await handler.HandleAsync(message);

            Assert.Equal(true, result);

            A.CallTo(() => repo.UpdateById(transactionId, "something")).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
