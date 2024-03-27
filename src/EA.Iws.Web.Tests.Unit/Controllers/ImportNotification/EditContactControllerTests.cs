namespace EA.Iws.Web.Tests.Unit.Controllers.ImportNotification
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.ImportNotification.Controllers;
    using EA.Iws.Core.ImportNotification.Summary;
    using EA.Iws.Core.Shared;
    using EA.Iws.Requests.ImportNotification.Exporters;
    using EA.Iws.Requests.ImportNotification.Facilities;
    using EA.Iws.Requests.ImportNotification.Importers;
    using EA.Iws.Requests.ImportNotification.Producers;
    using EA.Iws.Web.Areas.ImportNotification.ViewModels.EditContact;
    using EA.Iws.Web.Infrastructure.AdditionalCharge;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Xunit;

    public class EditContactControllerTests
    {
        private EditContactController editContactController;
        private readonly IMediator mediator;
        private readonly IAdditionalChargeService additionalChargeService;
        private readonly Guid importNotificationId = new Guid("A6386BA3-4070-4D83-99D1-54E9614A87EB");

        public EditContactControllerTests()
        {
            mediator = A.Fake<IMediator>();
            additionalChargeService = A.Fake<IAdditionalChargeService>();
            editContactController = new EditContactController(mediator, additionalChargeService);
        }

        [Fact]
        public async Task Export_ReturnsView()
        {
            A.CallTo(() => mediator.SendAsync(A<GetExporterByImportNotificationId>._)).Returns((Exporter)CreateValidEditContact("Exporter"));
            editContactController = new EditContactController(mediator, additionalChargeService);

            var model = ConvertToEditContactViewModel(CreateValidEditContact("Exporter"));
            var result = await editContactController.Exporter(importNotificationId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Exporter_ValidModel_RedirectsTo_NotificationOverview_Screen()
        {
            A.CallTo(() => mediator.SendAsync(A<GetExporterByImportNotificationId>._)).Returns((Exporter)CreateValidEditContact("Exporter"));
            editContactController = new EditContactController(mediator, additionalChargeService);

            var model = CreateValidEditContact("Exporter");
            var editContactModel = ConvertToEditContactViewModel(model);
            var result = await editContactController.Exporter(importNotificationId, editContactModel) as RedirectToRouteResult;

            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("Home", result.RouteValues["controller"]);
        }

        [Fact]
        public async Task Producer_ReturnsView()
        {
            A.CallTo(() => mediator.SendAsync(A<GetProducerByImportNotificationId>._)).Returns((Producer)CreateValidEditContact("Producer"));
            editContactController = new EditContactController(mediator, additionalChargeService);

            var model = ConvertToEditContactViewModel(CreateValidEditContact("Producer"));
            var result = await editContactController.Producer(importNotificationId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Producer_ValidModel_RedirectsTo_NotificationOverview_Screen()
        {
            A.CallTo(() => mediator.SendAsync(A<GetProducerByImportNotificationId>._)).Returns((Producer)CreateValidEditContact("Producer"));
            editContactController = new EditContactController(mediator, additionalChargeService);

            var model = CreateValidEditContact("Producer");
            var editContactModel = ConvertToEditContactViewModel(model);
            var result = await editContactController.Producer(importNotificationId, editContactModel) as RedirectToRouteResult;

            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("Home", result.RouteValues["controller"]);
        }

        [Fact]
        public async Task Importer_ReturnsView()
        {
            A.CallTo(() => mediator.SendAsync(A<GetImporterByImportNotificationId>._)).Returns((Importer)CreateValidEditContact("Importer"));
            editContactController = new EditContactController(mediator, additionalChargeService);

            var model = ConvertToEditContactViewModel(CreateValidEditContact("Importer"));
            var result = await editContactController.Importer(importNotificationId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Importer_ValidModel_RedirectsTo_NotificationOverview_Screen()
        {
            A.CallTo(() => mediator.SendAsync(A<GetImporterByImportNotificationId>._)).Returns((Importer)CreateValidEditContact("Importer"));
            editContactController = new EditContactController(mediator, additionalChargeService);

            var model = CreateValidEditContact("Importer");
            var editContactModel = ConvertToEditContactViewModel(model);
            var result = await editContactController.Importer(importNotificationId, editContactModel) as RedirectToRouteResult;

            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("Home", result.RouteValues["controller"]);
        }

        [Fact]
        public async Task Facility_ReturnsView()
        {
            A.CallTo(() => mediator.SendAsync(A<GetFacilityByImportNotificationId>._)).Returns((Facility)CreateValidEditContact("Facility"));
            editContactController = new EditContactController(mediator, additionalChargeService);

            var model = ConvertToEditContactViewModel(CreateValidEditContact("Facility"));
            var result = await editContactController.Facility(importNotificationId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Facility_ValidModel_RedirectsTo_NotificationOverview_Screen()
        {
            A.CallTo(() => mediator.SendAsync(A<GetFacilityByImportNotificationId>._)).Returns((Facility)CreateValidEditContact("Facility"));
            editContactController = new EditContactController(mediator, additionalChargeService);

            var model = CreateValidEditContact("Facility");
            var editContactModel = ConvertToEditContactViewModel(model);
            var result = await editContactController.Facility(importNotificationId, editContactModel) as RedirectToRouteResult;

            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("Home", result.RouteValues["controller"]);
        }

        private dynamic CreateValidEditContact(string type)
        {
            var address = new Address
            {
                AddressLine1 = "address1",
                AddressLine2 = "address2",
                Country = "United Kingdom",
                PostalCode = "postcode",
                TownOrCity = "town"
            };

            var contact = new Contact
            {
                Email = "email@address.com",
                Name = "first last",
                Telephone = "7448826833",
                TelephonePrefix = "44"
            };

            if (type != null && type == "Exporter")
            {
                return new Exporter
                {
                    Address = address,
                    Contact = contact,
                    BusinessType = BusinessType.SoleTrader,
                    Name = "Test Name",
                    RegistrationNumber = "4345435"
                };
            }
            else if (type != null && type == "Producer")
            {
                return new Producer
                {
                    Address = address,
                    Contact = contact,
                    BusinessType = BusinessType.SoleTrader,
                    Name = "Test Name",
                    RegistrationNumber = "4345435"
                };
            }
            else if (type != null && type == "Importer")
            {
                return new Importer
                {
                    Address = address,
                    Contact = contact,
                    BusinessType = BusinessType.SoleTrader,
                    Name = "Test Name",
                    RegistrationNumber = "4345435"
                };
            }
            else
            {
                return new Facility
                {
                    Address = address,
                    Contact = contact,
                    BusinessType = BusinessType.SoleTrader,
                    Name = "Test Name",
                    RegistrationNumber = "4345435"
                };
            }
        }

        private EditContactViewModel ConvertToEditContactViewModel(dynamic data)
        {
            return new EditContactViewModel()
            {
                Name = data.Name,
                FullName = data.Contact.Name,
                Email = data.Contact.Email,
                TelephonePrefix = data.Contact.TelephonePrefix,
                Telephone = data.Contact.Telephone,
                PostalCode = data.Address.PostalCode
            };
        }
    }
}