namespace EA.Iws.RequestHandlers.AddressBook
{
    using System.Threading.Tasks;
    using Core.AddressBook;
    using DataAccess;
    using Domain.AddressBook;
    using Prsd.Core.Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.AddressBook;

    internal class GetUserAddressBookByTypeHandler : IRequestHandler<GetUserAddressBookByType, AddressBookData>
    {
        private readonly IMap<AddressBook, AddressBookData> addressBookMap;
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public GetUserAddressBookByTypeHandler(IwsContext context,
            IUserContext userContext,
            IMap<AddressBook, AddressBookData> addressBookMap)
        {
            this.context = context;
            this.userContext = userContext;
            this.addressBookMap = addressBookMap;
        }

        public async Task<AddressBookData> HandleAsync(GetUserAddressBookByType message)
        {
            var addressBook = await context.GetAddressBookForUserAsync(userContext, 
                message.Type);

            return addressBookMap.Map(addressBook);
        }
    }
}