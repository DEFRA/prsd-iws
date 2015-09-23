namespace EA.Iws.RequestHandlers.AddressBook
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.AddressBook;
    using DataAccess;
    using Domain.AddressBook;
    using Prsd.Core.Domain;

    public static class IwsContextExtensions
    {
        public static async Task<AddressBook> GetAddressBookForUserAsync(this IwsContext context, 
            IUserContext userContext, 
            AddressRecordType type)
        {
            var userId = userContext.UserId;

            var addressBook = await context.AddressBooks
                .SingleOrDefaultAsync(ab => ab.UserId == userId 
                    && ab.Type == type);

            return addressBook 
                ?? new AddressBook(new AddressBookRecord[0], type, userId);
        }
    }
}
