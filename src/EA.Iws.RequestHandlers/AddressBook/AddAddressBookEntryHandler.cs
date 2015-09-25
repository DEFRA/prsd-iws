namespace EA.Iws.RequestHandlers.AddressBook
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.AddressBook;
    using DataAccess;
    using Domain;
    using Domain.AddressBook;
    using Mappings;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.AddressBook;

    internal class AddAddressBookEntryHandler : IRequestHandler<AddAddressBookEntry, bool>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public AddAddressBookEntryHandler(IwsContext context, IUserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<bool> HandleAsync(AddAddressBookEntry message)
        {
            var addressBook = await context.GetAddressBookForUserAsync(userContext, message.Type);

            var address = await GetAddress(message);
            var business = GetBusiness(message);
            var contact = ValueObjectInitializer.CreateContact(message.Contact);
            
            addressBook.AddAddress(new AddressBookRecord(address, business, contact));

            await SaveAddressBook(addressBook);

            return true;
        }

        private async Task<Address> GetAddress(AddAddressBookEntry message)
        {
            var country = await context.Countries.SingleAsync(c => c.Id == message.Address.CountryId);
            return ValueObjectInitializer.CreateAddress(message.Address, country.Name);
        }

        private Business GetBusiness(AddAddressBookEntry message)
        {
            return (message.Type == AddressRecordType.Producer) ? 
                ProducerBusiness.CreateProducerBusiness(message.Business.Name, BusinessType.FromBusinessType(message.Business.BusinessType),
                message.Business.RegistrationNumber, message.Business.OtherDescription)
                : Business.CreateBusiness(message.Business.Name, BusinessType.FromBusinessType(message.Business.BusinessType),
                message.Business.RegistrationNumber, message.Business.AdditionalRegistrationNumber);
        }

        private async Task SaveAddressBook(AddressBook addressBook)
        {
            if (context.Entry(addressBook).State == EntityState.Detached)
            {
                context.AddressBooks.Add(addressBook);
            }

            await context.SaveChangesAsync();
        }
    }
}
