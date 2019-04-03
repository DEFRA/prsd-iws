namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.ReviewUserAccess;
    using Core.Notification;
    using Core.Registration.Users;
    using FakeItEasy;
    using Iws.TestHelpers.Helpers;
    using Prsd.Core.Mediator;
    using Requests.SharedUsers;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Xunit;

    public class ReviewUserAccessControllerTests
{
        private readonly ReviewUserAccessController reviewUserAccessController;
        private readonly IMediator mediator;
        private readonly Guid notificationId = new Guid("4AB23CDF-9B24-4598-A302-A69EBB5F2152");

        private readonly Guid sharedUserId = new Guid("BA4A655A-73C4-485F-9628-5154D0CC048A");

        public ReviewUserAccessControllerTests()
        {
            mediator = A.Fake<IMediator>();
            reviewUserAccessController = new ReviewUserAccessController(mediator);
        }
        [Fact]
        public async Task List_ReturnsView()
        {
            var result = await reviewUserAccessController.UserList(notificationId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task List_GetSharedUsersByNotificationId()
        {
            await reviewUserAccessController.UserList(notificationId);

            A.CallTo(
                () =>
                    mediator.SendAsync(A<GetSharedUsersByNotificationId>.That.Matches(p => p.NotificationId == notificationId)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Remove_ReturnsView()
        {
            var result = await reviewUserAccessController.Remove(notificationId, sharedUserId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Remove_Post_RemovesSharedUserFromNotification()
        {
            var model = new RemoveUserViewModel
            {
                SharedUserId = sharedUserId,
                NotificationId = notificationId
            };

            await reviewUserAccessController.Remove(model);

            A.CallTo(() => mediator.SendAsync(A<DeleteSharedUserForNotification>.That.Matches(p => p.SharedId == sharedUserId && p.NotificationId == notificationId)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
