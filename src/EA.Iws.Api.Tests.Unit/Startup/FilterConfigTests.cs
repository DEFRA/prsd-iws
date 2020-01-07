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
            Assert.Equal(1, config.Collection.Count(x => x.GetType() == (typeof(ElmahHandleErrorApiAttribute))));
        }

        [Fact]
        public void FilterConfig_GivenMaintenanceMode_ShouldContainMaintenanceModeFilter()
        {
            A.CallTo(() => configuration.MaintenanceMode).Returns(true);

            var lconfig = new FilterConfig(configuration);

            Assert.Equal(1, lconfig.Collection.Count(x => x.GetType() == (typeof(MaintenanceModeFilter))));
        }

        [Fact]
        public void FilterConfig_GivenNotMaintenanceMode_ShouldNotContainMaintenanceModeFilter()
        {
            A.CallTo(() => configuration.MaintenanceMode).Returns(false);

            Assert.Equal(0, config.Collection.Count(x => x.GetType() == (typeof(MaintenanceModeFilter))));
        }
    }
}
