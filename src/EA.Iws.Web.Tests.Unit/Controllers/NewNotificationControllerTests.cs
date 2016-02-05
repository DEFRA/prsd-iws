namespace EA.Iws.Web.Tests.Unit.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Notification;
    using Core.Shared;
    using FakeItEasy;
    using Prsd.Core.Helpers;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Web.Controllers;
    using Web.ViewModels.NewNotification;
    using Web.ViewModels.Shared;
    using Xunit;

    public class NewNotificationControllerTests
    {
        private static CompetentAuthorityChoiceViewModel CompetentAuthorityChoice
        {
            get
            {
                var competentAuthorityChoice = new CompetentAuthorityChoiceViewModel
                {
                    CompetentAuthorities = new RadioButtonStringCollectionViewModel
                    {
                        SelectedValue = "Test",
                        PossibleValues = new[] { "Test", "String", "Value" }
                    }
                };
                return competentAuthorityChoice;
            }
        }

        private static NewNotificationController CreateNewNotificationController()
        {
            var mediator = A.Fake<IMediator>();
            A.CallTo(() => mediator.SendAsync(A<CreateNotificationApplication>._))
                .Returns(Guid.Empty);
            return new NewNotificationController(mediator);
        }

        [Fact]
        public void CompetentAuthority_Get_ReturnsCorrectView()
        {
            var controller = CreateNewNotificationController();

            var result = controller.CompetentAuthority() as ViewResult;

            Assert.Empty(result.ViewName);
            Assert.IsType<CompetentAuthorityChoiceViewModel>(result.Model);
        }

        [Fact]
        public void CompetentAuthority_Post_RedirectsToCorrectAction()
        {
            var controller = CreateNewNotificationController();

            var competentAuthorityChoice = CompetentAuthorityChoice;

            var result = controller.CompetentAuthority(competentAuthorityChoice, null) as RedirectToRouteResult;

            Assert.Equal("NotificationType", result.RouteValues["action"]);
        }

        [Fact]
        public void CompetentAuthority_PostInvalidModel_ReturnsToCorrectView()
        {
            var controller = CreateNewNotificationController();

            controller.ModelState.AddModelError("Error", "A Test Error");

            var competentAuthorityChoice = CompetentAuthorityChoice;

            var result = controller.CompetentAuthority(competentAuthorityChoice, null) as ViewResult;

            Assert.Empty(result.ViewName);
            Assert.IsType<CompetentAuthorityChoiceViewModel>(result.Model);
        }

        [Fact]
        public void NotificationTypeQuestion_Get_ReturnsCorrectView()
        {
            var controller = CreateNewNotificationController();
            var result = controller.NotificationType(EnumHelper.GetDisplayName(UKCompetentAuthority.England), EnumHelper.GetDisplayName(NotificationType.Recovery)) as ViewResult;

            Assert.Empty(result.ViewName);
            Assert.IsType<NotificationTypeViewModel>(result.Model);
        }

        [Fact]
        public async Task NotificationTypeQuestion_Post_RedirectsToCorrectAction()
        {
            var controller = CreateNewNotificationController();
            var model = new NotificationTypeViewModel();
            model.SelectedNotificationType = NotificationType.Recovery;
            var result = await controller.NotificationType(model, null) as RedirectToRouteResult;

            Assert.Equal("Created", result.RouteValues["action"]);
        }

        [Fact]
        public async Task NotificationTypeQuestion_PostInvalidModel_ReturnsToCorrectView()
        {
            var controller = CreateNewNotificationController();
            controller.ModelState.AddModelError("Error", "Test Error");
            var model = new NotificationTypeViewModel();
            var result = await controller.NotificationType(model, null) as ViewResult;

            Assert.Empty(result.ViewName);
            Assert.IsType<NotificationTypeViewModel>(result.Model);
        }
    }
}