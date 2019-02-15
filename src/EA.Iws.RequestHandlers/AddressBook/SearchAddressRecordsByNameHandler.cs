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

            AddressBook result = new AddressBook(addressBook.Addresses.Where(p => p.Business.Name.ToLower().Contains(message.SearchTerm.ToLower())), message.Type, userContext.UserId);

            return addressBookMap.Map(result);
        }
    }
}
