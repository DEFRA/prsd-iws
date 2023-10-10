namespace EA.Iws.Web.Tests.Unit.Controllers.AdminExportAssessment
{
    using EA.Iws.Core.Exporters;
    using EA.Iws.Core.Facilities;
    using EA.Iws.Core.Importer;
    using EA.Iws.Core.Producers;
    using EA.Iws.Core.Shared;
    using EA.Iws.Requests.Exporters;
    using EA.Iws.Requests.Facilities;
    using EA.Iws.Requests.Importer;
    using EA.Iws.Requests.Producers;
    using EA.Iws.Web.Areas.AdminExportAssessment.Controllers;
    using EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.EditContact;
    using EA.Iws.Web.Infrastructure;
    using EA.Prsd.Core.Mediator;
    using FakeItEasy;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Xunit;

    public class EditContactControllerTests
    {
        private EditContactController editContactController;
        private readonly IMediator mediator;
        private readonly IAuditService auditService;
        private readonly Guid notificationId = new Guid("4AB23CDF-9B24-4598-A302-A69EBB5F2152");

        public EditContactControllerTests()
        {
            mediator = A.Fake<IMediator>();
            auditService = A.Fake<IAuditService>();
            editContactController = new EditContactController(mediator, auditService);
        }

        [Fact]
        public async Task Export_ReturnsView()
        {
            A.CallTo(() => mediator.SendAsync(A<GetExporterByNotificationId>._)).Returns((ExporterData)CreateValidEditContact("Exporter"));
            editContactController = new EditContactController(mediator, auditService);

            var model = ConvertToEditContactViewModel(CreateValidEditContact("Exporter"));
            var result = await editContactController.Exporter(notificationId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Exporter_ValidModel_RedirectsTo_NotificationOverview_Screen()
        {
            A.CallTo(() => mediator.SendAsync(A<GetExporterByNotificationId>._)).Returns((ExporterData)CreateValidEditContact("Exporter"));
            editContactController = new EditContactController(mediator, auditService);

            var model = CreateValidEditContact("Exporter");
            var editContactModel = ConvertToEditContactViewModel(model);
            var result = await editContactController.Exporter(notificationId, editContactModel) as RedirectToRouteResult;

            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("Overview", result.RouteValues["controller"]);
        }

        [Fact]
        public async Task Importer_ReturnsView()
        {
            A.CallTo(() => mediator.SendAsync(A<GetImporterByNotificationId>._)).Returns((ImporterData)CreateValidEditContact("Importer"));
            editContactController = new EditContactController(mediator, auditService);

            var model = ConvertToEditContactViewModel(CreateValidEditContact("Importer"));
            var result = await editContactController.Importer(notificationId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Importer_ValidModel_RedirectsTo_NotificationOverview_Screen()
        {
            A.CallTo(() => mediator.SendAsync(A<GetImporterByNotificationId>._)).Returns((ImporterData)CreateValidEditContact("Importer"));
            editContactController = new EditContactController(mediator, auditService);

            var model = CreateValidEditContact("Importer");
            var editContactModel = ConvertToEditContactViewModel(model);
            var result = await editContactController.Importer(notificationId, editContactModel) as RedirectToRouteResult;

            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("Overview", result.RouteValues["controller"]);
        }

        [Fact]
        public async Task Producer_ReturnsView()
        {
            A.CallTo(() => mediator.SendAsync(A<GetProducersByNotificationId>._)).Returns((List<ProducerData>)CreateValidEditContact("Producer"));
            editContactController = new EditContactController(mediator, auditService);

            var producerData = (List<ProducerData>)CreateValidEditContact("Producer");
            var model = ConvertToEditContactViewModel(producerData.FirstOrDefault());
            var result = await editContactController.Producer(notificationId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Producer_ValidModel_RedirectsTo_NotificationOverview_Screen()
        {
            A.CallTo(() => mediator.SendAsync(A<GetProducersByNotificationId>._)).Returns((List<ProducerData>)CreateValidEditContact("Producer"));
            editContactController = new EditContactController(mediator, auditService);

            var producerData = (List<ProducerData>)CreateValidEditContact("Producer");
            var model = ConvertToEditContactViewModel(producerData.FirstOrDefault());
            var result = await editContactController.Producer(notificationId, model) as RedirectToRouteResult;

            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("Overview", result.RouteValues["controller"]);
        }

        [Fact]
        public async Task Facility_ReturnsView()
        {
            A.CallTo(() => mediator.SendAsync(A<GetFacilitiesByNotificationId>._)).Returns((List<FacilityData>)CreateValidEditContact("Facility"));
            editContactController = new EditContactController(mediator, auditService);

            var facilityData = (List<FacilityData>)CreateValidEditContact("Facility");
            var model = ConvertToEditContactViewModel(facilityData.FirstOrDefault());
            var result = await editContactController.Facility(notificationId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Facility_ValidModel_RedirectsTo_NotificationOverview_Screen()
        {
            A.CallTo(() => mediator.SendAsync(A<GetFacilitiesByNotificationId>._)).Returns((List<FacilityData>)CreateValidEditContact("Facility"));
            editContactController = new EditContactController(mediator, auditService);

            var facilityData = (List<FacilityData>)CreateValidEditContact("Facility");
            var model = ConvertToEditContactViewModel(facilityData.FirstOrDefault());
            var result = await editContactController.Facility(notificationId, model) as RedirectToRouteResult;

            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("Overview", result.RouteValues["controller"]);
        }

        private dynamic CreateValidEditContact(string type)
        {
            var producerList = new List<ProducerData>();
            var facilityList = new List<FacilityData>();

            var addressData = new AddressData()
            {
                StreetOrSuburb = "address1",
                Address2 = "address2",
                TownOrCity = "town",
                CountryName = "United Kingdom",
                PostalCode = "12345"
            };

            var contactData = new ContactData()
            {
                FullName = "first last",
                Email = "email@address.com",
                TelephonePrefix = "44",
                Telephone = "3234234",
                FaxPrefix = "44",
                Fax = "234234233"
            };

            var businessData = new BusinessInfoData()
            {
                Name = "Test Name",
                BusinessType = BusinessType.SoleTrader,
                RegistrationNumber = "4345435",
                AdditionalRegistrationNumber = string.Empty
            };

            if (type != null && type == "Exporter")
            {
                return new ExporterData
                {
                    Address = addressData,
                    Contact = contactData,
                    Business = businessData,
                    NotificationId = notificationId
                };
            }
            else if (type != null && type == "Producer")
            {
                var producer = new ProducerData
                {
                    Address = addressData,
                    Contact = contactData,
                    Business = businessData
                };
                producerList.Add(producer);

                return producerList;
            }
            else if (type != null && type == "Importer")
            {
                return new ImporterData
                {
                    Address = addressData,
                    Contact = contactData,
                    Business = businessData
                };
            }
            else
            {
                var facilityData = new FacilityData
                {
                    Address = addressData,
                    Contact = contactData,
                    Business = businessData
                };

                facilityList.Add(facilityData);

                return facilityList;
            }
        }

        private EditContactViewModel ConvertToEditContactViewModel(dynamic data)
        {
            return new EditContactViewModel()
            {
                FullName = data.Contact.FullName,
                Email = data.Contact.Email,
                TelephonePrefix = data.Contact.TelephonePrefix,
                Telephone = data.Contact.Telephone
            };
        }
    }
}
