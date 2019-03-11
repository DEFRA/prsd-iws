namespace EA.Iws.RequestHandlers.Tests.Unit.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EA.Iws.Core.Admin;
    using EA.Iws.Domain;
    using EA.Iws.Domain.NotificationAssessment;
    using EA.Iws.RequestHandlers.NotificationAssessment;
    using EA.Iws.Requests.NotificationAssessment;
    using FakeItEasy;
    using Xunit;

    public class GetNotificationCommentsUsersHandlerTests
    {
        private readonly INotificationCommentRepository repo;
        private readonly IInternalUserRepository internalUserRepository;

        private readonly GetNotificationCommentsUsersHandler handler;
        private readonly GetNotificationCommentsUsers message;

        private readonly Guid notificationId = Guid.NewGuid();

        public GetNotificationCommentsUsersHandlerTests()
        {
            this.repo = A.Fake<INotificationCommentRepository>();
            this.internalUserRepository = A.Fake<IInternalUserRepository>();

            A.CallTo(() => repo.GetUsers(A<Guid>.Ignored, A<NotificationShipmentsCommentsType>.Ignored)).Returns(this.GetFakeUsers());
            A.CallTo(() => internalUserRepository.GetByUserId(A<Guid>.Ignored)).Returns(GetFakeInternalUser());
            this.message = A.Fake<GetNotificationCommentsUsers>();
            this.handler = new GetNotificationCommentsUsersHandler(this.repo, this.internalUserRepository);
        }

        [Fact]
        public async Task GetImportNotificationComments_ReturnsComments()
        {
            var result = await handler.HandleAsync(this.message);

            Assert.Equal(GetFakeUsers().Count, result.Users.Count);
            A.CallTo(() => this.repo.GetUsers(A<Guid>.Ignored, A<NotificationShipmentsCommentsType>.Ignored))
               .MustHaveHappened();
        }

        private List<string> GetFakeUsers()
        {
            return new List<string>()
            {
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString()
            };
        }

        private InternalUser GetFakeInternalUser()
        {
            return A.Fake<InternalUser>();
        }
    }
}
