namespace EA.Iws.Web.Tests.Unit.Controllers.ImportNotification
{
    using EA.Iws.Api.Client.CompaniesHouseAPI;
    using EA.Iws.Api.Client.Models;
    using EA.Iws.Web.Areas.Common;
    using EA.Iws.Web.Areas.ImportNotification.Controllers;
    using EA.Iws.Web.Services;
    using EA.Prsd.Core.Mediator;
    using FakeItEasy;
    using FluentAssertions;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Xunit;

    public class FacilityControllerTests
    {
        private readonly IMediator mediator;
        private readonly FacilityController facilityController;
        private readonly ITrimTextService trimTextService;
        private readonly ICompaniesHouseClient companiesHouseClient;
        private readonly ConfigurationService configurationService;

        public FacilityControllerTests()
        {
            mediator = A.Fake<IMediator>();
            this.trimTextService = A.Fake<ITrimTextService>();
            companiesHouseClient = A.Fake<ICompaniesHouseClient>();
            configurationService = A.Fake<ConfigurationService>();

            facilityController = new FacilityController(mediator, trimTextService, () => companiesHouseClient, configurationService);
        }

        [Fact]
        public async Task GetCompany_Should_NotBeNull()
        {
            // Act
            var companyRegistrationNumber = "12345678";

            var result = await facilityController.GetCompanyName(companyRegistrationNumber) as JsonResult;

            result.Should().BeOfType<JsonResult>();
            result.JsonRequestBehavior.Should().Be(JsonRequestBehavior.AllowGet);

            A.CallTo(() => companiesHouseClient.GetCompanyDetailsAsync(configurationService.CurrentConfiguration.CompaniesHouseReferencePath, companyRegistrationNumber))
                                               .MustHaveHappenedOnceExactly();

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetCompany_HandlesCompanyNameCorrectly()
        {
            // Act
            string companyNumber = "12345678";
            string companyName = "Test Facility Company";
            var apiModel = new DefraCompaniesHouseApiModel
            {
                Organisation = new Organisation
                {
                    Name = companyName,
                    RegistrationNumber = companyNumber
                }
            };

            var jsonResult = "{ success = True, companyName = Test Facility Company }";

            A.CallTo(() => companiesHouseClient.GetCompanyDetailsAsync(A<string>._, A<string>._)).Returns(apiModel);

            // Assert
            var result = await facilityController.GetCompanyName(companyNumber) as JsonResult;
            result.Data.ToString().Should().Be(jsonResult);
        }
    }
}
