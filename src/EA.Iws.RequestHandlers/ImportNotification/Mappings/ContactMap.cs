namespace EA.Iws.RequestHandlers.ImportNotification.Mappings
{
    using Prsd.Core.Mapper;
    using Core = Core.ImportNotification.Summary;
    using Domain = Domain.ImportNotification;

    internal class ContactMap : IMap<Domain.Contact, Core.Contact>
    {
        public Core.Contact Map(Domain.Contact source)
        {
            var contact = new Core.Contact
            {
                Email = source.Email,
                Name = source.Name
            };

            if (source.Telephone != null
                && source.Telephone.Value.Contains("-")
                && (source.Telephone.Value.Split('-').Length > 0))
            {
                contact.TelephonePrefix = source.Telephone.Value.Split('-')[0];
                contact.Telephone = source.Telephone.Value.Split('-')[1];
            }
            else
            {
                contact.Telephone = source.Telephone;
            }

            return contact;
        }
    }
}