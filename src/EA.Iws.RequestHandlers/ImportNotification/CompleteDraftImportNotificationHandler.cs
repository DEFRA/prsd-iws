namespace EA.Iws.RequestHandlers.ImportNotification
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ImportNotificationAssessment;
    using DataAccess;
    using DataAccess.Draft;
    using Domain.ImportNotification;
    using FluentValidation;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using ImportNotification = Core.ImportNotification.Draft.ImportNotification;

    internal class CompleteDraftImportNotificationHandler : IRequestHandler<CompleteDraftImportNotification, bool>
    {
        private readonly ImportNotificationContext context;
        private readonly IDraftImportNotificationRepository draftImportNotificationRepository;
        private readonly IExporterRepository exporterRepository;
        private readonly IFacilityRepository facilityRepository;
        private readonly IImporterRepository importerRepository;
        private readonly IImportNotificationRepository importNotificationRepository;
        private readonly IImportNotificationAssessmentRepository importNotificationAssessmentRepository;
        private readonly IValidator<ImportNotification> importNotificationValidator;
        private readonly IMapper mapper;
        private readonly IProducerRepository producerRepository;
        private readonly IShipmentRepository shipmentRepository;
        private readonly ITransportRouteRepository transportRouteRepository;
        private readonly IWasteOperationRepository wasteOperationRepository;
        private readonly IWasteTypeRepository wasteTypeRepository;

        public CompleteDraftImportNotificationHandler(IValidator<ImportNotification> importNotificationValidator,
            IDraftImportNotificationRepository draftImportNotificationRepository,
            IMapper mapper,
            IImportNotificationRepository importNotificationRepository,
            IImportNotificationAssessmentRepository importNotificationAssessmentRepository,
            IExporterRepository exporterRepository,
            IFacilityRepository facilityRepository,
            IImporterRepository importerRepository,
            IProducerRepository producerRepository,
            IShipmentRepository shipmentRepository,
            ITransportRouteRepository transportRouteRepository,
            IWasteOperationRepository wasteOperationRepository,
            IWasteTypeRepository wasteTypeRepository,
            ImportNotificationContext context)
        {
            this.importNotificationValidator = importNotificationValidator;
            this.draftImportNotificationRepository = draftImportNotificationRepository;
            this.mapper = mapper;
            this.importNotificationRepository = importNotificationRepository;
            this.importNotificationAssessmentRepository = importNotificationAssessmentRepository;
            this.exporterRepository = exporterRepository;
            this.facilityRepository = facilityRepository;
            this.importerRepository = importerRepository;
            this.producerRepository = producerRepository;
            this.shipmentRepository = shipmentRepository;
            this.transportRouteRepository = transportRouteRepository;
            this.wasteOperationRepository = wasteOperationRepository;
            this.wasteTypeRepository = wasteTypeRepository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(CompleteDraftImportNotification message)
        {
            var draft = await draftImportNotificationRepository.Get(message.ImportNotificationId);
            var assessment =
                await importNotificationAssessmentRepository.GetByNotification(message.ImportNotificationId);

            var result = await importNotificationValidator.ValidateAsync(draft);

            if (result.IsValid && assessment.Status == ImportNotificationStatus.NotificationReceived)
            {
                var notification =
                    await importNotificationRepository.Get(message.ImportNotificationId);

                var exporter = mapper.Map<Exporter>(draft.Exporter);
                var facilityCollection = mapper.Map<FacilityCollection>(draft.Facilities, draft.Preconsented);
                var importer = mapper.Map<Importer>(draft.Importer);
                var producer = mapper.Map<Producer>(draft.Producer);
                var shipment = mapper.Map<Shipment>(draft.Shipment, draft.Preconsented);

                var transportRoute = new Domain.ImportNotification.TransportRoute(message.ImportNotificationId,
                    mapper.Map<StateOfExport>(draft.StateOfExport),
                    mapper.Map<StateOfImport>(draft.StateOfImport));

                if (!draft.TransitStates.HasNoTransitStates)
                {
                    transportRoute.SetTransitStates(
                        new TransitStateList(draft.TransitStates.TransitStates.Select(t => mapper.Map<TransitState>(t))));
                }

                var wasteOperation = mapper.Map<WasteOperation>(draft.WasteOperation, notification);
                var wasteType = mapper.Map<Domain.ImportNotification.WasteType>(draft.WasteType, draft.ChemicalComposition.Composition);

                exporterRepository.Add(exporter);
                facilityRepository.Add(facilityCollection);
                importerRepository.Add(importer);
                producerRepository.Add(producer);
                shipmentRepository.Add(shipment);
                transportRouteRepository.Add(transportRoute);
                wasteOperationRepository.Add(wasteOperation);
                wasteTypeRepository.Add(wasteType);

                assessment.Submit();

                await context.SaveChangesAsync();
            }

            return result.IsValid;
        }
    }
}