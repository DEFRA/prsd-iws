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

    public class ImporterControllerTests
    {
        private readonly IMediator mediator;
        private readonly ImporterController importerController;
        private readonly ITrimTextService trimTextService;
        private readonly ICompaniesHouseClient companiesHouseClient;
        private readonly ConfigurationService configurationService;

        public ImporterControllerTests()
        {
            mediator = A.Fake<IMediator>();
            this.trimTextService = A.Fake<ITrimTextService>();
            companiesHouseClient = A.Fake<ICompaniesHouseClient>();
            configurationService = A.Fake<ConfigurationService>();

            importerController = new ImporterController(mediator, trimTextService, () => companiesHouseClient, configurationService);
        }

        [Fact]
        public async Task GetCompany_Should_NotBeNull()
        {
            // Act
            var companyRegistrationNumber = "12345678";

            var result = await importerController.GetCompanyName(companyRegistrationNumber) as JsonResult;

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
            string companyName = "Test Importer Company";
            var apiModel = new DefraCompaniesHouseApiModel
            {
                Organisation = new Organisation
                {
                    Name = companyName,
                    RegistrationNumber = companyNumber
                }
            };

            var jsonResult = "{ success = True, companyName = Test Importer Company }";

            A.CallTo(() => companiesHouseClient.GetCompanyDetailsAsync(A<string>._, A<string>._)).Returns(apiModel);

            // Assert
            var result = await importerController.GetCompanyName(companyNumber) as JsonResult;
            result.Data.ToString().Should().Be(jsonResult);
        }
    }
}
