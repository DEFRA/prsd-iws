namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification
{
    using System.Threading.Tasks;
    using Domain.ImportNotificationAssessment;
    using FakeItEasy;
    using ImportNotificationAssessment;
    using Requests.ImportNotificationAssessment;
    using Xunit;

    public class AddImportNotificationCommentsHandlerTests
    {
        private readonly IImportNotificationCommentRepository repo;

        private AddImportNotificationCommentHandler handler;
        private AddImportNotificationComment message;

        public AddImportNotificationCommentsHandlerTests()
        {
            this.repo = A.Fake<IImportNotificationCommentRepository>();

            A.CallTo(() => repo.Add(A<ImportNotificationComment>.Ignored)).Returns(Task.FromResult(true));
            this.message = A.Fake<AddImportNotificationComment>();
            this.handler = new AddImportNotificationCommentHandler(this.repo);
        }

        [Fact]
        public async Task AddNewNotification_ReturnsTrue()
        {
            var result = await handler.HandleAsync(this.message);

            Assert.Equal(true, result);
            A.CallTo(() => this.repo.Add(A<ImportNotificationComment>.Ignored))
               .MustHaveHappened();
        }
    }
}
