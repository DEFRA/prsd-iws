namespace EA.Iws.Domain.AddressBook
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.AddressBook;

    public interface IAddressBookRepository
    {
        Task<AddressBook> GetAddressBookForUser(Guid userId, AddressRecordType type);

        Task<IList<AddressBook>> GetAllAddressBooksForUser(Guid userId);
    }
}
