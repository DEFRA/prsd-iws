namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using Core.Notification;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Requests.SharedUsers;
    using Requests.Users;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using TestHelpers;
    using Web.Controllers;
    using Web.ViewModels.ShareNotification;
    using Xunit;

    public class AddSharedUserForNotificationControllerTests
    {
        private readonly ShareNotificationController shareNotificationOptionController;
        private readonly Guid notificationId = new Guid();
        private readonly Guid userId = new Guid();
        private readonly string externalEmail = "external@fake.com";
        private readonly string internalEmail = "internal@fake.com";
        private readonly IMediator mediator;

        public AddSharedUserForNotificationControllerTests()
        {
            mediator = A.Fake<IMediator>();

            A.CallTo(
               () =>
               mediator.SendAsync(A<AddSharedUser>.That.Matches(p => p.UserId == userId.ToString())))
               .Returns(true);

            shareNotificationOptionController = new ShareNotificationController(mediator);

            A.CallTo(
                () =>
                mediator.SendAsync(A<GetUserId>.That.Matches(p => p.EmailAddress == externalEmail)))
                .Returns(userId);

            A.CallTo(
                () =>
                mediator.SendAsync(A<ExternalUserExists>.That.Matches(p => p.EmailAddress == externalEmail)))
                .Returns(true);
        }

        [Fact]
        public void ShareNotification_Get_ReturnsCorrectView()
        {
            var result = shareNotificationOptionController.Index(this.notificationId) as ViewResult;

            Assert.IsType<ShareNotificationViewModel>(result.Model);
        }

        [Fact]
        public async Task Add_Email_SuccessfullyAddsToList()
        {
            var model = new ShareNotificationViewModel(this.notificationId);
            model.EmailAddress = this.externalEmail;

            var result = await shareNotificationOptionController.Index(this.notificationId, model, "addshareduser", string.Empty) as ViewResult;

            var resultModel = result.Model as ShareNotificationViewModel;

            Assert.Equal(1, resultModel.SelectedSharedUsers.Count);
        }

        [Fact]
        public async Task Remove_Email_SuccessfullyRemovesFromList()
        {
            var model = new ShareNotificationViewModel(this.notificationId);
            model.SelectedSharedUsers = this.CreateSharedUserList(1);
            model.EmailAddress = model.SelectedSharedUsers[0].Email;

            var result = await shareNotificationOptionController.Index(this.notificationId, model, string.Empty, model.SelectedSharedUsers[0].UserId.ToString()) as ViewResult;

            var resultModel = result.Model as ShareNotificationViewModel;

            Assert.Equal(0, resultModel.SelectedSharedUsers.Count);
        }

        [Fact]
        public async Task Add_Email_AlreadyInList_DoesntAdd()
        {
            var model = new ShareNotificationViewModel(this.notificationId);

            model.SelectedSharedUsers = this.CreateSharedUserList(1);

            model.EmailAddress = model.SelectedSharedUsers[0].Email;

            var result = await shareNotificationOptionController.Index(this.notificationId, model, "addshareduser", string.Empty) as ViewResult;

            var resultModel = result.Model as ShareNotificationViewModel;

            Assert.Equal(1, resultModel.SelectedSharedUsers.Count);
        }
        
        [Fact]
        public void Add_Email_MaximumEmailCountReached()
        {
            var model = new ShareNotificationViewModel(this.notificationId);

            model.SelectedSharedUsers = this.CreateSharedUserList(5);

            Assert.True(ViewModelValidator.ValidateViewModel(model).Count > 0);
        }

        [Fact]
        public void Add_Email_EmailNotValidFormat()
        {
            var model = new ShareNotificationViewModel(this.notificationId);

            model.EmailAddress = "fake";

            Assert.True(ViewModelValidator.ValidateViewModel(model).Count > 0);
        }

        [Fact]
        public async Task Add_InternalEmail_ReturnsError()
        {
            A.CallTo(
               () =>
               mediator.SendAsync(A<ExternalUserExists>.That.Matches(p => p.EmailAddress == internalEmail)))
               .Returns(false);

            var model = new ShareNotificationViewModel(this.notificationId);
            model.EmailAddress = this.internalEmail;

            var result = await shareNotificationOptionController.Index(this.notificationId, model, "addshareduser", string.Empty) as ViewResult;

            Assert.True(result.ViewData.ModelState.Count == 1, "Email address can't be an internal user");
        }

        [Fact]
        public async Task Add_UnknownEmail_ReturnsError()
        {
            A.CallTo(
                () =>
                mediator.SendAsync(A<GetUserId>.That.Matches(p => p.EmailAddress == "fake@fake.com")))
                .Throws(new Exception());

            var model = new ShareNotificationViewModel(this.notificationId);
            model.EmailAddress = "fake@fake.com";

            var result = await shareNotificationOptionController.Index(this.notificationId, model, "addshareduser", string.Empty) as ViewResult;

            Assert.True(result.ViewData.ModelState.Count == 1, "Enter a valid email address");
        }

        [Fact]
        public async Task Add_UserAlreadyShared_ReturnsError()
        {
            List<NotificationSharedUser> users = new List<NotificationSharedUser>()
            {
                new NotificationSharedUser()
                {
                    UserId = this.userId.ToString()
                }
            };

            A.CallTo(
                () =>
                mediator.SendAsync(A<GetSharedUsersByNotificationId>.That.Matches(p => p.NotificationId == this.notificationId)))
                .Returns(users);

            var model = new ShareNotificationViewModel(this.notificationId, users);
            
            var result = await shareNotificationOptionController.Index(this.notificationId, model, "addshareduser", null) as ViewResult;

            Assert.True(result.ViewData.ModelState.Count == 1, "This email address has already been added as a shared user");
        }

        [Fact]
        public async Task Add_ConfirmAndAddUser_ReturnsSuccessView()
        {
            List<NotificationSharedUser> users = new List<NotificationSharedUser>()
            {
                new NotificationSharedUser()
                {
                    UserId = this.userId.ToString()
                }
            };

            this.shareNotificationOptionController.TempData["SharedUsers"] = users;

            var result = await shareNotificationOptionController.Confirm(this.notificationId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public void SuccessAction_ReturnsView()
        {
            var result = this.shareNotificationOptionController.Success(this.notificationId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        private List<NotificationSharedUser> CreateSharedUserList(int numberOfUsers)
        {
            List<NotificationSharedUser> returnList = new List<NotificationSharedUser>();

            for (int i = 0; i < numberOfUsers; i++)
            {
                returnList.Add(new NotificationSharedUser()
                {
                    Email = string.Format("{0}@fake.com", i),
                    UserId = Guid.NewGuid().ToString()
                });
            }
            return returnList;
        }
    }
}
