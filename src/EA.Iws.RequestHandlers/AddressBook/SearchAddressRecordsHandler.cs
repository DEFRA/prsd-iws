namespace EA.Iws.RequestHandlers.AddressBook
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.AddressBook;
    using Domain.AddressBook;
    using Prsd.Core.Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.AddressBook;

    public class SearchAddressRecordsHandler : IRequestHandler<SearchAddressRecords, IList<AddressBookRecordData>>
    {
        private readonly IUserContext userContext;
        private readonly IMap<AddressBookRecord, AddressBookRecordData> addressBookRecordMap;
        private readonly IAddressBookRepository addressBookRepository;

        public SearchAddressRecordsHandler(IUserContext userContext,
            IMap<AddressBookRecord, AddressBookRecordData> addressBookRecordMap,
            IAddressBookRepository addressBookRepository)
        {
            this.userContext = userContext;
            this.addressBookRecordMap = addressBookRecordMap;
            this.addressBookRepository = addressBookRepository;
        }

        public async Task<IList<AddressBookRecordData>> HandleAsync(SearchAddressRecords message)
        {
            var addressBook = await addressBookRepository.GetAddressBookForUser(userContext.UserId, message.Type);

            if (!addressBook.Addresses.Any())
            {
                return new AddressBookRecordData[0];
            }

            return
                addressBook.Addresses.Where(
                    a => a.Business.Name.StartsWith(message.SearchTerm, StringComparison.OrdinalIgnoreCase))
                    .Select(addressBookRecordMap.Map).ToArray();
        }
    }
}
