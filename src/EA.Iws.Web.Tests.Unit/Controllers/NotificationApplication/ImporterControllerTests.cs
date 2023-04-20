﻿namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.Importer;
    using Core.AddressBook;
    using Core.Notification.Audit;
    using Core.Shared;
    using EA.Iws.Web.Areas.Common;
    using FakeItEasy;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.AddressBook;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Web.Infrastructure;
    using Web.ViewModels.Shared;
    using Xunit;

    public class ImporterControllerTests
    {
        private readonly Guid notificationId = new Guid("31D3BCAA-7315-4FD6-A6C3-A1B9D6697DF2");
        private readonly ImporterController importerController;
        private readonly IAuditService auditService;
        private readonly IMediator mediator;
        private readonly ITrimTextMethod trimTextMethod;

        public ImporterControllerTests()
        {
            this.mediator = A.Fake<IMediator>();
            this.auditService = A.Fake<IAuditService>();
            this.trimTextMethod = A.Fake<ITrimTextMethod>();
            importerController = new ImporterController(A.Fake<IMediator>(), A.Fake<IMapWithParameter<ImporterViewModel, AddressRecordType, AddAddressBookEntry>>(), this.auditService, trimTextMethod);
            A.CallTo(() => auditService.AddAuditEntry(this.mediator, notificationId, "user", NotificationAuditType.Added, NotificationAuditScreenType.Importer));
        }

        private ImporterViewModel CreateImporterViewModel()
        {
            return new ImporterViewModel
            {
                NotificationId = notificationId,
                Address = new AddressData
                {
                    Address2 = "address2",
                    CountryId = new Guid("C5D27573-C35E-4565-A5BB-A28D74D5ABCD"),
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
        public async Task Importer_Post_BackToOverviewTrue_ReturnsOverview()
        {
            var model = CreateImporterViewModel();

            var result = await importerController.Index(model, true) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "Index", "Home");
        }

        [Fact]
        public async Task Importer_Post_BackToOverviewFalse_ReturnsFacilityList()
        {
            var model = CreateImporterViewModel();

            var result = await importerController.Index(model, false) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "List", "Facility");
        }

        [Fact]
        public async Task Importer_Post_BackToOverviewNull_ReturnsFacilityList()
        {
            var model = CreateImporterViewModel();

            var result = await importerController.Index(model, null) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "List", "Facility");
        }
    }
}
