namespace EA.Iws.Api.Tests.Unit.Controllers
{
    using System;
    using System.Web.Http;
    using Api.Controllers;
    using EA.Iws.TestHelpers.Helpers;
    using FluentAssertions;
    using Xunit;

    public class ErrorLogControllerTests
    {
        [Fact]
        public void Controller_ShouldHaveAuthorizeAttribute()
        {
            typeof(ErrorLogController).Should().BeDecoratedWith<AuthorizeAttribute>(a => a.Roles == "administrator");
        }

        [Fact]
        public void Controller_ShouldNotHaveAnonymousActions()
        {
            AttributeHelper.ShouldNotHaveAnonymousMethods(typeof(ErrorLogController));
        }
    }
}
