namespace EA.Iws.Web.Tests.Unit.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.NotificationApplication;
    using FakeItEasy;
    using Prsd.Core.Web.ApiClient;
    using Requests.Notification;
    using Requests.Shared;
    using ViewModels.Shared;
    using Web.Controllers;
    using Xunit;

    public class NotificationApplicationControllerTests
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

        private static NotificationApplicationController CreateNotificationApplicationController()
        {
            var client = A.Fake<IIwsClient>();
            A.CallTo(() => client.SendAsync<Guid>(A<string>._, A<CreateNotificationApplication>._))
                .Returns(Guid.Empty);
            return new NotificationApplicationController(() => client);
        }

        [Fact]
        public void CompetentAuthority_Get_ReturnsCorrectView()
        {
            var controller = CreateNotificationApplicationController();

            var result = controller.CompetentAuthority() as ViewResult;

            Assert.Empty(result.ViewName);
            Assert.IsType<CompetentAuthorityChoiceViewModel>(result.Model);
        }

        [Fact]
        public void CompetentAuthority_Post_RedirectsToCorrectAction()
        {
            var controller = CreateNotificationApplicationController();

            var competentAuthorityChoice = CompetentAuthorityChoice;

            var result = controller.CompetentAuthority(competentAuthorityChoice) as RedirectToRouteResult;

            Assert.Equal("NotificationTypeQuestion", result.RouteValues["action"]);
        }

        [Fact]
        public void CompetentAuthority_PostInvalidModel_ReturnsToCorrectView()
        {
            var controller = CreateNotificationApplicationController();

            controller.ModelState.AddModelError("Error", "A Test Error");

            var competentAuthorityChoice = CompetentAuthorityChoice;

            var result = controller.CompetentAuthority(competentAuthorityChoice) as ViewResult;

            Assert.Empty(result.ViewName);
            Assert.IsType<CompetentAuthorityChoiceViewModel>(result.Model);
        }

        [Fact]
        public void NotificationTypeQuestion_Get_ReturnsCorrectView()
        {
            var controller = CreateNotificationApplicationController();
            var result = controller.NotificationTypeQuestion("Environment Agency", "Recovery") as ViewResult;

            Assert.Empty(result.ViewName);
            Assert.IsType<NotificationTypeViewModel>(result.Model);
        }

        [Fact]
        public async Task NotificationTypeQuestion_Post_RedirectsToCorrectAction()
        {
            var controller = CreateNotificationApplicationController();
            var model = new NotificationTypeViewModel();
            model.SelectedNotificationType = NotificationType.Recovery;
            var result = await controller.NotificationTypeQuestion(model) as RedirectToRouteResult;

            Assert.Equal("Created", result.RouteValues["action"]);
        }

        [Fact]
        public async Task NotificationTypeQuestion_PostInvalidModel_ReturnsToCorrectView()
        {
            var controller = CreateNotificationApplicationController();
            controller.ModelState.AddModelError("Error", "Test Error");
            var model = new NotificationTypeViewModel();
            var result = await controller.NotificationTypeQuestion(model) as ViewResult;

            Assert.Empty(result.ViewName);
            Assert.IsType<NotificationTypeViewModel>(result.Model);
        }
    }
}