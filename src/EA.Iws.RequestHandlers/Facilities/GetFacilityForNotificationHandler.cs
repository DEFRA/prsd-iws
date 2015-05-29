namespace EA.Iws.RequestHandlers.Facilities
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Facilities;
    using Requests.Shared;

    internal class GetFacilityForNotificationHandler : IRequestHandler<GetFacilityForNotification, FacilityData>
    {
        private readonly IwsContext context;

        public GetFacilityForNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<FacilityData> HandleAsync(GetFacilityForNotification message)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.NotificationId);
            var facility = notification.GetFacility(message.FacilityId);

            return new FacilityData
            {
                Id = facility.Id,
                Business = new BusinessData
                {
                    Name = facility.Business.Name,
                    EntityType = facility.Business.Type,
                    RegistrationNumber = facility.Business.RegistrationNumber,
                    AdditionalRegistrationNumber = facility.Business.AdditionalRegistrationNumber
                },
                Address =
                    new AddressData
                    {
                        Address2 = facility.Address.Address2,
                        CountryName = facility.Address.Country,
                        Building = facility.Address.Building,
                        PostalCode = facility.Address.PostalCode,
                        StreetOrSuburb = facility.Address.Address1,
                        TownOrCity = facility.Address.TownOrCity
                    },
                Contact = new ContactData
                {
                    Email = facility.Contact.Email,
                    FirstName = facility.Contact.FirstName,
                    LastName = facility.Contact.LastName,
                    Fax = facility.Contact.Fax,
                    Telephone = facility.Contact.Telephone
                },
                NotificationId = message.NotificationId,
                IsActualSiteOfTreatment = facility.IsActualSiteOfTreatment
            };
        }
    }
}