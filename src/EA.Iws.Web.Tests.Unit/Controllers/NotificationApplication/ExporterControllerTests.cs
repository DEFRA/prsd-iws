namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.Exporter;
    using Core.Notification.Audit;
    using Core.Shared;
    using EA.Iws.Api.Client.CompaniesHouseAPI;
    using EA.Iws.Api.Client.Models;
    using EA.Iws.Web.Areas.Common;
    using EA.Iws.Web.Services;
    using FakeItEasy;
    using FluentAssertions;
    using Mappings;
    using Prsd.Core.Mediator;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Web.Infrastructure;
    using Web.ViewModels.Shared;
    using Xunit;

    public class ExporterControllerTests
    {
        private readonly IMediator client;
        private readonly Guid notificationId = new Guid("81CBBCEE-34C0-4628-B054-E0D8135A7947");
        private readonly ExporterController exporterController;
        private readonly IAuditService auditService;
        private readonly ITrimTextService trimTextService;
        private readonly ICompaniesHouseClient companiesHouseClient;
        private readonly ConfigurationService configurationService;

        public ExporterControllerTests()
        {
            client = A.Fake<IMediator>();
            auditService = A.Fake<IAuditService>();
            trimTextService = A.Fake<ITrimTextService>();
            companiesHouseClient = A.Fake<ICompaniesHouseClient>();
            configurationService = A.Fake<ConfigurationService>();
            exporterController = new ExporterController(client, new AddAddressBookEntryMap(), auditService, trimTextService, () => companiesHouseClient, configurationService);

            A.CallTo(() => auditService.AddAuditEntry(client, notificationId, "user", NotificationAuditType.Added, NotificationAuditScreenType.Exporter));
        }

        private ExporterViewModel CreateExporterViewModel()
        {
            return new ExporterViewModel
            {
                NotificationId = notificationId,
                Address = new AddressData
                {
                    Address2 = "address2",
                    CountryId = new Guid("31D3BCAA-7315-4FD6-A6C3-A1B9D6697DF2"),
                    CountryName = "United Kingdom",
                    PostalCode = "postcode",
                    Region = "region",
                    StreetOrSuburb = "street",
                    TownOrCity = "town"
                },
                Business = new BusinessTypeViewModel
                {
                    RegistrationNumber = "12345",
                    BusinessType = BusinessType.SoleTrader,
                    Name = "business name"
                },
                Contact = new ContactData
                {
                    Email = "email@address.com",
                    FullName = "first last",
                    Telephone = "123"
                },
            };
        }

        [Fact]
        public async Task Exporter_Post_BackToOverviewTrue_ReturnsOverview()
        {
            var model = CreateExporterViewModel();

            var result = await exporterController.Index(model, true) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "Index", "Home");
        }

        [Fact]
        public async Task Exporter_Post_BackToOverviewFalse_ReturnsProducerList()
        {
            var model = CreateExporterViewModel();

            var result = await exporterController.Index(model, false) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "List", "Producer");
        }

        [Fact]
        public async Task Exporter_Post_BackToOverviewNull_ReturnsProducerList()
        {
            var model = CreateExporterViewModel();

            var result = await exporterController.Index(model, null) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "List", "Producer");
        }

        [Fact]
        public async Task GetCompany_Should_NotBeNull()
        {
            // Act
            var companyRegistrationNumber = "22446688";

            var result = await exporterController.GetCompanyName(companyRegistrationNumber) as JsonResult;

            result.Should().BeOfType<JsonResult>();
            result.JsonRequestBehavior.Should().Be(JsonRequestBehavior.AllowGet);

            A.CallTo(() => companiesHouseClient.GetCompanyDetailsAsync(configurationService.CurrentConfiguration.CompaniesHouseReferencePath, companyRegistrationNumber))
                .MustHaveHappenedOnceExactly();

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void GetCompanyDetailsAsync_Should_Thow_EndPoint_ArgumentNull_Exeception()
        {
            A.CallTo(() => companiesHouseClient.GetCompanyDetailsAsync(string.Empty, "12345"))
                                               .Throws<ArgumentNullException>();
        }

        [Fact]
        public void GetCompanyDetailsAsync_Should_Return_InvalidReference_True()
        {
            var result = new DefraCompaniesHouseApiModel
            {
                InvalidReference = true
            };

            A.CallTo(() => companiesHouseClient.GetCompanyDetailsAsync("https://test.com", string.Empty)).Returns(result);

            result.Should().NotBeNull();
            result.InvalidReference.Should().BeTrue();
        }

        [Fact]
        public void GetCompanyDetailsAsync_Should_Thow_UnauthorizedAccessException()
        {
            A.CallTo(() => companiesHouseClient.GetCompanyDetailsAsync("https://test.com", "12345"))
                                               .Throws<UnauthorizedAccessException>();
        }
    }
}
