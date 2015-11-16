namespace EA.Iws.RequestHandlers.ImportNotification.Summary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ImportNotification.Summary;
    using DataAccess.Draft;
    using Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using Address = Core.ImportNotification.Summary.Address;
    using Draft = Core.ImportNotification.Draft;

    internal class GetSummaryHandler : IRequestHandler<GetSummary, InProgressImportNotificationSummary>
    {
        private readonly IImportNotificationRepository importNotificationRepository;
        private readonly Domain.ICountryRepository countryRepository;
        private readonly IDraftImportNotificationRepository draftRepository;
        private readonly TransportRouteSummary transportRouteSummary;
        private IList<Domain.Country> countries = new List<Domain.Country>();

        public GetSummaryHandler(IImportNotificationRepository importNotificationRepository,
            Domain.ICountryRepository countryRepository,
            IDraftImportNotificationRepository draftRepository,
            TransportRouteSummary transportRouteSummary)
        {
            this.importNotificationRepository = importNotificationRepository;
            this.countryRepository = countryRepository;
            this.draftRepository = draftRepository;
            this.transportRouteSummary = transportRouteSummary;
        }

        public async Task<InProgressImportNotificationSummary> HandleAsync(GetSummary message)
        {
            var notification = await importNotificationRepository.GetByImportNotificationId(message.Id);

            countries = (await countryRepository.GetAll()).ToArray();

            var transportRoute = await transportRouteSummary.GetTransportRoute(message.Id, countries);

            var summary = new InProgressImportNotificationSummary
            {
                Id = notification.Id,
                Type = notification.NotificationType,
                Number = notification.NotificationNumber,
                Exporter = await GetExporter(message.Id),
                Facilities = await GetFacilities(message.Id),
                Importer = await GetImporter(message.Id),
                Producer = await GetProducer(message.Id),
                StateOfExport = transportRoute.StateOfExport,
                StateOfImport = transportRoute.StateOfImport,
                TransitStates = transportRoute.TransitStates
            };
            
            return summary;
        }

        private async Task<Producer> GetProducer(Guid id)
        {
            var producer = await draftRepository.GetDraftData<Draft.Producer>(id);

            return new Producer
            {
                Address = ConvertAddress(producer.Address),
                Name = producer.BusinessName,
                Contact = Contact.FromDraftContact(producer.Contact),
                AreMultiple = producer.AreMultiple
            };
        }

        private async Task<Exporter> GetExporter(Guid id)
        {
            var exporter = await draftRepository.GetDraftData<Draft.Exporter>(id);

            return new Exporter
            {
                Address = ConvertAddress(exporter.Address),
                Name = exporter.BusinessName,
                Contact = Contact.FromDraftContact(exporter.Contact)
            };
        }

        private async Task<Importer> GetImporter(Guid id)
        {
            var importer = await draftRepository.GetDraftData<Draft.Importer>(id);

            return new Importer
            {
                Address = ConvertAddress(importer.Address),
                BusinessType = importer.Type,
                Contact = Contact.FromDraftContact(importer.Contact),
                Name = importer.BusinessName,
                RegistrationNumber = importer.RegistrationNumber
            };
        }

        private async Task<IList<Facility>> GetFacilities(Guid id)
        {
            var facilitiesCollection = await draftRepository.GetDraftData<Draft.FacilityCollection>(id);

            if (facilitiesCollection.Facilities == null)
            {
                return new Facility[0];
            }

            return facilitiesCollection.Facilities.Select(f => 
            new Facility
            {
                Address = ConvertAddress(f.Address),
                Contact = Contact.FromDraftContact(f.Contact),
                Name = f.BusinessName,
                BusinessType = f.Type,
                RegistrationNumber = f.RegistrationNumber,
                IsActualSite = f.IsActualSite
            }).ToArray();
        }

        private Address ConvertAddress(Draft.Address address)
        {
            if (address == null)
            {
                return new Address();
            }

            return new Address
            {
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                Country = GetCountryName(address.CountryId),
                TownOrCity = address.TownOrCity,
                PostalCode = address.PostalCode
            };
        }

        private string GetCountryName(Guid? countryId)
        {
            if (countryId.HasValue)
            {
                var country = countries.SingleOrDefault(c => c.Id == countryId.Value);

                if (country != null)
                {
                    return country.Name;
                }
            }

            return string.Empty;
        }
    }
}
