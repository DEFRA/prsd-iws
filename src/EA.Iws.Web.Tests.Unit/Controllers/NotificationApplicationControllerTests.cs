namespace EA.Iws.Web.Tests.Unit.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Api.Client.Entities;
    using FakeItEasy;
    using ViewModels.NotificationApplication;
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
            return new NotificationApplicationController(() => client);
        }

        [Fact]
        public void CompetentAuthority_Get_ReturnsCorrectView()
        {
            var controller = CreateNotificationApplicationController();

            var result = controller.CompetentAuthority() as ViewResult;

            Assert.Equal("CompetentAuthority", result.ViewName);
            Assert.IsType<CompetentAuthorityChoiceViewModel>(result.Model);
        }

        [Fact]
        public void CompetentAuthority_Post_RedirectsToCorrectAction()
        {
            var controller = CreateNotificationApplicationController();

            var competentAuthorityChoice = CompetentAuthorityChoice;

            var result = controller.CompetentAuthority(competentAuthorityChoice) as RedirectToRouteResult;

            Assert.Equal("WasteActionQuestion", result.RouteValues["action"]);
        }

        [Fact]
        public void CompetentAuthority_PostInvalidModel_ReturnsToCorrectView()
        {
            var controller = CreateNotificationApplicationController();

            controller.ModelState.AddModelError("Error", "A Test Error");

            var competentAuthorityChoice = CompetentAuthorityChoice;

            var result = controller.CompetentAuthority(competentAuthorityChoice) as ViewResult;

            Assert.Equal("CompetentAuthority", result.ViewName);
            Assert.IsType<CompetentAuthorityChoiceViewModel>(result.Model);
        }

        [Fact]
        public void WasteActionQuestion_Get_ReturnsCorrectView()
        {
            var controller = CreateNotificationApplicationController();
            var result = controller.WasteActionQuestion("Environment Agency", "Recovery") as ViewResult;

            Assert.Empty(result.ViewName);
            Assert.IsType<InitialQuestionsViewModel>(result.Model);
        }

        [Fact]
        public async Task WasteActionQuestion_Post_RedirectsToCorrectAction()
        {
            var controller = CreateNotificationApplicationController();
            var model = new InitialQuestionsViewModel();
            model.SelectedWasteAction = WasteAction.Recovery;
            var result = await controller.WasteActionQuestion(model) as RedirectToRouteResult;

            Assert.Equal("Created", result.RouteValues["action"]);
        }

        [Fact]
        public async Task WasteActionQuestion_PostInvalidModel_ReturnsToCorrectView()
        {
            var controller = CreateNotificationApplicationController();
            controller.ModelState.AddModelError("Error", "Test Error");
            var model = new InitialQuestionsViewModel();
            var result = await controller.WasteActionQuestion(model) as ViewResult;

            Assert.Empty(result.ViewName);
            Assert.IsType<InitialQuestionsViewModel>(result.Model);
        }
    }
}