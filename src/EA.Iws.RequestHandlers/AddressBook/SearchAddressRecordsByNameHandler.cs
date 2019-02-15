namespace EA.Iws.RequestHandlers.AddressBook
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.AddressBook;
    using Domain.AddressBook;
    using Prsd.Core.Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.AddressBook;

    public class SearchAddressRecordsByNameHandler : IRequestHandler<SearchAddressRecordsByName, AddressBookData>
    {
        private readonly IUserContext userContext;
        private readonly IMap<AddressBook, AddressBookData> addressBookMap;
        private readonly IAddressBookRepository addressBookRepository;

        private const int PageSize = 5;

        public SearchAddressRecordsByNameHandler(IUserContext userContext,
            IMap<AddressBook, AddressBookData> addressBookMap,
            IAddressBookRepository addressBookRepository)
        {
            this.userContext = userContext;
            this.addressBookMap = addressBookMap;
            this.addressBookRepository = addressBookRepository;
        }

        public async Task<AddressBookData> HandleAsync(SearchAddressRecordsByName message)
        {
            var addressBook = await addressBookRepository.GetAddressBookForUser(userContext.UserId, message.Type);

            if (!addressBook.Addresses.Any())
            {
                return new AddressBookData();
            }

            var addresses = addressBook.Addresses.Where(p => p.Business.Name.ToLower().Contains(message.SearchTerm.ToLower()));

            AddressBook result = new AddressBook(addresses.Skip((message.PageNumber - 1) * PageSize).Take(PageSize), message.Type, userContext.UserId);

            var returnData = addressBookMap.Map(result);
            returnData.NumberOfMatchedRecords = addresses.Count();
            returnData.PageNumber = message.PageNumber;
            returnData.PageSize = PageSize;

            return returnData;
        }
    }
}
