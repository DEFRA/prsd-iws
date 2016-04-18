namespace EA.Iws.Web.Tests.Unit.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using Xunit;

    public class ControllerTests
    {
        private static IEnumerable<MethodInfo> AllControllerActions(IEnumerable<Type> allControllerTypes)
        {
            return allControllerTypes.SelectMany(type => type.GetMethods());
        }

        private static IEnumerable<Type> AllControllerTypes()
        {
            return
                typeof(Global).Assembly.GetTypes()
                    .Where(type => typeof(Controller).IsAssignableFrom(type));
        }

        // Stolen from http://codiply.com/blog/test-for-omission-of-validateantiforgerytoken-attribute-in-asp-net-mvc
        [Fact]
        public void AllHttpPostControllerActionsShouldBeDecoratedWithValidateAntiForgeryTokenAttribute()
        {
            var allControllerTypes = AllControllerTypes();
            var allControllerActions = AllControllerActions(allControllerTypes);

            var failingActions = allControllerActions
                .Where(method =>
                    Attribute.GetCustomAttribute(method, typeof(HttpPostAttribute)) != null)
                .Where(method =>
                    Attribute.GetCustomAttribute(method, typeof(ValidateAntiForgeryTokenAttribute)) == null)
                .ToList();

            var message = string.Empty;
            if (failingActions.Any())
            {
                message =
                    failingActions.Count() + " failing action" +
                    (failingActions.Count() == 1 ? ":\n" : "s:\n") +
                    failingActions.Select(method => method.Name + " in " + method.DeclaringType.Name)
                        .Aggregate((a, b) => a + ",\n" + b);
            }

            Assert.False(failingActions.Any(), message);
        }

        [Fact]
        public void AllControllersShouldHaveAuthorizeAttributes()
        {
            var allControllerTypes = AllControllerTypes();

            var failingControllers = allControllerTypes
                .Where(controller =>
                    !Attribute.GetCustomAttributes(controller, typeof(AuthorizeAttribute)).Any() &&
                    !Attribute.GetCustomAttributes(controller, typeof(AllowAnonymousAttribute)).Any())
                .Where(controller => controller.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).All(method => 
                    Attribute.GetCustomAttribute(method, typeof(AuthorizeAttribute)) == null &&
                    Attribute.GetCustomAttribute(method, typeof(AllowAnonymousAttribute)) == null))
                .ToList();

            var message = string.Empty;

            if (failingControllers.Any())
            {
                message =
                    failingControllers.Count() + " failing controller" +
                    (failingControllers.Count() == 1 ? ":\n" : "s:\n") +
                    failingControllers.Select(controller => controller.FullName)
                        .Aggregate((a, b) => a + ",\n" + b);
            }

            Assert.False(failingControllers.Any(), message);
        }
    }
}