namespace EA.Iws.RequestHandlers.Tests.Unit.AddressBook
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.AddressBook;
    using Core.ImportNotification.Draft;
    using DataAccess;
    using DataAccess.Draft;
    using Domain;
    using Domain.AddressBook;
    using FakeItEasy;
    using Helpers;
    using ImportNotification.Validate;
    using Prsd.Core.Domain;
    using RequestHandlers.AddressBook;
    using Requests.AddressBook;
    using TestHelpers.Helpers;
    using Xunit;

    public class AddImportAddressBookEntryHandlerTests
    {
        private AddImportAddressBookEntryHandler handler;
        private readonly AddImportAddressBookEntry message;
        private readonly IwsContext context;
        private readonly AddressBook addressBook;

        private readonly IUserContext userContext;
        private readonly IAddressBookRepository addressBookRepository;
        private readonly IDraftImportNotificationRepository draftImportNotificationRepository;

        private readonly Guid notificationId = Guid.NewGuid();
        private readonly Guid userId = Guid.NewGuid();
        private readonly Guid countryId = Guid.NewGuid();

        public AddImportAddressBookEntryHandlerTests()
        {
            context = new TestIwsContext();
            userContext = A.Fake<IUserContext>();
            addressBookRepository = A.Fake<IAddressBookRepository>();
            draftImportNotificationRepository = A.Fake<IDraftImportNotificationRepository>();
            message = new AddImportAddressBookEntry(notificationId);

            var country = CountryFactory.Create(countryId);
            context.Countries.Add(country);

            addressBook = A.Fake<AddressBook>();

            A.CallTo(() => userContext.UserId).Returns(userId);
            A.CallTo(() => addressBookRepository.GetAddressBookForUser(userContext.UserId, AddressRecordType.Carrier)).Returns(addressBook);
        }

        private ImportNotification GetTestImportNotification(bool addToAddressBook)
        {
            ImportNotification importNotification = new ImportNotification()
            {
                Exporter = GetValidExporter(addToAddressBook),
                Importer = GetValidImporter(addToAddressBook),
                Producer = GetValidProducer(addToAddressBook),
                Facilities = GetValidFacilities(addToAddressBook)
            };

            return importNotification;
        }

        private Exporter GetValidExporter(bool addedToAddressBook)
        {
            return new Exporter(Guid.NewGuid())
            {
                Address = AddressTestData.GetValidTestAddress(countryId),
                Contact = ContactTestData.GetValidTestContact(),
                BusinessName = "Test exporter",
                IsAddedToAddressBook = addedToAddressBook
            };
        }

        private Producer GetValidProducer(bool addedToAddressBook)
        {
            return new Producer(Guid.NewGuid())
            {
                Address = AddressTestData.GetValidTestAddress(countryId),
                Contact = ContactTestData.GetValidTestContact(),
                BusinessName = "Test producer",
                IsAddedToAddressBook = addedToAddressBook
            };
        }

        private Importer GetValidImporter(bool addedToAddressBook)
        {
            return new Importer(Guid.NewGuid())
            {
                Address = AddressTestData.GetValidTestAddress(countryId),
                Contact = ContactTestData.GetValidTestContact(),
                BusinessName = "Test importer",
                IsAddedToAddressBook = addedToAddressBook
            };
        }

        private FacilityCollection GetValidFacilities(bool addedToAddressBook)
        {
            FacilityCollection result = new FacilityCollection();
            result.Facilities = new System.Collections.Generic.List<Facility>()
            {
                new Facility(Guid.NewGuid())
                    {
                        Address = AddressTestData.GetValidTestAddress(countryId),
                        Contact = ContactTestData.GetValidTestContact(),
                        BusinessName = "Test facility",
                        IsAddedToAddressBook = addedToAddressBook
                    }
            };

            return result;
        }

        [Fact]
        public async Task IsAddedToAddressBook_AddedToAddressBook()
        {
            var testNotification = GetTestImportNotification(true);
            A.CallTo(() => draftImportNotificationRepository.Get(notificationId)).Returns(testNotification);

            handler = new AddImportAddressBookEntryHandler(context, userContext, addressBookRepository, draftImportNotificationRepository);

            var result = await handler.HandleAsync(message);

            Assert.Equal(result, true);
            A.CallTo(() => addressBookRepository.Update(addressBook)).MustHaveHappened();
        }

        [Fact]
        public async Task NotAddedToAddressBook_NotToAddressBook()
        {
            var testNotification = GetTestImportNotification(false);
            A.CallTo(() => draftImportNotificationRepository.Get(notificationId)).Returns(testNotification);

            handler = new AddImportAddressBookEntryHandler(context, userContext, addressBookRepository, draftImportNotificationRepository);

            var result = await handler.HandleAsync(message);

            Assert.Equal(result, false);
            A.CallTo(() => addressBookRepository.Update(addressBook)).MustNotHaveHappened();
        }
    }
}
