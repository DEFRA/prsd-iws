namespace EA.Iws.RequestHandlers.Tests.Unit.Notification
{
    using System;
    using System.Threading.Tasks;
    using EA.Iws.Domain.NotificationAssessment;
    using EA.Iws.RequestHandlers.NotificationAssessment;
    using EA.Iws.Requests.ImportNotificationAssessment;
    using FakeItEasy;
    using Xunit;

    public class DeleteNotificationCommentHandlerTests
    {
        private readonly INotificationCommentRepository repo;

        private readonly DeleteNotificationCommentHandler handler;
        private readonly DeleteNotificationComment message;

        public DeleteNotificationCommentHandlerTests()
        {
            this.repo = A.Fake<INotificationCommentRepository>();

            this.message = A.Fake<DeleteNotificationComment>();
            this.handler = new DeleteNotificationCommentHandler(this.repo);
        }

        [Fact]
        public async Task DeleteComment_ReturnsTrue()
        {
            A.CallTo(() => repo.Delete(A<Guid>.Ignored)).Returns(true);

            var result = await handler.HandleAsync(this.message);

            Assert.Equal(true, result);
            A.CallTo(() => this.repo.Delete(A<Guid>.Ignored))
                .MustHaveHappened();
        }

        [Fact]
        public async Task DeleteComment_NotExist_ReturnsFalse()
        {
            Guid commentId = Guid.NewGuid();
            A.CallTo(() => repo.Delete(commentId)).Returns(true);

            var result = await handler.HandleAsync(this.message);

            Assert.Equal(false, result);
        }
    }
}
