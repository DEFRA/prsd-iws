namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.Carrier;
    using Core.Carriers;
    using Core.Notification.Audit;
    using Core.Shared;
    using FakeItEasy;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.Carriers;
    using Requests.Shared;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Web.Infrastructure;
    using Web.ViewModels.Shared;
    using Xunit;

    public class CarrierControllerTests
    {
        private readonly CarrierController carrierController;
        private readonly Guid carrierId = new Guid("2196585B-F0F0-4A01-BC2F-EB8191B30FC6");
        private readonly IMediator mediator;
        private readonly IAuditService auditService;
        private readonly Guid notificationId = new Guid("4AB23CDF-9B24-4598-A302-A69EBB5F2152");

        public CarrierControllerTests()
        {
            mediator = A.Fake<IMediator>();
            this.auditService = A.Fake<IAuditService>();
            A.CallTo(() => mediator.SendAsync(A<GetCountries>._)).Returns(new List<CountryData>
            {
                new CountryData
                {
                    Id = new Guid("4345FB05-F7DF-4E16-939C-C09FCA5C7D7B"),
                    Name = "United Kingdom"
                },
                new CountryData
                {
                    Id = new Guid("29B0D09E-BA77-49FB-AF95-4171408C07C9"),
                    Name = "Germany"
                }
            });

            A.CallTo(
                () =>
                    mediator.SendAsync(A<GetCarrierForNotification>.That.Matches(p => p.CarrierId == carrierId)))
                .Returns(CreateCarrier(carrierId));

            carrierController = new CarrierController(mediator, new AddAddressBookEntryMap(), this.auditService);
            A.CallTo(() => auditService.AddAuditEntry(this.mediator, notificationId, "user", NotificationAuditType.Create, NotificationAuditScreenType.IntendedCarrier));
        }

        private AddCarrierViewModel CreateValidAddCarrier()
        {
            return new AddCarrierViewModel
            {
                Address = new AddressData
                {
                    Address2 = "address2",
                    CountryId = new Guid("4345FB05-F7DF-4E16-939C-C09FCA5C7D7B"),
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
                NotificationId = notificationId
            };
        }

        private EditCarrierViewModel CreateValidEditCarrier()
        {
            return new EditCarrierViewModel
            {
                Address = new AddressData
                {
                    Address2 = "address2",
                    CountryId = new Guid("4345FB05-F7DF-4E16-939C-C09FCA5C7D7B"),
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
                NotificationId = notificationId,
                CarrierId = carrierId
            };
        }

        private CarrierData CreateCarrier(Guid carrierId)
        {
            return new CarrierData
            {
                Address = new AddressData(),
                Business = new BusinessInfoData(),
                Contact = new ContactData(),
                Id = carrierId,
                NotificationId = notificationId
            };
        }

        [Fact]
        public async Task Add_ReturnsView()
        {
            var result = await carrierController.Add(notificationId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Add_InvalidModel_ReturnsView()
        {
            var model = new AddCarrierViewModel();

            carrierController.ModelState.AddModelError("Test", "Error");

            var result = await carrierController.Add(model) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Add_ValidModel_AddsCarrierToNotification()
        {
            var model = CreateValidAddCarrier();

            await carrierController.Add(model, null);

            A.CallTo(() => mediator.SendAsync(A<AddCarrierToNotification>.That.Matches(p => p.NotificationId == notificationId)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Add_ValidModel_RedirectsToList()
        {
            var model = CreateValidAddCarrier();

            var result = await carrierController.Add(model) as RedirectToRouteResult;

            Assert.Equal("List", result.RouteValues["action"]);
        }

        [Fact]
        public async Task Add_ValidModel_WithBackToOverviewTrue_MaintainsRouteValue()
        {
            var model = CreateValidAddCarrier();

            var result = await carrierController.Add(model, true) as RedirectToRouteResult;

            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.True(Convert.ToBoolean(result.RouteValues[backToOverviewKey]));
        }

        [Fact]
        public async Task Add_ValidModel_WithBackToOverviewFalse_MaintainsRouteValue()
        {
            var model = CreateValidAddCarrier();

            var result = await carrierController.Add(model, false) as RedirectToRouteResult;

            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.False(Convert.ToBoolean(result.RouteValues[backToOverviewKey]));
        }

        [Fact]
        public async Task Add_ValidModel_WithBackToOverviewNull_DefaultsRouteValueToFalse()
        {
            var model = CreateValidAddCarrier();

            var result = await carrierController.Add(model, null) as RedirectToRouteResult;

            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.False(Convert.ToBoolean(result.RouteValues[backToOverviewKey]));
        }

        [Fact]
        public async Task Edit_ReturnsView()
        {
            var result = await carrierController.Edit(notificationId, carrierId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Edit_InvalidModel_ReturnsView()
        {
            var model = new EditCarrierViewModel();

            carrierController.ModelState.AddModelError("Test", "Error");

            var result = await carrierController.Edit(model) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Edit_ValidModel_EditsCarrierToNotification()
        {
            var model = CreateValidEditCarrier();

            await carrierController.Edit(model);

            A.CallTo(() => mediator.SendAsync(A<UpdateCarrierForNotification>.That.Matches(p => p.NotificationId == notificationId)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Edit_ValidModel_RedirectsToList()
        {
            var model = CreateValidEditCarrier();

            var result = await carrierController.Edit(model) as RedirectToRouteResult;

            Assert.Equal("List", result.RouteValues["action"]);
        }

        [Fact]
        public async Task Edit_ValidModel_WithBackToOverviewTrue_MaintainsRouteValue()
        {
            var model = CreateValidEditCarrier();

            var result = await carrierController.Edit(model, true) as RedirectToRouteResult;

            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.True(Convert.ToBoolean(result.RouteValues[backToOverviewKey]));
        }

        [Fact]
        public async Task Edit_ValidModel_WithBackToOverviewFalse_MaintainsRouteValue()
        {
            var model = CreateValidEditCarrier();

            var result = await carrierController.Edit(model, false) as RedirectToRouteResult;

            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.False(Convert.ToBoolean(result.RouteValues[backToOverviewKey]));
        }

        [Fact]
        public async Task Edit_ValidModel_WithBackToOverviewNull_DefaultsRouteValueToFalse()
        {
            var model = CreateValidEditCarrier();

            var result = await carrierController.Edit(model, null) as RedirectToRouteResult;

            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.False(Convert.ToBoolean(result.RouteValues[backToOverviewKey]));
        }

        [Fact]
        public async Task List_ReturnsView()
        {
            var result = await carrierController.List(notificationId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task List_GetsFacilitiesByNotificationId()
        {
            await carrierController.List(notificationId);

            A.CallTo(
                () =>
                    mediator.SendAsync(A<GetCarriersByNotificationId>.That.Matches(p => p.NotificationId == notificationId)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Remove_ReturnsView()
        {
            var result = await carrierController.Remove(notificationId, carrierId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Remove_CallsGetCarrier()
        {
            await carrierController.Remove(notificationId, carrierId);

            A.CallTo(
                () =>
                    mediator.SendAsync(A<GetCarrierForNotification>.That.Matches(
                            p => p.CarrierId == carrierId && p.NotificationId == notificationId)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Remove_Post_RemovesCarrierFromNotification()
        {
            var model = new RemoveCarrierViewModel
            {
                CarrierId = carrierId,
                NotificationId = notificationId
            };

            await carrierController.Remove(model);

            A.CallTo(() => mediator.SendAsync(A<DeleteCarrierForNotification>.That.Matches(p => p.CarrierId == carrierId && p.NotificationId == notificationId)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Remove_WithBackToOverviewTrue_MaintainsRouteValue()
        {
            var model = new RemoveCarrierViewModel
            {
                NotificationId = notificationId,
                CarrierId = carrierId
            };

            var result = await carrierController.Remove(model, true) as RedirectToRouteResult;

            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.True(Convert.ToBoolean(result.RouteValues[backToOverviewKey]));
        }

        [Fact]
        public async Task Remove_WithBackToOverviewFalse_MaintainsRouteValue()
        {
            var model = new RemoveCarrierViewModel
            {
                NotificationId = notificationId,
                CarrierId = carrierId
            };

            var result = await carrierController.Remove(model, false) as RedirectToRouteResult;

            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.False(Convert.ToBoolean(result.RouteValues[backToOverviewKey]));
        }

        [Fact]
        public async Task Remove_WithBackToOverviewNull_DefaultsRouteValueToFalse()
        {
            var model = new RemoveCarrierViewModel
            {
                NotificationId = notificationId,
                CarrierId = carrierId
            };

            var result = await carrierController.Remove(model, null) as RedirectToRouteResult;

            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.False(Convert.ToBoolean(result.RouteValues[backToOverviewKey]));
        }
    }
}