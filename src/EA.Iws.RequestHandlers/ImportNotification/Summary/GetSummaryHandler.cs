namespace EA.Iws.RequestHandlers.ImportNotification.Summary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ImportNotification.Summary;
    using Core.ImportNotificationAssessment;
    using DataAccess.Draft;
    using Domain.ImportNotification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using Address = Core.ImportNotification.Summary.Address;
    using Contact = Core.ImportNotification.Summary.Contact;
    using Draft = Core.ImportNotification.Draft;
    using Exporter = Core.ImportNotification.Summary.Exporter;
    using Facility = Core.ImportNotification.Summary.Facility;
    using Importer = Core.ImportNotification.Summary.Importer;
    using Producer = Core.ImportNotification.Summary.Producer;
    using WasteOperation = Core.ImportNotification.Summary.WasteOperation;

    internal class GetSummaryHandler : IRequestHandler<GetSummary, ImportNotificationSummary>
    {
        private readonly IMapper mapper;
        private readonly IImportNotificationOverviewRepository summaryRepository;
        private readonly IImportNotificationAssessmentRepository assessmentRepository;
        private readonly IImportNotificationRepository importNotificationRepository;
        private readonly Domain.ICountryRepository countryRepository;
        private readonly IDraftImportNotificationRepository draftRepository;
        private readonly TransportRouteSummary transportRouteSummary;
        private readonly WasteTypeSummary wasteTypeSummary;
        private IList<Domain.Country> countries = new List<Domain.Country>();

        public GetSummaryHandler(IImportNotificationRepository importNotificationRepository,
            Domain.ICountryRepository countryRepository,
            IDraftImportNotificationRepository draftRepository,
            IImportNotificationAssessmentRepository assessmentRepository,
            TransportRouteSummary transportRouteSummary,
            WasteTypeSummary wasteTypeSummary,
            IImportNotificationOverviewRepository summaryRepository,
            IMapper mapper)
        {
            this.importNotificationRepository = importNotificationRepository;
            this.countryRepository = countryRepository;
            this.draftRepository = draftRepository;
            this.transportRouteSummary = transportRouteSummary;
            this.wasteTypeSummary = wasteTypeSummary;
            this.assessmentRepository = assessmentRepository;
            this.summaryRepository = summaryRepository;
            this.mapper = mapper;
        }

        public async Task<ImportNotificationSummary> HandleAsync(GetSummary message)
        {
            var status = await assessmentRepository.GetStatusByNotification(message.Id);

            if (status == ImportNotificationStatus.NotificationReceived)
            {
                var data = await draftRepository.Get(message.Id);

                var notification = await importNotificationRepository.Get(message.Id);

                countries = (await countryRepository.GetAll()).ToArray();

                var transportRoute = await transportRouteSummary.GetTransportRoute(message.Id, countries);

                return new ImportNotificationSummary
                {
                    Id = notification.Id,
                    Type = notification.NotificationType,
                    Number = notification.NotificationNumber,
                    Status = status,
                    Exporter = GetExporter(data),
                    Facilities = GetFacilities(data),
                    Importer = GetImporter(data),
                    Producer = GetProducer(data),
                    IntendedShipment = GetIntendedShipment(data),
                    StateOfExport = transportRoute.StateOfExport,
                    StateOfImport = transportRoute.StateOfImport,
                    TransitStates = transportRoute.TransitStates,
                    HasNoTransitStates = transportRoute.HasNoTransitStates,
                    WasteOperation = GetWasteOperation(data),
                    WasteType = await wasteTypeSummary.GetWasteType(message.Id),
                    AreFacilitiesPreconsented = GetFacilityPreconsent(data),
                    Composition = GetChemicalComposition(data)
                };
            }

            return mapper.Map<ImportNotificationSummary>(await summaryRepository.Get(message.Id));
        }

        private Producer GetProducer(Draft.ImportNotification notification)
        {
            return new Producer
            {
                Address = ConvertAddress(notification.Producer.Address),
                Name = notification.Producer.BusinessName,
                Contact = Contact.FromDraftContact(notification.Producer.Contact),
                AreMultiple = notification.Producer.AreMultiple
            };
        }

        private Exporter GetExporter(Draft.ImportNotification notification)
        {
            return new Exporter
            {
                Address = ConvertAddress(notification.Exporter.Address),
                Name = notification.Exporter.BusinessName,
                Contact = Contact.FromDraftContact(notification.Exporter.Contact)
            };
        }

        private Importer GetImporter(Draft.ImportNotification notification)
        {
            return new Importer
            {
                Address = ConvertAddress(notification.Importer.Address),
                BusinessType = notification.Importer.Type,
                Contact = Contact.FromDraftContact(notification.Importer.Contact),
                Name = notification.Importer.BusinessName,
                RegistrationNumber = notification.Importer.RegistrationNumber
            };
        }

        private IList<Facility> GetFacilities(Draft.ImportNotification notification)
        {
            if (notification.Facilities.Facilities == null)
            {
                return new Facility[0];
            }

            return notification.Facilities.Facilities.Select(f =>
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

        private static IntendedShipment GetIntendedShipment(Draft.ImportNotification notification)
        {
            return new IntendedShipment
            {
                TotalShipments = notification.Shipment.TotalShipments,
                Start = notification.Shipment.StartDate,
                End = notification.Shipment.EndDate,
                Quantity = notification.Shipment.Quantity,
                Units = notification.Shipment.Unit
            };
        }

        private static WasteOperation GetWasteOperation(Draft.ImportNotification notification)
        {
            return new WasteOperation
            {
                OperationCodes = notification.WasteOperation.OperationCodes,
                TechnologyEmployed = notification.WasteOperation.TechnologyEmployed
            };
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

        private static bool? GetFacilityPreconsent(Draft.ImportNotification notification)
        {
            return notification.Preconsented.AllFacilitiesPreconsented;
        }

        private static ChemicalComposition GetChemicalComposition(Draft.ImportNotification notification)
        {
            return new ChemicalComposition
            {
                Composition = notification.ChemicalComposition.Composition
            };
        }
    }
}