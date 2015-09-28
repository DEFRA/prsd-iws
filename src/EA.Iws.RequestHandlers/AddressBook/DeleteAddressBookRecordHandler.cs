namespace EA.Iws.RequestHandlers.AddressBook
{
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.AddressBook;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.AddressBook;

    internal class DeleteAddressBookRecordHandler : IRequestHandler<DeleteAddressBookRecord, bool>
    {
        private readonly IAddressBookRepository addressBookRepository;
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public DeleteAddressBookRecordHandler(IAddressBookRepository addressBookRepository, 
            IwsContext context,
            IUserContext userContext)
        {
            this.addressBookRepository = addressBookRepository;
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<bool> HandleAsync(DeleteAddressBookRecord message)
        {
            var addressBook = await addressBookRepository.GetAddressBookForUser(userContext.UserId, message.Type);

            var recordToDelete = addressBook.Addresses.Single(abr => abr.Id == message.Id);

            addressBook.Delete(recordToDelete);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
