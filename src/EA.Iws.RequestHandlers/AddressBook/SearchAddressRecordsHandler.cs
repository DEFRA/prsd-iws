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
    using Prsd.Core.Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.AddressBook;

    public class SearchAddressRecordsHandler : IRequestHandler<SearchAddressRecords, IList<AddressBookRecordData>>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;
        private readonly IMap<Address, AddressData> addressMap;
        private readonly IMap<Business, BusinessInfoData> businessMap;
        private readonly IMap<Contact, ContactData> contactMap;

        public SearchAddressRecordsHandler(IwsContext context, 
            IUserContext userContext,
            IMap<Address, AddressData> addressMap,
            IMap<Business, BusinessInfoData> businessMap,
            IMap<Contact, ContactData> contactMap)
        {
            this.context = context;
            this.userContext = userContext;
            this.addressMap = addressMap;
            this.businessMap = businessMap;
            this.contactMap = contactMap;
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
                    .Select(a => new AddressBookRecordData
                    {
                        AddressData = addressMap.Map(a.Address),
                        BusinessData = businessMap.Map(a.Business),
                        ContactData = contactMap.Map(a.Contact)
                    }).ToArray();
        }
    }
}
