namespace EA.Iws.RequestHandlers.Producers
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Producers;
    using Requests.Shared;

    public class GetProducersByNotificationIdHandler :
        IRequestHandler<GetProducersByNotificationId, IList<ProducerData>>
    {
        private readonly IwsContext db;

        public GetProducersByNotificationIdHandler(IwsContext db)
        {
            this.db = db;
        }

        public async Task<IList<ProducerData>> HandleAsync(GetProducersByNotificationId message)
        {
            var result = await db.NotificationApplications.Where(n => n.Id == message.NotificationId).SingleAsync();

            return result.Producers.Select(p => new ProducerData
            {
                Id = p.Id,
                Business = new BusinessData
                {
                    Name = p.Business.Name,
                    EntityType = p.Business.Type,
                    AdditionalRegistrationNumber = p.Business.AdditionalRegistrationNumber
                },
                Address =
                    new AddressData
                    {
                        Address2 = p.Address.Address2,
                        CountryName = p.Address.Country,
                        Building = p.Address.Building,
                        PostalCode = p.Address.PostalCode,
                        StreetOrSuburb = p.Address.Address1,
                        TownOrCity = p.Address.TownOrCity
                    },
                Contact = new ContactData
                {
                    Email = p.Contact.Email,
                    FirstName = p.Contact.FirstName,
                    LastName = p.Contact.LastName,
                    Fax = p.Contact.Fax,
                    Telephone = p.Contact.Telephone
                },
                NotificationId = message.NotificationId,
                IsSiteOfExport = p.IsSiteOfExport
            }).ToArray();
        }
    }
}