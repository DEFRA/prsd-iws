namespace EA.Iws.RequestHandlers.Admin.Reports
{
    using EA.Iws.Core.Admin.Reports;
    using EA.Iws.Core.Notification;
    using EA.Iws.Domain.Reports;
    using EA.Iws.Requests.Admin.Reports;
    using EA.Prsd.Core.Domain;
    using EA.Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using System.Linq;
    using System.Threading.Tasks;

    internal class GetEADataReportHandler : IRequestHandler<GetEADataReport, EADataReportsData>
    {
        private readonly IShipmentsRepository shipmentsRepository;
        private readonly IMapWithParameter<Shipment, UKCompetentAuthority, ShipmentData> shipmentMapper;

        private readonly IFinanceReportRepository financeReportRepository;
        private readonly IProducerRepository producerReportRepository;
        private readonly IFreedomOfInformationRepository foiRepository;

        private readonly IUserContext userContext;
        private readonly Domain.IInternalUserRepository internalUserRepository;

        public GetEADataReportHandler(IShipmentsRepository shipmentsRepository,
            IMapWithParameter<Shipment, UKCompetentAuthority, ShipmentData> shipmentMapper,
            IFinanceReportRepository financeReportRepository,
            IProducerRepository producerReportRepository,
            IFreedomOfInformationRepository foiRepository,
            IUserContext userContext,
            Domain.IInternalUserRepository internalUserRepository)
        {
            this.shipmentsRepository = shipmentsRepository;
            this.shipmentMapper = shipmentMapper;

            this.financeReportRepository = financeReportRepository;
            this.producerReportRepository = producerReportRepository;
            this.foiRepository = foiRepository;

            this.userContext = userContext;
            this.internalUserRepository = internalUserRepository;
        }

        public async Task<EADataReportsData> HandleAsync(GetEADataReport message)
        {
            var user = await internalUserRepository.GetByUserId(userContext.UserId);
            var shipmentData = await shipmentsRepository.GetEAShipmentData(message.FromDate, message.ToDate);
            var financeData = await financeReportRepository.GetFinanceReport(message.FromDate, message.ToDate);
            var producerData = await producerReportRepository.GetProducerReport(message.FromDate, message.ToDate);
            var foiReportData = await foiRepository.GetFOIReport(message.FromDate, message.ToDate);

            var reportsData = new EADataReportsData()
            {
                ShipmentReportData = shipmentData.Select(x => shipmentMapper.Map(x, user.CompetentAuthority)).ToArray(),
                FinanceReportData = financeData.ToArray(),
                ProducerReportData = producerData.ToArray(),
                FreedomOfInformationReportData = foiReportData.ToArray()
            };

            return reportsData;
        }
    }
}
