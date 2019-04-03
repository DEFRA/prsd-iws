namespace EA.Iws.RequestHandlers.Mappings.ImportNotification
{
    using Core.ImportNotification.Draft;
    using Domain;
    using Prsd.Core.Mapper;

    internal class ContactMap : IMap<Contact, Domain.ImportNotification.Contact>
    {
        public Domain.ImportNotification.Contact Map(Contact source)
        {
            var prefixedTelephone = source.Telephone;

            if (!string.IsNullOrEmpty(source.Telephone)
                && !string.IsNullOrEmpty(source.TelephonePrefix))
            {
                prefixedTelephone = string.Format("{0}-{1}", source.TelephonePrefix, source.Telephone);
            }

            return new Domain.ImportNotification.Contact(
                source.ContactName,
                new PhoneNumber(prefixedTelephone),
                new EmailAddress(source.Email));
        }
    }
}