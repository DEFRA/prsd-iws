namespace EA.Iws.RequestHandlers.Admin.Reports
{
    using EA.Iws.Core.Admin.Reports;
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.Reports;
    using EA.Iws.Domain.Reports;
    using EA.Iws.Requests.Admin.Reports;
    using EA.Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    internal class GetEADataReportHandler : IRequestHandler<GetEADataReport, EADataReportsData>
    {
        private readonly IMapWithParameter<Shipment, UKCompetentAuthority, ShipmentData> shipmentMapper;
        private readonly IMapWithParameter<DataExportNotification, UKCompetentAuthority, DataExportNotificationData> dataExportMapper;
        private readonly IMapWithParameter<DataImportNotification, UKCompetentAuthority, DataImportNotificationData> dataImportMapper;
        private readonly IShipmentsRepository shipmentsRepository;
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
            IMapWithParameter<DataImportNotification, UKCompetentAuthority, DataImportNotificationData> dataImportMapper)
        {
            this.shipmentsRepository = shipmentsRepository;
            this.financeReportRepository = financeReportRepository;
            this.producerReportRepository = producerReportRepository;
            this.foiRepository = foiRepository;
            this.exportNotificationsRepository = exportNotificationsRepository;
            this.importNotificationsRepository = importNotificationsRepository;
            this.shipmentMapper = shipmentMapper;
            this.dataExportMapper = dataExportMapper;
            this.dataImportMapper = dataImportMapper;
        }

        public async Task<EADataReportsData> HandleAsync(GetEADataReport message)
        {
            var authority = UKCompetentAuthority.England;
            var fromDate = message.FromDate;
            var toDate = message.ToDate;

            var selectedReports = message.SelectedReportList?.ToHashSet() ?? new HashSet<EAReportList>();

            var reportsData = new EADataReportsData();

            if (selectedReports.Contains(EAReportList.ShipmentReport))
            {
                var data = await shipmentsRepository
                    .GetShipmentReportData(fromDate, toDate, authority);

                reportsData.ShipmentReportData = data?.Select(x => shipmentMapper.Map(x, authority))
                    .ToArray();
            }

            if (selectedReports.Contains(EAReportList.FinanceReport))
            {
                var data = await financeReportRepository
                    .GetFinanceReportData(fromDate, toDate, authority);

                reportsData.FinanceReportData = data?.ToArray();
            }

            if (selectedReports.Contains(EAReportList.ProducerReport))
            {
                var data = await producerReportRepository
                    .GetProducerReportData(fromDate, toDate, authority);

                reportsData.ProducerReportData = data?.ToArray();
            }

            if (selectedReports.Contains(EAReportList.FOIReport))
            {
                var data = await foiRepository
                    .GetFOIReportData(fromDate, toDate, authority);

                reportsData.FreedomOfInformationReportData = data?.ToArray();
            }

            if (selectedReports.Contains(EAReportList.DataExportNotification))
            {
                var data = await exportNotificationsRepository
                    .GetDataExportNotificationData(fromDate, toDate, authority);

                reportsData.DataExportNotificationData = data?.Select(x => dataExportMapper.Map(x, authority))
                    .ToArray();
            }

            if (selectedReports.Contains(EAReportList.DataImportNotification))
            {
                var data = await importNotificationsRepository
                    .GetDataImportNotificationData(fromDate, toDate, authority);

                reportsData.DataImportNotificationData = data?.Select(x => dataImportMapper.Map(x, authority))
                    .ToArray();
            }

            return reportsData;
        }
    }
}
