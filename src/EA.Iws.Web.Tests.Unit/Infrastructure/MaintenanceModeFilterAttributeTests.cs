﻿namespace EA.Iws.Web.Tests.Unit.Infrastructure
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using FakeItEasy;
    using Web.Infrastructure;
    using Xunit;

    public class MaintenanceModeFilterAttributeTests
    {
        private readonly MaintenanceModeFilterAttribute filter;

        public MaintenanceModeFilterAttributeTests()
        {
            filter = new MaintenanceModeFilterAttribute();
        }

        [Fact]
        public void OnActionExecuted_GivenNotMaintenanceAction_ShouldRedirectToMaintenanceAction()
        {
            var routeData = new RouteData();
            routeData.Values.Add("action", string.Empty);
            routeData.Values.Add("controller", string.Empty);

            var context = Context(routeData);

            filter.OnActionExecuted(context);

            var result = context.Result as RedirectToRouteResult;
            Assert.Equal("Maintenance", result.RouteValues["action"]);
            Assert.Equal("Errors", result.RouteValues["controller"]);
        }

        [Fact]
        public void OnActionExecuted_GivenMaintenanceAction_RouteDataShouldBeNull()
        {
            var routeData = new RouteData();
            routeData.Values.Add("action", "Maintenance");
            routeData.Values.Add("controller", "Errors");

            var context = Context(routeData);

            filter.OnActionExecuted(context);
            Assert.Equal((typeof(EmptyResult)), context.Result.GetType());
        }

        public ActionExecutedContext Context(RouteData route)
        {
            return new ActionExecutedContext(new ControllerContext(A.Fake<HttpContextBase>(), route, A.Fake<ControllerBase>()), A.Fake<ActionDescriptor>(), false, null);
        }
    }
}
