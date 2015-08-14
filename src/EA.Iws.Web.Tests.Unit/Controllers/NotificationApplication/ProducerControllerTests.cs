namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.Producer;
    using Core.Producers;
    using Core.Shared;
    using FakeItEasy;
    using Requests.Producers;
    using Requests.Shared;
    using Xunit;

    public class ProducerControllerTests
    {
        private readonly IIwsClient client;
        private readonly Guid notificationId = new Guid("4AB23CDF-9B24-4598-A302-A69EBB5F2152");
        private readonly Guid producerId = new Guid("2196585B-F0F0-4A01-BC2F-EB8191B30FC6");
        private readonly ProducerController producerController;
        private readonly Guid producerId2 = new Guid("D8991991-64A7-4101-A3A2-2F6B538A0A7A");

        public ProducerControllerTests()
        {
            client = A.Fake<IIwsClient>();
            A.CallTo(() => client.SendAsync(A<GetCountries>._)).Returns(new List<CountryData>
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

            A.CallTo(() => client.SendAsync(A<string>._, A<GetProducerForNotification>._)).Returns(CreateProducer(producerId));
            producerController = new ProducerController(() => client);
        }

        private ProducerData CreateProducer(Guid producerId)
        {
            return new ProducerData
            {
                Address = new AddressData(),
                Business = new BusinessInfoData(),
                Contact = new ContactData(),
                Id = producerId,
                IsSiteOfExport = producerId == this.producerId,
                NotificationId = notificationId
            };
        }

        [Fact]
        public async Task Add_ReturnsView()
        {
            var result = await producerController.Add(notificationId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Add_InvalidModel_ReturnsView()
        {
            var model = new AddProducerViewModel();

            producerController.ModelState.AddModelError("Test", "Error");

            var result = await producerController.Add(model) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Add_ValidModel_CallsClient()
        {
            var model = CreateValidAddProducer();

            await producerController.Add(model);

            A.CallTo(() => client.SendAsync(A<string>._, A<AddProducerToNotification>._)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Add_ValidModel_RedirectsToList()
        {
            var model = CreateValidAddProducer();

            var result = await producerController.Add(model) as RedirectToRouteResult;

            Assert.Equal("List", result.RouteValues["action"]);
        }

        [Fact]
        public async Task Add_ValidModel_WithBackToOverviewTrue_MaintainsRouteValue()
        {
            var model = CreateValidAddProducer();

            var result = await producerController.Add(model, true) as RedirectToRouteResult;

            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.True(Convert.ToBoolean(result.RouteValues[backToOverviewKey]));
        }

        [Fact]
        public async Task Add_ValidModel_WithBackToOverviewFalse_MaintainsRouteValue()
        {
            var model = CreateValidAddProducer();

            var result = await producerController.Add(model, false) as RedirectToRouteResult;

            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.False(Convert.ToBoolean(result.RouteValues[backToOverviewKey]));
        }

        [Fact]
        public async Task Add_ValidModel_WithBackToOverviewNull_DefaultsRouteValueToFalse()
        {
            var model = CreateValidAddProducer();

            var result = await producerController.Add(model, null) as RedirectToRouteResult;

            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.False(Convert.ToBoolean(result.RouteValues[backToOverviewKey]));
        }

        [Fact]
        public async Task Edit_ReturnsView()
        {
            var result = await producerController.Edit(notificationId, producerId, null) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Edit_CallsClient()
        {
            await producerController.Edit(notificationId, producerId);

            A.CallTo(
                () =>
                    client.SendAsync(A<string>._,
                        A<GetProducerForNotification>.That.Matches(
                            p => p.NotificationId == notificationId && p.ProducerId == producerId)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Edit_InvalidModel_ReturnsView()
        {
            var model = new EditProducerViewModel();

            producerController.ModelState.AddModelError("Test", "Error");

            var result = await producerController.Edit(model) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Edit_ValidModel_CallsClient()
        {
            var model = CreateValidEditProducer();

            await producerController.Edit(model);

            A.CallTo(() => client.SendAsync(A<string>._, A<UpdateProducerForNotification>._)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Edit_ValidModel_RedirectsToList()
        {
            var model = CreateValidEditProducer();

            var result = await producerController.Edit(model) as RedirectToRouteResult;

            Assert.Equal("List", result.RouteValues["action"]);
        }

        [Fact]
        public async Task Edit_ValidModel_WithBackToOverviewTrue_MaintainsRouteValue()
        {
            var model = CreateValidEditProducer();

            var result = await producerController.Edit(model, true) as RedirectToRouteResult;

            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.True(Convert.ToBoolean(result.RouteValues[backToOverviewKey]));
        }

        [Fact]
        public async Task Edit_ValidModel_WithBackToOverviewFalse_MaintainsRouteValue()
        {
            var model = CreateValidEditProducer();

            var result = await producerController.Edit(model, false) as RedirectToRouteResult;

            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.False(Convert.ToBoolean(result.RouteValues[backToOverviewKey]));
        }

        [Fact]
        public async Task Edit_ValidModel_WithBackToOverviewNull_DefaultsRouteValueToFalse()
        {
            var model = CreateValidEditProducer();

            var result = await producerController.Edit(model, null) as RedirectToRouteResult;

            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.False(Convert.ToBoolean(result.RouteValues[backToOverviewKey]));
        }

        [Fact]
        public async Task Remove_MultipleProducersRemoveSiteOfExport_ReturnsViewWithError()
        {
            A.CallTo(() => client.SendAsync(A<string>._, A<GetProducersByNotificationId>._)).Returns(
                new List<ProducerData>
                {
                    CreateProducer(producerId),
                    CreateProducer(producerId2)
                });

            var result = await producerController.Remove(notificationId, producerId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
            Assert.NotNull(result.ViewBag.Error);
        }

        [Fact]
        public async Task Remove_MultipleProducersRemoveNotSiteOfExport_ReturnsViewWithNoError()
        {
            A.CallTo(() => client.SendAsync(A<string>._, A<GetProducersByNotificationId>._)).Returns(
                new List<ProducerData>
                {
                    CreateProducer(producerId),
                    CreateProducer(producerId2)
                });

            var result = await producerController.Remove(notificationId, producerId2) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
            Assert.Null(result.ViewBag.Error);
        }

        [Fact]
        public async Task Remove_SingleProducer_ReturnsViewWithNoError()
        {
            A.CallTo(() => client.SendAsync(A<string>._, A<GetProducersByNotificationId>._)).Returns(
                new List<ProducerData>
                {
                    CreateProducer(producerId)
                });

            var result = await producerController.Remove(notificationId, producerId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
            Assert.Null(result.ViewBag.Error);
        }

        [Fact]
        public async Task Remove_CallsClient()
        {
            var model = new RemoveProducerViewModel
            {
                NotificationId = notificationId,
                ProducerId = producerId
            };

            await producerController.Remove(model);

            A.CallTo(
                () =>
                    client.SendAsync(A<string>._,
                        A<DeleteProducerForNotification>.That.Matches(
                            p => p.NotificationId == notificationId && p.ProducerId == producerId)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Remove_ReturnsList()
        {
            var model = new RemoveProducerViewModel
            {
                NotificationId = notificationId,
                ProducerId = producerId
            };

            var result = await producerController.Remove(model) as RedirectToRouteResult;

            Assert.Equal("List", result.RouteValues["action"]);
        }

        [Fact]
        public async Task Remove_WithBackToOverviewTrue_MaintainsRouteValue()
        {
            var model = new RemoveProducerViewModel
            {
                NotificationId = notificationId,
                ProducerId = producerId
            };

            var result = await producerController.Remove(model, true) as RedirectToRouteResult;

            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.True(Convert.ToBoolean(result.RouteValues[backToOverviewKey]));
        }

        [Fact]
        public async Task Remove_WithBackToOverviewFalse_MaintainsRouteValue()
        {
            var model = new RemoveProducerViewModel
            {
                NotificationId = notificationId,
                ProducerId = producerId
            };

            var result = await producerController.Remove(model, false) as RedirectToRouteResult;

            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.False(Convert.ToBoolean(result.RouteValues[backToOverviewKey]));
        }

        [Fact]
        public async Task Remove_WithBackToOverviewNull_DefaultsRouteValueToFalse()
        {
            var model = new RemoveProducerViewModel
            {
                NotificationId = notificationId,
                ProducerId = producerId
            };

            var result = await producerController.Remove(model, null) as RedirectToRouteResult;

            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.False(Convert.ToBoolean(result.RouteValues[backToOverviewKey]));
        }

        [Fact]
        public async Task List_ReturnsView()
        {
            var result = await producerController.List(notificationId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task List_CallsClient()
        {
            await producerController.List(notificationId);

            A.CallTo(
                () =>
                    client.SendAsync(A<string>._,
                        A<GetProducersByNotificationId>.That.Matches(p => p.NotificationId == notificationId)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task SiteOfExport_ReturnsView()
        {
            var result = await producerController.SiteOfExport(notificationId, null) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task SiteOfExport_CallsClient()
        {
            await producerController.SiteOfExport(notificationId, null);

            A.CallTo(
                () =>
                    client.SendAsync(A<string>._,
                        A<GetProducersByNotificationId>.That.Matches(p => p.NotificationId == notificationId)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task SiteOfExport_Post_CallsClient()
        {
            var model = new SiteOfExportViewModel
            {
                NotificationId = notificationId,
                SelectedSiteOfExport = producerId
            };

            await producerController.SiteOfExport(model, null);

            A.CallTo(
                () =>
                    client.SendAsync(A<string>._,
                        A<SetSiteOfExport>.That.Matches(p => p.NotificationId == notificationId && p.ProducerId == producerId)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task SiteOfExport_BackToListTrue_ReturnsList()
        {
            var model = new SiteOfExportViewModel
            {
                NotificationId = notificationId,
                SelectedSiteOfExport = producerId
            };

            var result = await producerController.SiteOfExport(model, true) as RedirectToRouteResult;

            Assert.Equal("List", result.RouteValues["action"]);
        }

        [Fact]
        public async Task SiteOfExport_BackToListFalse_ReturnsImporterIndex()
        {
            var model = new SiteOfExportViewModel
            {
                NotificationId = notificationId,
                SelectedSiteOfExport = producerId
            };

            var result = await producerController.SiteOfExport(model, false) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "Index", "Importer");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(null)]
        public async Task SiteOfExport_BackToListTrue_IgnoresBackToOverview(bool? backToOverview)
        {
            var model = new SiteOfExportViewModel
            {
                NotificationId = notificationId,
                SelectedSiteOfExport = producerId
            };

            var result = await producerController.SiteOfExport(model, true, backToOverview) as RedirectToRouteResult;

            Assert.Equal("List", result.RouteValues["action"]);
        }

        [Fact]
        public async Task SiteOfExport_BackToListFalse_BackToOverviewTrue_ReturnsOverview()
        {
            var model = new SiteOfExportViewModel
            {
                NotificationId = notificationId,
                SelectedSiteOfExport = producerId
            };

            var result = await producerController.SiteOfExport(model, false, true) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "Index", "Home");
        }

        [Fact]
        public async Task SiteOfExport_BackToListFalse_BackToOverviewFalse_ReturnsImporterIndex()
        {
            var model = new SiteOfExportViewModel
            {
                NotificationId = notificationId,
                SelectedSiteOfExport = producerId
            };

            var result = await producerController.SiteOfExport(model, false, false) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "Index", "Importer");
        }

        [Fact]
        public async Task SiteOfExport_BackToListFalse_BackToOverviewNull_ReturnsImporterIndex()
        {
            var model = new SiteOfExportViewModel
            {
                NotificationId = notificationId,
                SelectedSiteOfExport = producerId
            };

            var result = await producerController.SiteOfExport(model, false, null) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "Index", "Importer");
        }

        private AddProducerViewModel CreateValidAddProducer()
        {
            return new AddProducerViewModel
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
                Business = new ProducerBusinessTypeViewModel
                {
                    BusinessType = BusinessType.SoleTrader,
                    Name = "business name"
                },
                Contact = new ContactData
                {
                    Email = "email@address.com",
                    FirstName = "first",
                    LastName = "last",
                    Telephone = "123"
                },
                NotificationId = notificationId
            };
        }

        private EditProducerViewModel CreateValidEditProducer()
        {
            return new EditProducerViewModel
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
                Business = new ProducerBusinessTypeViewModel
                {
                    BusinessType = BusinessType.SoleTrader,
                    Name = "business name"
                },
                Contact = new ContactData
                {
                    Email = "email@address.com",
                    FirstName = "first",
                    LastName = "last",
                    Telephone = "123"
                },
                NotificationId = notificationId,
                ProducerId = producerId
            };
        }
    }
}