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

    public class IwsContextExtensionsTests : TestBase
    {
        private readonly AddressBook addressBook;
        private readonly List<AddressBookRecord> addressBookRecords; 

        public IwsContextExtensionsTests()
        {
            addressBookRecords = new List<AddressBookRecord>();

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
            var result = await Context.GetAddressBookForUserAsync(UserContext, AddressRecordType.Producer);

            Assert.Empty(result.Addresses);
            Assert.Equal(AddressRecordType.Producer, result.Type);
        }

        [Fact]
        public async Task UserHasNoAddressBook_ReturnsAddressBookForUser()
        {
            var result = await Context.GetAddressBookForUserAsync(UserContext, AddressRecordType.Producer);

            Assert.Equal(UserId, result.UserId);
        }

        [Fact]
        public async Task UserHasEmptyAddressBook_ReturnsAddressBook()
        {
            Context.AddressBooks.Add(addressBook);

            var result = await Context.GetAddressBookForUserAsync(UserContext, AddressRecordType.Producer);

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

            var result = await Context.GetAddressBookForUserAsync(UserContext, AddressRecordType.Producer);

            Assert.Single(result.Addresses, abr => 
                abr.Address.Address1 == TestableAddress.WitneyAddress.Address1);
        }

        [Fact]
        public async Task UserHasAddressBookOfOtherType_DoesNotReturnWrongAddressBook()
        {
            Context.AddressBooks.Add(addressBook);

            var result = await Context.GetAddressBookForUserAsync(UserContext, AddressRecordType.Carrier);

            Assert.Equal(Guid.Empty, result.Id);
        }
    }
}
