namespace EA.Iws.RequestHandlers.AddressBook
{
    using System.Threading.Tasks;
    using Core.AddressBook;
    using Domain.AddressBook;
    using Prsd.Core.Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.AddressBook;

    internal class GetUserAddressBookByTypeHandler : IRequestHandler<GetUserAddressBookByType, AddressBookData>
    {
        private readonly IMap<AddressBook, AddressBookData> addressBookMap;
        private readonly IAddressBookRepository addressBookRepository;
        private readonly IUserContext userContext;

        public GetUserAddressBookByTypeHandler(IUserContext userContext,
            IMap<AddressBook, AddressBookData> addressBookMap,
            IAddressBookRepository addressBookRepository)
        {
            this.userContext = userContext;
            this.addressBookMap = addressBookMap;
            this.addressBookRepository = addressBookRepository;
        }

        public async Task<AddressBookData> HandleAsync(GetUserAddressBookByType message)
        {
            var addressBook = await addressBookRepository.GetAddressBookForUser(userContext.UserId, message.Type);

            return addressBookMap.Map(addressBook);
        }
    }
}