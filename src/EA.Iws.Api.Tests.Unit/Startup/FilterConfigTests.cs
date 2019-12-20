namespace EA.Iws.Api.Tests.Unit.Startup
{
    using Api.Filters;
    using Elmah.Contrib.WebApi;
    using FakeItEasy;
    using Services;
    using System.Linq;
    using Xunit;

    public class FilterConfigTests
    {
        private readonly FilterConfig config;
        private readonly IAppConfiguration configuration;

        public FilterConfigTests()
        {
            configuration = A.Fake<IAppConfiguration>();

            config = new FilterConfig(configuration);
        }

        [Fact]
        public void FilterConfig_GivenConfig_ShouldContainElmahHandleErrorApiAttribute()
        {
            Assert.Equal(config.Collection.Count(x => x.GetType() == (typeof(ElmahHandleErrorApiAttribute))), 1);
        }

        [Fact]
        public void FilterConfig_GivenMaintenanceMode_ShouldContainMaintenanceModeFilter()
        {
            A.CallTo(() => configuration.MaintenanceMode).Returns(true);

            var lconfig = new FilterConfig(configuration);

            Assert.Equal(lconfig.Collection.Count(x => x.GetType() == (typeof(MaintenanceModeFilter))), 1);
        }

        [Fact]
        public void FilterConfig_GivenNotMaintenanceMode_ShouldNotContainMaintenanceModeFilter()
        {
            A.CallTo(() => configuration.MaintenanceMode).Returns(false);

            Assert.Equal(config.Collection.Count(x => x.GetType() == (typeof(MaintenanceModeFilter))), 0);
        }
    }
}
