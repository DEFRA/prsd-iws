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

            Assert.Equal("WasteOperation", result.RouteValues["action"]);
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
    }
}