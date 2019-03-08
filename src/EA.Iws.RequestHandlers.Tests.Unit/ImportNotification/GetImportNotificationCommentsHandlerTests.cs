namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.InternalComments;
    using Domain.ImportNotificationAssessment;
    using EA.Iws.Core.Admin;
    using FakeItEasy;
    using ImportNotificationAssessment;
    using Prsd.Core.Mapper;
    using Requests.ImportNotificationAssessment;
    using Xunit;

    public class GetImportNotificationCommentsHandlerTests
    {
        private readonly IImportNotificationCommentRepository repo;
        private readonly IMap<ImportNotificationComment, InternalComment> mapper;

        private readonly GetImportNotificationCommentsHandler handler;
        private readonly GetImportNotificationComments message;

        private readonly Guid notificationId = Guid.NewGuid();

        public GetImportNotificationCommentsHandlerTests()
        {
            this.repo = A.Fake<IImportNotificationCommentRepository>();
            this.mapper = A.Fake<IMap<ImportNotificationComment, InternalComment>>();

               A.CallTo(() => repo.GetPagedComments(A<Guid>.Ignored, A<NotificationShipmentsCommentsType>.Ignored, A<int>.Ignored, A<int>.Ignored, A<DateTime>.Ignored, A<DateTime>.Ignored, A<int>.Ignored)).Returns(this.GetFakeComments());
            this.message = A.Fake<GetImportNotificationComments>();
            this.handler = new GetImportNotificationCommentsHandler(this.repo, this.mapper);
        }

        [Fact]
        public async Task GetImportNotificationComments_ReturnsComments()
        {
            var result = await handler.HandleAsync(this.message);

            Assert.Equal(GetFakeComments().Count, result.NotificationComments.Count);
            A.CallTo(() => this.repo.GetPagedComments(A<Guid>.Ignored, A<NotificationShipmentsCommentsType>.Ignored, A<int>.Ignored, A<int>.Ignored, A<DateTime>.Ignored, A<DateTime>.Ignored, A<int>.Ignored))
               .MustHaveHappened();
        }

        private List<ImportNotificationComment> GetFakeComments()
        {
            return new List<ImportNotificationComment>()
            {
                new ImportNotificationComment(notificationId, Guid.NewGuid().ToString(), "One", 123, DateTime.Now.AddDays(-1)),
                new ImportNotificationComment(notificationId, Guid.NewGuid().ToString(), "One", 0, DateTime.Now.AddDays(-1))
            };
        }
    }
}