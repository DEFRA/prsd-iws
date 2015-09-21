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

            controller = new AccountController(A.Fake<IOAuthClient>, A.Fake<IAuthenticationManager>(), () => client,
                A.Fake<IUserInfoClient>);

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

            var result = await controller.ForgotPassword(new ResetPasswordViewModel()) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task ForgotPassword_ValidModel_CallsApi()
        {
            await controller.ForgotPassword(new ResetPasswordViewModel { Email = "test@email.com" });

            A.CallTo(() => client.Registration.ResetPasswordAsync(A<PasswordResetRequest>._))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task ForgotPassword_ValidModel_ReturnsEmailSentView()
        {
            A.CallTo(() => client.Registration.ResetPasswordAsync(A<PasswordResetRequest>._)).Returns(true);

            var result =
                await controller.ForgotPassword(new ResetPasswordViewModel { Email = "test@email.com" }) as
                    RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "ResetPasswordEmailSent", "Account");
        }
    }
}