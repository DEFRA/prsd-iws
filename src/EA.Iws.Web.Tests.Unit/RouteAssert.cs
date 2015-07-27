namespace EA.Iws.Web.Tests.Unit
{
    using System.Web.Routing;
    using Xunit;

    internal static class RouteAssert
    {
        public static void RoutesTo(RouteValueDictionary routeValues, string action, string controller, string area = null)
        {
            var actualAction = routeValues["action"];
            var actualController = routeValues["controller"];
            var actualArea = routeValues["area"];
            bool isValid = (string)actualAction == action
                           && (string)actualController == controller
                           && (area == null
                               || (string)actualArea == area);

            Assert.True(isValid,
                string.Format(
                    "Route is not valid!\nExpected\naction: {0}\ncontroller: {1}\narea: {2}\n\nFound\naction: {3}\ncontroller: {4}\narea: {5}\n",
                    action, controller, area ?? "null", actualAction, actualController, actualArea ?? "null"));
        }
    }
}