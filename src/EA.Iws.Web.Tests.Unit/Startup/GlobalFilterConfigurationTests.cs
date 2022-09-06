namespace EA.Iws.Web.Tests.Unit.Startup
{
    using FakeItEasy;
    using Services;
    using System.Linq;
    using System.Web.Mvc;
    using Web.Infrastructure;
    using Web.Infrastructure.VirusScanning;
    using Xunit;

    public class GlobalFilterConfigurationTests
    {
        [Fact]
        public void GlobalFilters_ShouldContainCorrectFilters()
        {
            GlobalFilters.Filters.Clear();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters, A.Fake<IAppConfiguration>());

            Assert.Equal(1, GlobalFilters.Filters.Count(x => x.Instance.GetType() == (typeof(RequireHttpsAttribute))));
            Assert.Equal(1, GlobalFilters.Filters.Count(x => x.Instance.GetType() == (typeof(EmailVerificationRequiredAttribute))));
            Assert.Equal(1, GlobalFilters.Filters.Count(x => x.Instance.GetType() == (typeof(OrganisationRequiredAttribute))));
            Assert.Equal(1, GlobalFilters.Filters.Count(x => x.Instance.GetType() == (typeof(AdminApprovalRequiredAttribute))));
            Assert.Equal(1, GlobalFilters.Filters.Count(x => x.Instance.GetType() == (typeof(HandleApiErrorFilter))));
            Assert.Equal(1, GlobalFilters.Filters.Count(x => x.Instance.GetType() == (typeof(AntiForgeryErrorFilter))));
            Assert.Equal(1, GlobalFilters.Filters.Count(x => x.Instance.GetType() == (typeof(VirusFoundFilter))));
        }

        [Fact]
        public void GlobalFilters_GivenMaintenanceMode_FiltersShouldContainMaintenanceModeFilter()
        {
            GlobalFilters.Filters.Clear();

            var configuration = A.Fake<IAppConfiguration>();
            A.CallTo(() => configuration.MaintenanceMode).Returns(true);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters, configuration);

            Assert.Equal(1, GlobalFilters.Filters.Count(x => x.Instance.GetType() == (typeof(MaintenanceModeFilterAttribute))));
        }

        [Fact]
        public void GlobalFilters_GivenNotMaintenanceMode_FiltersShouldNotContainMaintenanceModeFilter()
        {
            GlobalFilters.Filters.Clear();

            var configuration = A.Fake<IAppConfiguration>();
            A.CallTo(() => configuration.MaintenanceMode).Returns(false);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters, configuration);

            Assert.Equal(0, GlobalFilters.Filters.Count(x => x.Instance.GetType() == (typeof(MaintenanceModeFilterAttribute))));
        }
    }
}
