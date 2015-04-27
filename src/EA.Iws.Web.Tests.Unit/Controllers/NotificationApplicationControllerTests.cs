namespace EA.Iws.Web.Tests.Unit.Controllers
{
    using System.Web.Mvc;
    using ViewModels.NotificationApplication;
    using ViewModels.Shared;
    using Web.Controllers;
    using Xunit;

    public class NotificationApplicationControllerTests
    {
        private static CompetentAuthorityChoice CompetentAuthorityChoice
        {
            get
            {
                var competentAuthorityChoice = new CompetentAuthorityChoice
                {
                    CompetentAuthorities = new RadioButtonStringCollection
                    {
                        SelectedValue = "Test",
                        PossibleValues = new[] {"Test", "String", "Value"}
                    }
                };
                return competentAuthorityChoice;
            }
        }

        [Fact]
        public void CompetentAuthority_Get_ReturnsCorrectView()
        {
            var controller = new NotificationApplicationController();

            var result = controller.CompetentAuthority() as ViewResult;

            Assert.Equal("CompetentAuthority", result.ViewName);
            Assert.IsType<CompetentAuthorityChoice>(result.Model);
        }

        [Fact]
        public void CompetentAuthority_Post_RedirectsToCorrectAction()
        {
            var controller = new NotificationApplicationController();

            var competentAuthorityChoice = CompetentAuthorityChoice;

            var result = controller.CompetentAuthority(competentAuthorityChoice) as RedirectToRouteResult;

            Assert.Equal("WasteActionQuestion", result.RouteValues["action"]);
        }

        [Fact]
        public void CompetentAuthority_PostInvalidModel_ReturnsToCorrectView()
        {
            var controller = new NotificationApplicationController();

            controller.ModelState.AddModelError("Error", "A Test Error");

            var competentAuthorityChoice = CompetentAuthorityChoice;

            var result = controller.CompetentAuthority(competentAuthorityChoice) as ViewResult;

            Assert.Equal("CompetentAuthority", result.ViewName);
            Assert.IsType<CompetentAuthorityChoice>(result.Model);
        }
    
        [Fact]
        public void WasteActionQuestion_Get_ReturnsCorrectView()
        {
            var controller = new NotificationApplicationController();
            var result = controller.WasteActionQuestion(null, null) as ViewResult;

            Assert.Empty(result.ViewName);
            Assert.IsType<InitialQuestions>(result.Model);
        }

        [Fact]
        public void WasteActionQuestion_Post_RedirectsToCorrectAction()
        {
            var controller = new NotificationApplicationController();
            var model = new InitialQuestions();
            model.SelectedWasteAction = WasteAction.Recovery.ToString();
            var result = controller.WasteActionQuestion(model) as RedirectToRouteResult;

            Assert.Equal("GenerateNumber", result.RouteValues["action"]);
        }

        [Fact]
        public void WasteActionQuestion_PostInvalidModel_ReturnsToCorrectView()
        {
            var controller = new NotificationApplicationController();
            controller.ModelState.AddModelError("Error", "Test Error");
            var model = new InitialQuestions();
            var result = controller.WasteActionQuestion(model) as ViewResult;

            Assert.Empty(result.ViewName);
            Assert.IsType<InitialQuestions>(result.Model);
        }
    }
}
