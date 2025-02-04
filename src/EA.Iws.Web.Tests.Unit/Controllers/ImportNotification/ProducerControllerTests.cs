namespace EA.Iws.Web.Tests.Unit.Controllers.ImportNotification
{
    using EA.Iws.Api.Client.CompaniesHouseAPI;
    using EA.Iws.Api.Client.Models;
    using EA.Iws.Web.Areas.AdminExportAssessment.Controllers;
    using EA.Iws.Web.Infrastructure;
    using EA.Iws.Web.Infrastructure.AdditionalCharge;
    using EA.Iws.Web.Services;
    using EA.Prsd.Core.Mediator;
    using FakeItEasy;
    using FluentAssertions;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Xunit;

    public class ProducerControllerTests
    {
        private readonly IMediator mediator;
        private readonly IAuditService auditService;
        private readonly ProducerController producerController;
        private readonly IAdditionalChargeService additionalChargeService;
        private readonly ICompaniesHouseClient companiesHouseClient;
        private readonly ConfigurationService configurationService;

        public ProducerControllerTests()
        {
            mediator = A.Fake<IMediator>();
            this.additionalChargeService = A.Fake<IAdditionalChargeService>();
            companiesHouseClient = A.Fake<ICompaniesHouseClient>();
            configurationService = A.Fake<ConfigurationService>();

            producerController = new ProducerController(mediator, auditService, additionalChargeService, () => companiesHouseClient, configurationService);
        }

        [Fact]
        public async Task GetCompany_Should_NotBeNull()
        {
            // Act
            var companyRegistrationNumber = "99775533";

            var result = await producerController.GetCompanyName(companyRegistrationNumber) as JsonResult;

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
            string companyNumber = "22446688";
            string companyName = "Test Producer Company";
            var apiModel = new DefraCompaniesHouseApiModel
            {
                Organisation = new Organisation
                {
                    Name = companyName,
                    RegistrationNumber = companyNumber
                }
            };

            var jsonResult = "{ success = True, companyName = Test Producer Company }";

            A.CallTo(() => companiesHouseClient.GetCompanyDetailsAsync(A<string>._, A<string>._)).Returns(apiModel);

            // Assert
            var result = await producerController.GetCompanyName(companyNumber) as JsonResult;
            result.Data.ToString().Should().Be(jsonResult);
        }
    }
}
