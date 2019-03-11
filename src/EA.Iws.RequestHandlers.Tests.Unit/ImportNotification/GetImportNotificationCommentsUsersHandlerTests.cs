namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EA.Iws.Core.Admin;
    using EA.Iws.Domain;
    using EA.Iws.Domain.ImportNotificationAssessment;
    using EA.Iws.RequestHandlers.ImportNotificationAssessment;
    using EA.Iws.Requests.ImportNotificationAssessment;
    using FakeItEasy;
    using Xunit;

    public class GetImportNotificationCommentsUsersHandlerTests
    {
        private readonly IImportNotificationCommentRepository repo;
        private readonly IInternalUserRepository internalUserRepository;

        private readonly GetImportNotificationCommentsUsersHandler handler;
        private readonly GetImportNotificationCommentsUsers message;

        private readonly Guid notificationId = Guid.NewGuid();

        public GetImportNotificationCommentsUsersHandlerTests()
        {
            this.repo = A.Fake<IImportNotificationCommentRepository>();
            this.internalUserRepository = A.Fake<IInternalUserRepository>();

            A.CallTo(() => repo.GetUsers(A<Guid>.Ignored, A<NotificationShipmentsCommentsType>.Ignored)).Returns(this.GetFakeUsers());
            A.CallTo(() => internalUserRepository.GetByUserId(A<Guid>.Ignored)).Returns(GetFakeInternalUser());
            this.message = A.Fake<GetImportNotificationCommentsUsers>();
            this.handler = new GetImportNotificationCommentsUsersHandler(this.repo, this.internalUserRepository);
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
