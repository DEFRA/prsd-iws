namespace EA.Iws.Requests.AddressBook
{
    using Core.AddressBook;
    using Prsd.Core.Mediator;

    public class GetUserAddressBookByType : IRequest<AddressBookData>
    {
        public AddressRecordType Type { get; private set; }

        public GetUserAddressBookByType(AddressRecordType type)
        {
            Type = type;
        }
    }
}
