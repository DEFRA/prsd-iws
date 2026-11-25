namespace EA.Iws.Web.Tests.Unit.Controllers.Admin
{
    using System.Collections.Generic;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Core.Admin;
    using Core.Authorization.Permissions;
    using Core.Notification;
    using EA.Iws.Core.Authorization;
    using EA.Iws.Web.Areas.Admin.Controllers;
    using EA.Iws.Web.Areas.Admin.ViewModels.Menu;
    using EA.Iws.Web.Infrastructure;
    using EA.Iws.Web.Infrastructure.Authorization;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.Admin;
    using Requests.Admin.UserAdministration;
    using Requests.Authorization;
    using Xunit;

    /// <summary>
    /// Regression tests for MenuController.HomeNavigation.
    /// These tests guard against that regression recurring by asserting that
    /// GetNewInternalUsers is only ever sent to the mediator when the current user
    /// is authorized to approve new internal users (i.e. is an Administrator).
    /// </summary>
    public class MenuControllerTests
    {
        private readonly IMediator mediator;
        private readonly AuthorizationService authorizationService;
        private readonly MenuController controller;

        public MenuControllerTests()
        {
            mediator = A.Fake<IMediator>();

            // AuthorizationService is a thin, concrete wrapper around the mediator -
            // faking the mediator calls it makes gives full control over which
            // activities are "authorized" without needing to fake the service itself.
            authorizationService = new AuthorizationService(mediator, new RequestAuthorizationAttributeCache());

            controller = new MenuController(mediator, authorizationService);

            // MenuController.CreateAdminLinksViewModel reads User.IsInRole("readonly"),
            // so a ControllerContext with a usable IPrincipal must be wired up,
            // otherwise controller.User is null and the controller throws a
            // NullReferenceException before any assertions can run.
            var httpContext = A.Fake<HttpContextBase>();
            var principal = A.Fake<IPrincipal>();
            A.CallTo(() => principal.IsInRole(A<string>._)).Returns(false);
            A.CallTo(() => httpContext.User).Returns(principal);

            controller.ControllerContext = new ControllerContext(httpContext, new RouteData(), controller);

            // Default: nothing is authorized, and GetUserCompetentAuthority/GetNewInternalUsers
            // are stubbed out with safe defaults unless a test overrides them.
            A.CallTo(() => mediator.SendAsync(A<AuthorizeActivity>._)).Returns(false);

            A.CallTo(() => mediator.SendAsync(A<GetUserCompetentAuthority>._))
                .Returns(UKCompetentAuthority.Wales);

            A.CallTo(() => mediator.SendAsync(A<GetNewInternalUsers>._))
                .Returns(new List<InternalUserData>());
        }

        private void AuthorizeApproveNewInternalUser(bool isAuthorized)
        {
            A.CallTo(() => mediator.SendAsync(
                A<AuthorizeActivity>.That.Matches(a => a.Activity == UserAdministrationPermissions.CanApproveNewInternalUser)))
                .Returns(isAuthorized);
        }

        [Fact]
        public void HomeNavigation_DoesNotCallGetNewInternalUsers_WhenUserCannotApproveNewInternalUsers()
        {
            // Arrange: simulates a non-administrator internal/readonly user, or a
            // user still pending approval - exactly the scenario that previously
            // caused the unhandled SecurityException and 500 error.
            AuthorizeApproveNewInternalUser(false);

            // Act
            var result = controller.HomeNavigation(AdminHomeNavigationSection.Notifications) as PartialViewResult;

            // Assert
            A.CallTo(() => mediator.SendAsync(A<GetNewInternalUsers>._)).MustNotHaveHappened();

            var model = Assert.IsType<AdminLinksViewModel>(result.Model);
            Assert.False(model.ShowApproveNewInternalUserLink);
            Assert.Equal(0, model.UsersAwaitingApproval);
        }

        [Fact]
        public void HomeNavigation_CallsGetNewInternalUsers_WhenUserCanApproveNewInternalUsers()
        {
            // Arrange: simulates an approved Administrator, who is allowed to see
            // the list of users awaiting approval.
            AuthorizeApproveNewInternalUser(true);

            A.CallTo(() => mediator.SendAsync(A<GetNewInternalUsers>._))
                .Returns(new List<InternalUserData>
                {
                    new InternalUserData(),
                    new InternalUserData()
                });

            // Act
            var result = controller.HomeNavigation(AdminHomeNavigationSection.Notifications) as PartialViewResult;

            // Assert
            A.CallTo(() => mediator.SendAsync(A<GetNewInternalUsers>._)).MustHaveHappenedOnceExactly();

            var model = Assert.IsType<AdminLinksViewModel>(result.Model);
            Assert.True(model.ShowApproveNewInternalUserLink);
            Assert.Equal(2, model.UsersAwaitingApproval);
        }

        [Fact]
        public void HomeNavigation_DoesNotThrow_WhenUserCannotApproveNewInternalUsers_AndGetNewInternalUsersWouldFail()
        {
            // Arrange: explicitly simulate the production failure mode - if
            // GetNewInternalUsers were called for an unapproved user it would throw.
            // This proves the controller no longer reaches that call in this scenario.
            AuthorizeApproveNewInternalUser(false);

            A.CallTo(() => mediator.SendAsync(A<GetNewInternalUsers>._))
                .Throws(new System.Security.SecurityException(
                    "A user who is not an administrator or not approved may not retrieve the pending user list."));

            // Act / Assert - should not throw, because GetNewInternalUsers should never be called.
            var result = controller.HomeNavigation(AdminHomeNavigationSection.Notifications) as PartialViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void HomeNavigation_SetsShowEAReportLinks_WhenCompetentAuthorityIsEngland()
        {
            A.CallTo(() => mediator.SendAsync(A<GetUserCompetentAuthority>._))
                .Returns(UKCompetentAuthority.England);

            var result = controller.HomeNavigation(AdminHomeNavigationSection.Notifications) as PartialViewResult;

            var model = Assert.IsType<AdminLinksViewModel>(result.Model);
            Assert.True(model.ShowEAReportLinks);
        }

        [Fact]
        public void HomeNavigation_DoesNotSetShowEAReportLinks_WhenCompetentAuthorityIsNotEngland()
        {
            A.CallTo(() => mediator.SendAsync(A<GetUserCompetentAuthority>._))
                .Returns(UKCompetentAuthority.Scotland);

            var result = controller.HomeNavigation(AdminHomeNavigationSection.Notifications) as PartialViewResult;

            var model = Assert.IsType<AdminLinksViewModel>(result.Model);
            Assert.False(model.ShowEAReportLinks);
        }

        [Fact]
        public void HomeNavigation_ReturnsCorrectPartialView()
        {
            var result = controller.HomeNavigation(AdminHomeNavigationSection.Notifications) as PartialViewResult;

            Assert.Equal("_HomeNavigation", result.ViewName);
        }
    }
}