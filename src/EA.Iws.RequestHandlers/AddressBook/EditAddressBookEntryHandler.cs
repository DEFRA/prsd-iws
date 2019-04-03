namespace EA.Iws.RequestHandlers.AddressBook
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.AddressBook;
    using DataAccess;
    using Domain.AddressBook;
    using EA.Iws.Requests.AddressBook;
    using Mappings;
    using Prsd.Core.Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;

    internal class EditAddressBookEntryHandler : IRequestHandler<EditAddressBookEntry, bool>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;
        private readonly IAddressBookRepository addressBookRepository;
        private readonly IMap<AddressBookRecord, AddressBookRecordData> addressBookRecordMap;

        public EditAddressBookEntryHandler(IwsContext context, IUserContext userContext, IAddressBookRepository addressBookRepository, IMap<AddressBookRecord, AddressBookRecordData> addressBookRecordMap)
        {
            this.context = context;
            this.userContext = userContext;
            this.addressBookRepository = addressBookRepository;
            this.addressBookRecordMap = addressBookRecordMap;
        }

        public async Task<bool> HandleAsync(EditAddressBookEntry message)
        {
            var addressBook = await addressBookRepository.GetAddressBookForUser(userContext.UserId, message.Type);

            var addressBookRecord = addressBook.Addresses.Single(abr => abr.Id == message.Id);

            var country = await context.Countries.SingleAsync(c => c.Id == message.Address.CountryId);

            var address = ValueObjectInitializer.CreateAddress(message.Address, country.Name);
            var contact = ValueObjectInitializer.CreateContact(message.Contact);
            var business = ValueObjectInitializer.CreateBusiness(message.Business);

            addressBookRecord.Update(address, business, contact);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
