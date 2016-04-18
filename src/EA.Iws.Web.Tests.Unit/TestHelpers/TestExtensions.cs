namespace EA.Iws.Web.Tests.Unit.TestHelpers
{
    using System.Web.Mvc;
    using Xunit;

    public static class TestExtensions
    {
        public static void AssertControllerReturn(this RedirectToRouteResult result, string actionName,
            string controllerName)
        {
            Assert.Equal(actionName, result.RouteValues["action"]);
            Assert.Equal(controllerName, result.RouteValues["controller"]);
        }
    }
}
