namespace EA.Iws.Web.Tests.Unit.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Api.Client;
    using Api.Client.Entities;
    using FakeItEasy;
    using Microsoft.Owin.Security;
    using Prsd.Core.Web.OAuth;
    using Prsd.Core.Web.OpenId;
    using Web.Controllers;
    using Web.ViewModels.Account;
    using Xunit;

    public class AccountControllerTests
    {
        private readonly IIwsClient client;
        private readonly AccountController controller;

        public AccountControllerTests()
        {
            client = A.Fake<IIwsClient>();

            controller = new AccountController(A.Fake<IOAuthClient>(), A.Fake<IAuthenticationManager>(), client,
                A.Fake<IUserInfoClient>());

            var request = A.Fake<HttpRequestBase>();
            var context = A.Fake<HttpContextBase>();

            A.CallTo(() => request.Url).Returns(new Uri("https://test.com"));
            A.CallTo(() => context.Request).Returns(request);

            controller.ControllerContext = new ControllerContext(context, new RouteData(), controller);

            controller.Url = new UrlHelper(new RequestContext(context, new RouteData()));
        }

        [Fact]
        public void ForgotPassword_ReturnsView()
        {
            var result = controller.ForgotPassword() as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task ForgotPassword_InvalidModel_ReturnsView()
        {
            controller.ModelState.AddModelError("Email", "Invalid email");

            var result = await controller.ForgotPassword(new ForgotPasswordViewModel()) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task ForgotPassword_ValidModel_CallsApi()
        {
            await controller.ForgotPassword(new ForgotPasswordViewModel { Email = "test@email.com" });

            A.CallTo(() => client.Registration.ResetPasswordRequestAsync(A<PasswordResetRequest>._))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task ForgotPassword_ValidModel_ReturnsEmailSentView()
        {
            A.CallTo(() => client.Registration.ResetPasswordRequestAsync(A<PasswordResetRequest>._)).Returns(true);

            var result =
                await controller.ForgotPassword(new ForgotPasswordViewModel { Email = "test@email.com" }) as
                    RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "ResetPasswordEmailSent", "Account");
        }

        [Fact]
        public void ResetPassword_ReturnsView()
        {
            var result = controller.ResetPassword(new Guid("9E6E137D-4903-414B-BE9D-1A89893C678B"), "code") as ViewResult;

            Assert.Equal(String.Empty, result.ViewName);
        }

        [Fact]
        public async Task ResetPassword_InvalidModel_ReturnsView()
        {
            controller.ModelState.AddModelError("Password", "Error");
            var result = await controller.ResetPassword(new Guid("9E6E137D-4903-414B-BE9D-1A89893C678B"), "code", new ResetPasswordViewModel()) as ViewResult;

            Assert.Equal(String.Empty, result.ViewName);
        }

        [Fact]
        public async Task ResetPassword_ValidModel_CallsApi()
        {
            await
                controller.ResetPassword(new Guid("9E6E137D-4903-414B-BE9D-1A89893C678B"), "code",
                    new ResetPasswordViewModel());

            A.CallTo(() => client.Registration.ResetPasswordAsync(A<PasswordResetData>._))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task ResetPassword_ValidModel_RedirectsToPasswordUpdated()
        {
            var result = await controller.ResetPassword(new Guid("9E6E137D-4903-414B-BE9D-1A89893C678B"), "code",
                new ResetPasswordViewModel()) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "PasswordUpdated", "Account");
        }
    }
}