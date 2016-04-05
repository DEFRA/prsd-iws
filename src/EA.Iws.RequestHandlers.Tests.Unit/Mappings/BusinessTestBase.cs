namespace EA.Iws.RequestHandlers.Tests.Unit.Mappings
{
    using System;
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication;
    using Helpers;
    using Prsd.Core.Mapper;
    using BusinessType = Core.Shared.BusinessType;

    public class BusinessTestBase
    {
        protected const string AnyString = "test";
        protected const string TestString = "Hover-board";

        protected static readonly Guid Guid1 = new Guid("3CA5AF6F-57A9-43D0-82AE-B19919D0580D");
        protected static readonly Guid Guid2 = new Guid("EA88DE5A-EA28-4852-8942-C42E25DEAE15");

        protected static readonly AddressDataEqualityComparer AddressComparer 
            = new AddressDataEqualityComparer();
        protected static readonly BusinessInfoDataEqualityComparer BusinessComparer 
            = new BusinessInfoDataEqualityComparer();
        protected static readonly ContactDataEqualityComparer ContactComparer 
            = new ContactDataEqualityComparer();

        protected BusinessInfoData ReturnBusiness { get; set; }
        protected AddressData ReturnAddress { get; set; }
        protected ContactData ReturnContact { get; set; }

        protected TestAddressMap addressMap;
        protected TestContactMap contactMap;
        protected TestBusinessMap businessMap;

        public BusinessTestBase()
        {
            ReturnAddress = new AddressData
            {
                StreetOrSuburb = AnyString,
                Address2 = AnyString,
                CountryId = new Guid("519FEB1B-2389-4D9C-AB47-FDCF87C45E35"),
                CountryName = AnyString,
                PostalCode = AnyString,
                Region = AnyString,
                TownOrCity = AnyString
            };

            ReturnBusiness = new BusinessInfoData
            {
                AdditionalRegistrationNumber = AnyString,
                BusinessType = BusinessType.LimitedCompany,
                Name = AnyString,
                RegistrationNumber = AnyString
            };

            ReturnContact = new ContactData
            {
                FullName = AnyString,
                Email = AnyString,
                Fax = AnyString,
                FaxPrefix = AnyString,
                Telephone = AnyString,
                TelephonePrefix = AnyString
            };

            addressMap = new TestAddressMap
            {
                AddressData = ReturnAddress
            };

            contactMap = new TestContactMap
            {
                ContactData = ReturnContact
            };

            businessMap = new TestBusinessMap
            {
                BusinessData = ReturnBusiness
            };
        }

        protected class TestAddressMap : IMap<Address, AddressData>
        {
            public AddressData AddressData { get; set; }
            public AddressData Map(Address source)
            {
                return AddressData;
            }
        }

        protected class TestContactMap : IMap<Contact, ContactData>
        {
            public ContactData ContactData { get; set; }
            public ContactData Map(Contact source)
            {
                return ContactData;
            }
        }

        protected class TestBusinessMap : IMap<Business, BusinessInfoData>
        {
            public BusinessInfoData BusinessData { get; set; }
            public BusinessInfoData Map(Business source)
            {
                return BusinessData;
            }
        }
    }
}
