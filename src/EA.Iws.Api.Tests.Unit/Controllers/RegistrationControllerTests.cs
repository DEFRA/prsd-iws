namespace EA.Iws.Api.Tests.Unit.Controllers
{
    using System.Web.Http;
    using Api.Controllers;
    using FluentAssertions;
    using TestHelpers.Helpers;
    using Xunit;

    public class RegistrationControllerTests
    {
        [Fact]
        public void Controller_ShouldHaveAuthorizeAttribute()
        {
            typeof(RegistrationController).Should().BeDecoratedWith<AuthorizeAttribute>();
        }

        [Fact]
        public void Controller_ShouldNotHaveAnonymousActions()
        {
            AttributeHelper.ShouldNotHaveAnonymousMethods(typeof(RegistrationController));
        }
    }
}
