namespace EA.Iws.Web.Tests.Unit.Controllers.AddressBook
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.AddressBook.Controllers;
    using Areas.AddressBook.ViewModels;
    using Core.AddressBook;
    using Core.Shared;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.AddressBook;
    using Requests.Shared;
    using Xunit;

    public class EditControllerTests
    {
        private readonly Guid userId = new Guid("899A33A0-3C83-48F9-9A15-77114F34C28D");
        private readonly Guid addressBookRecordId = new Guid("21088CB8-69B8-446E-B27D-2F47663FC135");
        private readonly EditController editController;
        private IMediator mediator;
        private readonly AddressRecordType addressRecordType = AddressRecordType.Producer;

        public EditControllerTests()
        {
            mediator = A.Fake<IMediator>();
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

            A.CallTo(() => mediator.SendAsync(A<GetAddressBookRecordById>._)).Returns(CreateValidAddEntry());
            editController = new EditController(mediator);
        }

        [Fact]
        public async Task Edit_ReturnsView()
        {
            var result = await editController.Index(userId, addressRecordType) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Edit_ValidModel()
        {
            var result = await editController.Index(CreateValidEditAddress());

            A.CallTo(() => mediator.SendAsync(A<EditAddressBookEntry>._)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Edit_ValidModel_RedirectsToList()
        {
            var model = CreateValidEditAddress();

            var result = await editController.Index(model) as RedirectToRouteResult;

            Assert.Equal("Index", result.RouteValues["action"]);
        }

        [Fact]
        public async Task Add_ValidModel()
        {
            var result = await editController.Add(CreateValidAddAddress());

            A.CallTo(() => mediator.SendAsync(A<AddNewAddressBookEntry>._)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Add_ValidModel_RedirectsToList()
        {
            var model = CreateValidAddAddress();

            var result = await editController.Add(model) as RedirectToRouteResult;

            Assert.Equal("Index", result.RouteValues["action"]);
        }
        private AddAddressViewModel CreateValidAddAddress()
        {
            return new AddAddressViewModel
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
                Business = new AddressBusinessTypeViewModel
                {
                    BusinessType = BusinessType.SoleTrader,
                    Name = "business name"
                },
                Contact = new ContactData
                {
                    Email = "email@address.com",
                    FullName = "first last",
                    Telephone = "123"
                },
                Type = Core.AddressBook.AddressRecordType.Producer,
                UserId = userId
            };
        }

        private AddressBookRecordData CreateValidAddEntry()
        {
            return new AddressBookRecordData
            {
                AddressData = new AddressData
                {
                    Address2 = "address2",
                    CountryId = new Guid("4345FB05-F7DF-4E16-939C-C09FCA5C7D7B"),
                    CountryName = "United Kingdom",
                    PostalCode = "postcode",
                    Region = "region",
                    StreetOrSuburb = "street",
                    TownOrCity = "town"
                },
                BusinessData = new BusinessInfoData
                {
                    BusinessType = BusinessType.SoleTrader,
                    Name = "business name"
                },
                ContactData = new ContactData
                {
                    Email = "email@address.com",
                    FullName = "first last",
                    Telephone = "123"
                }
            };
        }

        private EditAddressViewModel CreateValidEditAddress()
        {
            return new EditAddressViewModel
            {
                AddressBookRecordId = addressBookRecordId,
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
                Business = new AddressBusinessTypeViewModel
                {
                    BusinessType = BusinessType.SoleTrader,
                    Name = "business name"
                },
                Contact = new ContactData
                {
                    Email = "email@address.com",
                    FullName = "first last",
                    Telephone = "123"
                },
              Type = Core.AddressBook.AddressRecordType.Producer
            };
        }       
    }
}
