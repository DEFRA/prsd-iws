namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.SpecialHandling;
    using FakeItEasy;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Xunit;

    public class SpecialHandlingControllerTests
    {
        private readonly SpecialHandlingController specialHandlingController;

        public SpecialHandlingControllerTests()
        {
            specialHandlingController = new SpecialHandlingController(A.Fake<IMediator>());
        }

        [Fact]
        public async Task Post_BackToOverviewTrue_RedirectsToOverview()
        {
            var model = new SpecialHandlingViewModel();
            var result = await specialHandlingController.Index(model, true) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "Home");
        }

        [Fact]
        public async Task Post_BackToOverviewFalse_RedirectsToStateOfExport()
        {
            var model = new SpecialHandlingViewModel();
            var result = await specialHandlingController.Index(model, false) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "StateOfExport");
        }
    }
}
