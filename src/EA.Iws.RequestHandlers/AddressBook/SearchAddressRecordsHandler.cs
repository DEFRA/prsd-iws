namespace EA.Iws.RequestHandlers.AddressBook
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.AddressBook;
    using Core.Shared;
    using DataAccess;
    using Domain;
    using Domain.AddressBook;
    using Prsd.Core.Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.AddressBook;

    public class SearchAddressRecordsHandler : IRequestHandler<SearchAddressRecords, IList<AddressBookRecordData>>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;
        private readonly IMap<AddressBookRecord, AddressBookRecordData> addressBookRecordMap;

        public SearchAddressRecordsHandler(IwsContext context, 
            IUserContext userContext,
            IMap<AddressBookRecord, AddressBookRecordData> addressBookRecordMap)
        {
            this.context = context;
            this.userContext = userContext;
            this.addressBookRecordMap = addressBookRecordMap;
        }

        public async Task<IList<AddressBookRecordData>> HandleAsync(SearchAddressRecords message)
        {
            var addressBook = await context.GetAddressBookForUserAsync(userContext, message.Type);

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
