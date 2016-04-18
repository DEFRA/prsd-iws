namespace EA.Iws.RequestHandlers.Mappings.ImportNotification
{
    using Core.ImportNotification.Draft;
    using Domain;
    using Prsd.Core.Mapper;

    internal class ContactMap : IMap<Contact, Domain.ImportNotification.Contact>
    {
        public Domain.ImportNotification.Contact Map(Contact source)
        {
            return new Domain.ImportNotification.Contact(
                source.ContactName,
                new PhoneNumber(source.Telephone),
                new EmailAddress(source.Email));
        }
    }
}