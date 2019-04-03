namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using EA.Iws.Domain.ImportNotificationAssessment;
    using EA.Iws.RequestHandlers.ImportNotificationAssessment;
    using EA.Iws.Requests.ImportNotificationAssessment;
    using FakeItEasy;
    using Xunit;

    public class DeleteImportNotificationCommentHandlerTest
    {
        private readonly IImportNotificationCommentRepository repo;

        private readonly DeleteImportNotificationCommentHandler handler;
        private readonly DeleteImportNotificationComment message;

        public DeleteImportNotificationCommentHandlerTest()
        {
            this.repo = A.Fake<IImportNotificationCommentRepository>();

            this.message = A.Fake<DeleteImportNotificationComment>();
            this.handler = new DeleteImportNotificationCommentHandler(this.repo);
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
