namespace EA.Iws.RequestHandlers.Carrier
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Carriers;
    using Requests.Shared;

    internal class GetCarrierForNotificationHandler : IRequestHandler<GetCarrierForNotification, CarrierData>
    {
        private readonly IwsContext context;

        public GetCarrierForNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<CarrierData> HandleAsync(GetCarrierForNotification message)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.NotificationId);
            var carrier = notification.GetCarrier(message.CarrierId);

            return new CarrierData
            {
                Id = carrier.Id,
                Business = new BusinessData
                {
                    Name = carrier.Business.Name,
                    EntityType = carrier.Business.Type,
                    RegistrationNumber = carrier.Business.RegistrationNumber,
                    AdditionalRegistrationNumber = carrier.Business.AdditionalRegistrationNumber
                },
                Address =
                    new AddressData
                    {
                        Address2 = carrier.Address.Address2,
                        CountryName = carrier.Address.Country,
                        Building = carrier.Address.Building,
                        PostalCode = carrier.Address.PostalCode,
                        StreetOrSuburb = carrier.Address.Address1,
                        TownOrCity = carrier.Address.TownOrCity
                    },
                Contact = new ContactData
                {
                    Email = carrier.Contact.Email,
                    FirstName = carrier.Contact.FirstName,
                    LastName = carrier.Contact.LastName,
                    Fax = carrier.Contact.Fax,
                    Telephone = carrier.Contact.Telephone
                },
                NotificationId = message.NotificationId
            };
        }
    }
}