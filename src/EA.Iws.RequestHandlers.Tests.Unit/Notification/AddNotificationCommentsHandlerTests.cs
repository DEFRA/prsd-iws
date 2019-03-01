namespace EA.Iws.RequestHandlers.Tests.Unit.Notification
{
    using System.Threading.Tasks;
    using Domain.NotificationAssessment;
    using FakeItEasy;
    using NotificationAssessment;
    using Requests.NotificationAssessment;
    using Xunit;

    public class AddNotificationCommentsHandlerTests
    {
        private readonly INotificationCommentRepository repo;

        private readonly AddNotificationCommentHandler handler;
        private readonly AddNotificationComment message;

        public AddNotificationCommentsHandlerTests()
        {
            this.repo = A.Fake<INotificationCommentRepository>();

            A.CallTo(() => repo.Add(A<NotificationComment>.Ignored)).Returns(Task.FromResult(true));
            this.message = A.Fake<AddNotificationComment>();
            this.handler = new AddNotificationCommentHandler(this.repo);
        }

        [Fact]
        public async Task AddNewNotification_ReturnsTrue()
        {
            var result = await handler.HandleAsync(this.message);

            Assert.Equal(true, result);
            A.CallTo(() => this.repo.Add(A<NotificationComment>.Ignored))
               .MustHaveHappened();
        }
    }
}
