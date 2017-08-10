namespace EA.Iws.RequestHandlers.Carrier
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.AddressBook;
    using DataAccess;
    using Domain.AddressBook;
    using Domain.NotificationApplication;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Carriers;

    internal class AddCarriersToNotificationFromAddressBookHandler :
        IRequestHandler<AddCarriersToNotificationFromAddressBook, Unit>
    {
        private readonly IAddressBookRepository addressBookRepository;
        private readonly ICarrierRepository carrierRepository;
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public AddCarriersToNotificationFromAddressBookHandler(IAddressBookRepository addressBookRepository,
            ICarrierRepository carrierRepository, IwsContext context, IUserContext userContext)
        {
            this.addressBookRepository = addressBookRepository;
            this.carrierRepository = carrierRepository;
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<Unit> HandleAsync(AddCarriersToNotificationFromAddressBook message)
        {
            var addressBook = await addressBookRepository.GetAddressBookForUser(userContext.UserId, AddressRecordType.Carrier);
            var carriers = await carrierRepository.GetByNotificationId(message.NotificationId);

            var carrierAddresses = addressBook.Addresses.Where(a => message.AddressBookItemIds.Contains(a.Id));

            foreach (var address in carrierAddresses)
            {
                carriers.AddCarrier(address.Business, address.Address, address.Contact);
            }

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}