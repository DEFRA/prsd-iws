namespace EA.Iws.RequestHandlers.Tests.Unit.AddressBook
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.AddressBook;
    using Domain.AddressBook;
    using RequestHandlers.AddressBook;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class AddressBookRepositoryTests : TestBase
    {
        private readonly AddressBook addressBook;
        private readonly List<AddressBookRecord> addressBookRecords;
        private readonly AddressBookRepository addressBookRepository;

        public AddressBookRepositoryTests()
        {
            addressBookRecords = new List<AddressBookRecord>();

            addressBookRepository = new AddressBookRepository(Context);

            addressBook = new TestableAddressBook
            {
                UserId = UserId,
                Id = DeterministicGuid(1),
                Type = AddressRecordType.Producer,
                Addresses = addressBookRecords
            };
        }

        [Fact]
        public async Task UserHasNoAddressBook_ReturnsEmptyAddressBook()
        {
            var result = await addressBookRepository.GetAddressBookForUser(UserId, AddressRecordType.Producer);

            Assert.Empty(result.Addresses);
            Assert.Equal(AddressRecordType.Producer, result.Type);
        }

        [Fact]
        public async Task UserHasNoAddressBook_ReturnsAddressBookForUser()
        {
            var result = await addressBookRepository.GetAddressBookForUser(UserId, AddressRecordType.Producer);

            Assert.Equal(UserId, result.UserId);
        }

        [Fact]
        public async Task UserHasEmptyAddressBook_ReturnsAddressBook()
        {
            Context.AddressBooks.Add(addressBook);

            var result = await addressBookRepository.GetAddressBookForUser(UserId, AddressRecordType.Producer);

            Assert.Equal(DeterministicGuid(1), result.Id);
        }

        [Fact]
        public async Task UserHasAddressBookWithRecords_ReturnsAddressBook()
        {
            addressBookRecords.Add(new TestableAddressBookRecord
            {
                Address = TestableAddress.WitneyAddress,
                Business = TestableBusiness.LargeObjectHeap,
                Contact = TestableContact.MikeMerry
            });

            Context.AddressBooks.Add(addressBook);

            var result = await addressBookRepository.GetAddressBookForUser(UserId, AddressRecordType.Producer);

            Assert.Single(result.Addresses, abr => 
                abr.Address.Address1 == TestableAddress.WitneyAddress.Address1);
        }

        [Fact]
        public async Task UserHasAddressBookOfOtherType_DoesNotReturnWrongAddressBook()
        {
            Context.AddressBooks.Add(addressBook);

            var result = await addressBookRepository.GetAddressBookForUser(UserId, AddressRecordType.Carrier);

            Assert.Equal(Guid.Empty, result.Id);
        }
    }
}
