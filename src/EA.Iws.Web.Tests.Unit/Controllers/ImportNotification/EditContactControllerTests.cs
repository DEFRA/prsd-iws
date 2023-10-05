namespace EA.Iws.Web.Tests.Unit.Controllers.ImportNotification
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.ImportNotification.Controllers;
    using EA.Iws.Core.ImportNotification.Summary;
    using EA.Iws.Core.Shared;
    using EA.Iws.Requests.ImportNotification.Exporters;
    using EA.Iws.Requests.Shared;
    using EA.Iws.Web.Areas.ImportNotification.ViewModels.EditContact;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Xunit;

    public class EditContactControllerTests
    {
        private EditContactController editContactController;
        private readonly IMediator mediator;
        private readonly Guid importNotificationId = new Guid("A6386BA3-4070-4D83-99D1-54E9614A87EB");

        public EditContactControllerTests()
        {
            mediator = A.Fake<IMediator>();
            editContactController = new EditContactController(mediator);
        }

        [Fact]
        public async Task Export_ReturnsView()
        {
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

            A.CallTo(() => mediator.SendAsync(A<GetExporterByImportNotificationId>._)).Returns(CreateValidExporterEditContact());
            editContactController = new EditContactController(mediator);

            var model = ConvertToEditContactViewModel(CreateValidExporterEditContact());
            var result = await editContactController.Exporter(importNotificationId) as ViewResult;
            Assert.Equal(string.Empty, result.ViewName);
        }

        private Exporter CreateValidExporterEditContact()
        {
            return new Exporter
            {
                Address = new Address
                {
                    AddressLine2 = "address2",
                    AddressLine1 = "address1",
                    Country = "United Kingdom",
                    PostalCode = "postcode",
                    TownOrCity = "town"
                },
                Contact = new Contact
                {
                    Email = "email@address.com",
                    Name = "first last",
                    Telephone = "123",
                    TelephonePrefix = "44"
                },
                BusinessType = BusinessType.SoleTrader,
                Name = "Test Name",
                RegistrationNumber = "4345435"
            };
        }

        private EditContactViewModel ConvertToEditContactViewModel(Exporter data)
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
