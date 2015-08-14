namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using EA.Iws.Api.Client;
    using EA.Iws.Web.Areas.NotificationApplication.Controllers;
    using EA.Iws.Web.Areas.NotificationApplication.ViewModels.SpecialHandling;
    using FakeItEasy;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Xunit;

    public class SpecialHandlingControllerTests
    {
        private readonly IIwsClient client;
        private readonly SpecialHandlingController specialHandlingController;

        public SpecialHandlingControllerTests()
        {
            client = A.Fake<IIwsClient>();
            specialHandlingController = new SpecialHandlingController(() => client);
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
