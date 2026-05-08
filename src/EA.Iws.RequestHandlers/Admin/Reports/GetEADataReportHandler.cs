namespace EA.Iws.RequestHandlers.Admin.Reports
{
    using EA.Iws.Core.Admin.Reports;
    using EA.Iws.Core.Notification;
    using EA.Iws.Domain.Reports;
    using EA.Iws.Requests.Admin.Reports;
    using EA.Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using System.Linq;
    using System.Threading.Tasks;

    internal class GetEADataReportHandler : IRequestHandler<GetEADataReport, EADataReportsData>
    {
        private readonly IShipmentsRepository shipmentsRepository;
        private readonly IMapWithParameter<Shipment, UKCompetentAuthority, ShipmentData> shipmentMapper;
        private readonly IMapWithParameter<DataExportNotification, UKCompetentAuthority, DataExportNotificationData> dataExportMapper;
        private readonly IMapWithParameter<DataImportNotification, UKCompetentAuthority, DataImportNotificationData> dataImportMapper;

        private readonly IFinanceReportRepository financeReportRepository;
        private readonly IProducerRepository producerReportRepository;
        private readonly IFreedomOfInformationRepository foiRepository;
        private readonly IExportNotificationsRepository exportNotificationsRepository;
        private readonly IImportNotificationsRepository importNotificationsRepository;

        public GetEADataReportHandler(IShipmentsRepository shipmentsRepository,
            IMapWithParameter<Shipment, UKCompetentAuthority, ShipmentData> shipmentMapper,
            IFinanceReportRepository financeReportRepository,
            IProducerRepository producerReportRepository,
            IFreedomOfInformationRepository foiRepository,
            IExportNotificationsRepository exportNotificationsRepository,
            IMapWithParameter<DataExportNotification, UKCompetentAuthority, DataExportNotificationData> dataExportMapper,
            IImportNotificationsRepository importNotificationsRepository,
            IMapWithParameter<DataImportNotification, UKCompetentAuthority, DataImportNotificationData> dataImportMapper,
            Domain.IInternalUserRepository internalUserRepository)
        {
            this.shipmentsRepository = shipmentsRepository;
            this.shipmentMapper = shipmentMapper;

            this.financeReportRepository = financeReportRepository;
            this.producerReportRepository = producerReportRepository;
            this.foiRepository = foiRepository;
            this.exportNotificationsRepository = exportNotificationsRepository;
            this.dataExportMapper = dataExportMapper;
            this.importNotificationsRepository = importNotificationsRepository;
            this.dataImportMapper = dataImportMapper;
        }

        public async Task<EADataReportsData> HandleAsync(GetEADataReport message)
        {
            var shipmentData = await shipmentsRepository.GetEAShipmentData(message.FromDate, message.ToDate);
            var financeData = await financeReportRepository.GetFinanceReport(message.FromDate, message.ToDate);
            var producerData = await producerReportRepository.GetProducerReport(message.FromDate, message.ToDate);
            var foiReportData = await foiRepository.GetFOIReport(message.FromDate, message.ToDate);
            var dataExportNotification = await exportNotificationsRepository.Get(message.FromDate, message.ToDate, UKCompetentAuthority.England);
            var dataImportNotification = await importNotificationsRepository.Get(message.FromDate, message.ToDate, UKCompetentAuthority.England);

            var reportsData = new EADataReportsData()
            {
                ShipmentReportData = shipmentData.Select(shipmentDatas => shipmentMapper.Map(shipmentDatas, UKCompetentAuthority.England)).ToArray(),
                FinanceReportData = financeData.ToArray(),
                ProducerReportData = producerData.ToArray(),
                FreedomOfInformationReportData = foiReportData.ToArray(),
                DataExportNotificationData = dataExportNotification.Select(exportData => dataExportMapper.Map(exportData, UKCompetentAuthority.England)).ToArray(),
                DataImportNotificationData = dataImportNotification.Select(importData => dataImportMapper.Map(importData, UKCompetentAuthority.England)).ToArray()
            };

            return reportsData;
        }
    }
}
