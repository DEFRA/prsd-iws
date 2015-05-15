namespace EA.Iws.RequestHandlers
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Requests.Organisations;

    public class GetProducersByNotificationIdHandler : IRequestHandler<GetProducersByNotificationId, IList<ProducerData>>
    {
        private readonly IwsContext db;

        public GetProducersByNotificationIdHandler(IwsContext db)
        {
            this.db = db;
        }

        public async Task<IList<ProducerData>> HandleAsync(GetProducersByNotificationId message)
        {
            var result =
                await
                    db.NotificationApplications.Where(p => p.Id == message.NotificationId).ToArrayAsync();

            //var producerData = result.Select(c => new ProducerData
            //{
            //    Name = c.Business.Name,
            //    Address =
            //        new AddressData
            //        {
            //            Address2 = c.Address.Address2,
            //            Country = "United Kingdom",
            //            Building = c.Address.Building,
            //            PostalCode = c.Address.PostalCode,
            //            StreetOrSuburb = c.Address.Address1,
            //            TownOrCity = c.Address.TownOrCity
            //        },
            //    CompaniesHouseNumber = c.Business.RegistrationNumber,
            //    IsSiteOfExport = c.IsSiteOfExport,
            //    Contact = new ContactData
            //    {
            //        Email = c.Contact.Email,
            //        FirstName = c.Contact.FirstName,
            //        LastName = c.Contact.LastName,
            //        Fax = c.Contact.Fax,
            //        Telephone = c.Contact.Telephone
            //    },
            //    AdditionalRegistrationNumber = c.Business.AdditionalRegistrationNumber,
            //    NotificationId = c.Id
            //}).ToArray();
            var aa = result[0].Producers;
            return new List<ProducerData>();
        }
    }
}
