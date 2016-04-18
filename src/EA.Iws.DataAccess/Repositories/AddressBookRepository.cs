namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.AddressBook;
    using DataAccess;
    using Domain.AddressBook;

    internal class AddressBookRepository : IAddressBookRepository
    {
        private readonly IwsContext context;

        public AddressBookRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<AddressBook> GetAddressBookForUser(Guid userId, AddressRecordType type)
        {
            var addressBook = await context.AddressBooks
                .SingleOrDefaultAsync(ab => ab.UserId == userId 
                    && ab.Type == type);
            
            return addressBook 
                ?? new AddressBook(new AddressBookRecord[0], type, userId);
        }

        public async Task<IList<AddressBook>> GetAllAddressBooksForUser(Guid userId)
        {
            return await context.AddressBooks.Where(ab => ab.UserId == userId).ToArrayAsync();
        }

        public async Task Update(AddressBook addressBook)
        {
            if (context.Entry(addressBook).State == EntityState.Detached)
            {
                context.AddressBooks.Add(addressBook);
            }

            await context.SaveChangesAsync();
        }
    }
}
