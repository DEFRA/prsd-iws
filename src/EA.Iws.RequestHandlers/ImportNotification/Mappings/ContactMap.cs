namespace EA.Iws.RequestHandlers.ImportNotification.Mappings
{
    using Prsd.Core.Mapper;
    using Core = Core.ImportNotification.Summary;
    using Domain = Domain.ImportNotification;

    internal class ContactMap : IMap<Domain.Contact, Core.Contact>
    {
        public Core.Contact Map(Domain.Contact source)
        {
            return new Core.Contact
            {
                Email = source.Email,
                Name = source.Name,
                Telephone = source.Telephone.Value
            };
        }
    }
}