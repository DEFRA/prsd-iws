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

    internal class GetAddressBookRecordByIdHandler : IRequestHandler<GetAddressBookRecordById, AddressBookRecordData>
    {
        private readonly IUserContext userContext;
        private readonly IAddressBookRepository addressBookRepository;
        private readonly IMap<AddressBookRecord, AddressBookRecordData> addressBookRecordMap;

        public GetAddressBookRecordByIdHandler(IUserContext userContext, 
            IAddressBookRepository addressBookRepository,
            IMap<AddressBookRecord, AddressBookRecordData> addressBookRecordMap)
        {
            this.userContext = userContext;
            this.addressBookRepository = addressBookRepository;
            this.addressBookRecordMap = addressBookRecordMap;
        }

        public async Task<AddressBookRecordData> HandleAsync(GetAddressBookRecordById message)
        {
            var addressBook = await addressBookRepository.GetAddressBookForUser(userContext.UserId, message.Type);

            return addressBookRecordMap.Map(addressBook.Addresses.Single(abr => abr.Id == message.Id));
        }
    }
}
