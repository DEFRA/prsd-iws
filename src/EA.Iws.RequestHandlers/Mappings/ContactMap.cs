namespace EA.Iws.RequestHandlers.Mappings
{
    using Domain;
    using Prsd.Core.Mapper;
    using Requests.Shared;

    internal class ContactMap : IMap<Contact, ContactData>
    {
        public ContactData Map(Contact source)
        {
            return new ContactData
            {
                FirstName = source.FirstName,
                LastName = source.LastName,
                Telephone = source.Telephone,
                Fax = source.Fax,
                Email = source.Email
            };
        }
    }
}
