namespace EA.Iws.RequestHandlers.AddressBook
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.AddressBook;
    using DataAccess;
    using Domain;
    using Domain.AddressBook;
    using Domain.NotificationApplication;
    using Mappings;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.AddressBook;

    internal class AddAddressBookEntryHandler : IRequestHandler<AddAddressBookEntry, bool>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;
        private readonly IAddressBookRepository addressBookRepository;

        public AddAddressBookEntryHandler(IwsContext context, IUserContext userContext, IAddressBookRepository addressBookRepository)
        {
            this.context = context;
            this.userContext = userContext;
            this.addressBookRepository = addressBookRepository;
        }

        public async Task<bool> HandleAsync(AddAddressBookEntry message)
        {
            var addressBook = await addressBookRepository.GetAddressBookForUser(userContext.UserId, message.Type);

            var address = await GetAddress(message);
            var business = GetBusiness(message);
            var contact = ValueObjectInitializer.CreateContact(message.Contact);
            
            addressBook.Add(new AddressBookRecord(address, business, contact));

            await addressBookRepository.Update(addressBook);

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
                CreateProducerBusiness(message)
                : CreateBusiness(message);
        }

        private Business CreateProducerBusiness(AddAddressBookEntry message)
        {
            return ProducerBusiness.CreateProducerBusiness(message.Business.Name,
                BusinessType.FromBusinessType(message.Business.BusinessType),
                message.Business.RegistrationNumber, message.Business.OtherDescription);
        }

        private Business CreateBusiness(AddAddressBookEntry message)
        {
            return (message.Business.BusinessType == Core.Shared.BusinessType.Other) ? 
                Business.CreateOtherBusiness(message.Business.Name, message.Business.RegistrationNumber, 
                message.Business.AdditionalRegistrationNumber, message.Business.OtherDescription) 
                : Business.CreateBusiness(message.Business.Name, BusinessType.FromBusinessType(message.Business.BusinessType),
                message.Business.RegistrationNumber, message.Business.AdditionalRegistrationNumber);
        }
    }
}
