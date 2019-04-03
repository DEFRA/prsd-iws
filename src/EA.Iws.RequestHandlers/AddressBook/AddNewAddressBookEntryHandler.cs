namespace EA.Iws.RequestHandlers.AddressBook
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.AddressBook;
    using Mappings;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.AddressBook;
    internal class AddNewAddressBookEntryHandler : IRequestHandler<AddNewAddressBookEntry, bool>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;
        private readonly IAddressBookRepository addressBookRepository;

        public AddNewAddressBookEntryHandler(IwsContext context, IUserContext userContext, IAddressBookRepository addressBookRepository)
        {
            this.context = context;
            this.userContext = userContext;
            this.addressBookRepository = addressBookRepository;
        }

        public async Task<bool> HandleAsync(AddNewAddressBookEntry message)
        {
            var addressBook = await addressBookRepository.GetAddressBookForUser(userContext.UserId, message.Type);
            var country = await context.Countries.SingleAsync(c => c.Id == message.Address.CountryId);
            var address = ValueObjectInitializer.CreateAddress(message.Address, country.Name); 
            var business = ValueObjectInitializer.CreateBusiness(message.Business);
            var contact = ValueObjectInitializer.CreateContact(message.Contact);

            addressBook.Add(new AddressBookRecord(address, business, contact));

            await addressBookRepository.Update(addressBook);

            return true;
        }
    }
}
