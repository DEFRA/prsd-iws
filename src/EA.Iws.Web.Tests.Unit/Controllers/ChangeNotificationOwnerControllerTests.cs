namespace EA.Iws.Web.Tests.Unit.Controllers
{
    using Core.Notification;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.SharedUsers;
    using Requests.Users;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using TestHelpers;
    using Web.Controllers;
    using Web.ViewModels.ChangeNotificationOwner;
    using Xunit;
    public class ChangeNotificationOwnerControllerTests
    {
        private readonly IMediator mediator;
        private readonly ChangeNotificationOwnerController changeOwnerController;
        private readonly Guid notificationId = new Guid("88DCCF29-8785-44D5-A08E-46203A4F09CC");
        private readonly string externalUserEmail = "externalUser@test.com";

        public ChangeNotificationOwnerControllerTests()
        {
            mediator = A.Fake<IMediator>();
            changeOwnerController = new ChangeNotificationOwnerController(mediator);         
        }

        [Fact]
        public async Task RedirectsToConfirm_ForValidEmail()
        {
            var model = new ChangeOwnerViewModel(this.notificationId);
            model.EmailAddress = externalUserEmail;

            A.CallTo(
             () =>
             mediator.SendAsync(A<ExternalUserExists>.That.Matches(p => p.EmailAddress == externalUserEmail.ToString())))
             .Returns(true);

            A.CallTo(
          () => mediator.SendAsync(A<GetSharedUsersByNotificationId>._)).Returns(new List<NotificationSharedUser>
          {
               new NotificationSharedUser
                {
                    Id = new Guid("4345FB05-F7DF-4E16-939C-C09FCA5C7D7B"),
                    Email = "test1.fake.com"
                },
                new NotificationSharedUser
                {
                    Id = new Guid("29B0D09E-BA77-49FB-AF95-4171408C07C9"),
                    Email = "test2.fake.com"
                }
         });

            var result = await changeOwnerController.Index(model) as RedirectToRouteResult;

            Assert.True(ViewModelValidator.ValidateViewModel(model).Count == 0);

            Assert.Equal("Confirm", result.RouteValues["action"]);
        }

        [Fact]
        public async Task RedirectsToReviewAccess_ForSharedUserEmail()
        {
            var model = new ChangeOwnerViewModel(this.notificationId);
            model.EmailAddress = externalUserEmail;

            A.CallTo(
             () =>
             mediator.SendAsync(A<ExternalUserExists>.That.Matches(p => p.EmailAddress == externalUserEmail.ToString())))
             .Returns(true);

            A.CallTo(
          () => mediator.SendAsync(A<GetSharedUsersByNotificationId>._)).Returns(new List<NotificationSharedUser>
          {
               new NotificationSharedUser
                {
                    Id = new Guid("4345FB05-F7DF-4E16-939C-C09FCA5C7D7B"),
                    Email = "test1.fake.com"
                },
                new NotificationSharedUser
                {
                    Id = new Guid("29B0D09E-BA77-49FB-AF95-4171408C07C9"),
                    Email = externalUserEmail
                }
         });

            var result = await changeOwnerController.Index(model) as RedirectToRouteResult;

            Assert.True(ViewModelValidator.ValidateViewModel(model).Count == 0);

            Assert.Equal("ReviewAccess", result.RouteValues["action"]);
        }
        [Fact]
        public async Task ThrowsError_ForInValidEmail()
        {
            var model = new ChangeOwnerViewModel(this.notificationId);
            model.EmailAddress = externalUserEmail;

            A.CallTo(
             () =>
             mediator.SendAsync(A<ExternalUserExists>.That.Matches(p => p.EmailAddress == externalUserEmail.ToString())))
             .Returns(false);
            var result = await changeOwnerController.Index(model) as ViewResult;
            Assert.True(result.ViewData.ModelState.Count == 1);
        }
    }
}
