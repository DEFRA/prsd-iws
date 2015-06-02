namespace EA.Iws.RequestHandlers.Exporters
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Exporters;
    using Requests.Shared;

    internal class GetExporterByNotificationIdHandler : IRequestHandler<GetExporterByNotificationId, ExporterData>
    {
        private readonly IwsContext context;

        public GetExporterByNotificationIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<ExporterData> HandleAsync(GetExporterByNotificationId message)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.NotificationId);

            return new ExporterData
            {
                NotificationId = message.NotificationId,
                Business = new BusinessData
                {
                    Name = notification.Exporter.Business.Name,
                    EntityType = notification.Exporter.Business.Type,
                    AdditionalRegistrationNumber = notification.Exporter.Business.AdditionalRegistrationNumber,
                    RegistrationNumber = notification.Exporter.Business.RegistrationNumber
                },
                Address = new AddressData
                {
                    Building = notification.Exporter.Address.Building,
                    StreetOrSuburb = notification.Exporter.Address.Address1,
                    Address2 = notification.Exporter.Address.Address2,
                    TownOrCity = notification.Exporter.Address.TownOrCity,
                    PostalCode = notification.Exporter.Address.PostalCode,
                    CountryName = notification.Exporter.Address.Country
                },
                Contact = new ContactData
                {
                    FirstName = notification.Exporter.Contact.FirstName,
                    LastName = notification.Exporter.Contact.LastName,
                    Telephone = notification.Exporter.Contact.Telephone,
                    Fax = notification.Exporter.Contact.Fax,
                    Email = notification.Exporter.Contact.Email
                }
            };
        }
    }
}