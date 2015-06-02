namespace EA.Iws.RequestHandlers.Carrier
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Carriers;
    using Requests.Shared;

    internal class GetCarriersByNotificationIdHandler :
        IRequestHandler<GetCarriersByNotificationId, IEnumerable<CarrierData>>
    {
        private readonly IwsContext context;

        public GetCarriersByNotificationIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<CarrierData>> HandleAsync(GetCarriersByNotificationId message)
        {
            var result = await context.NotificationApplications.SingleAsync(n => n.Id == message.NotificationId);

            return result.Carriers.Select(c => new CarrierData
            {
                Id = c.Id,
                Business = new BusinessData
                {
                    Name = c.Business.Name,
                    EntityType = c.Business.Type,
                    RegistrationNumber = c.Business.RegistrationNumber,
                    AdditionalRegistrationNumber = c.Business.AdditionalRegistrationNumber
                },
                Address =
                    new AddressData
                    {
                        Address2 = c.Address.Address2,
                        CountryName = c.Address.Country,
                        Building = c.Address.Building,
                        PostalCode = c.Address.PostalCode,
                        StreetOrSuburb = c.Address.Address1,
                        TownOrCity = c.Address.TownOrCity
                    },
                Contact = new ContactData
                {
                    Email = c.Contact.Email,
                    FirstName = c.Contact.FirstName,
                    LastName = c.Contact.LastName,
                    Fax = c.Contact.Fax,
                    Telephone = c.Contact.Telephone
                },
                NotificationId = message.NotificationId
            }).ToArray();
        }
    }
}