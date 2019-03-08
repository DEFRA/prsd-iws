namespace EA.Iws.RequestHandlers.Tests.Unit.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.InternalComments;
    using Domain.NotificationAssessment;
    using EA.Iws.Core.Admin;
    using FakeItEasy;
    using NotificationAssessment;
    using Prsd.Core.Mapper;
    using Requests.NotificationAssessment;
    using Xunit;

    public class GetNotificationCommentsHandlerTests
    {
        private readonly INotificationCommentRepository repo;
        private readonly IMap<NotificationComment, InternalComment> mapper;

        private readonly GetNotificationCommentsHandler handler;
        private readonly GetNotificationComments message;

        private readonly Guid notificationId = Guid.NewGuid();

        public GetNotificationCommentsHandlerTests()
        {
            this.repo = A.Fake<INotificationCommentRepository>();
            this.mapper = A.Fake<IMap<NotificationComment, InternalComment>>();

            A.CallTo(() => repo.GetPagedComments(A<Guid>.Ignored, A<NotificationShipmentsCommentsType>.Ignored, A<int>.Ignored, A<int>.Ignored, A<DateTime>.Ignored, A<DateTime>.Ignored, A<int>.Ignored)).Returns(this.GetFakeComments());
            this.message = A.Fake<GetNotificationComments>();
            this.handler = new GetNotificationCommentsHandler(this.repo, this.mapper);
        }

        [Fact]
        public async Task GetNotificationComments_ReturnsComments()
        {
            var result = await handler.HandleAsync(this.message);

            Assert.Equal(GetFakeComments().Count, result.NotificationComments.Count);
            A.CallTo(() => this.repo.GetPagedComments(A<Guid>.Ignored, A<NotificationShipmentsCommentsType>.Ignored, A<int>.Ignored, A<int>.Ignored, A<DateTime>.Ignored, A<DateTime>.Ignored, A<int>.Ignored))
               .MustHaveHappened();
        }

        private List<NotificationComment> GetFakeComments()
        {
            return new List<NotificationComment>()
            {
                new NotificationComment(notificationId, Guid.NewGuid().ToString(), "One", 123, DateTime.Now.AddDays(-1)),
                new NotificationComment(notificationId, Guid.NewGuid().ToString(), "One", 0, DateTime.Now.AddDays(-1))
            };
        }
    }
}