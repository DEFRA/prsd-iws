namespace EA.Iws.RequestHandlers.Importers
{
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Importer;
    using Requests.Shared;

    internal class GetImporterByNotificationIdHandler : IRequestHandler<GetImporterByNotificationId, ImporterData>
    {
        private readonly IwsContext context;

        public GetImporterByNotificationIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<ImporterData> HandleAsync(GetImporterByNotificationId message)
        {
            var notification = await context.NotificationApplications.FindAsync(message.NotificationId);

            return new ImporterData
            {
                NotificationId = message.NotificationId,
                Business = new BusinessData
                {
                    Name = notification.Importer.Business.Name,
                    EntityType = notification.Importer.Business.Type,
                    AdditionalRegistrationNumber = notification.Importer.Business.AdditionalRegistrationNumber,
                    RegistrationNumber = notification.Importer.Business.RegistrationNumber
                },
                Address = new AddressData
                {
                    Building = notification.Importer.Address.Building,
                    StreetOrSuburb = notification.Importer.Address.Address1,
                    Address2 = notification.Importer.Address.Address2,
                    TownOrCity = notification.Importer.Address.TownOrCity,
                    Region = notification.Importer.Address.Region,
                    PostalCode = notification.Importer.Address.PostalCode,
                    CountryName = notification.Importer.Address.Country
                },
                Contact = new ContactData
                {
                    FirstName = notification.Importer.Contact.FirstName,
                    LastName = notification.Importer.Contact.LastName,
                    Telephone = notification.Importer.Contact.Telephone,
                    Fax = notification.Importer.Contact.Fax,
                    Email = notification.Importer.Contact.Email
                }
            };
        }
    }
}