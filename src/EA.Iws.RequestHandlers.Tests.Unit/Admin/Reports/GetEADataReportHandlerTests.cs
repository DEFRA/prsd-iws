namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EA.Iws.Core.Admin.Reports;
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.Reports;
    using EA.Iws.Domain.Reports;
    using EA.Iws.RequestHandlers.Admin.Reports;
    using EA.Iws.Requests.Admin.Reports;
    using EA.Prsd.Core.Mapper;
    using FakeItEasy;
    using Xunit;

    public class GetEADataReportHandlerTests
    {
        private readonly IShipmentsRepository shipmentsRepository;
        private readonly IFinanceReportRepository financeReportRepository;
        private readonly IProducerRepository producerReportRepository;
        private readonly IFreedomOfInformationRepository foiRepository;
        private readonly IExportNotificationsRepository exportNotificationsRepository;
        private readonly IImportNotificationsRepository importNotificationsRepository;

        private readonly IMapWithParameter<Shipment, UKCompetentAuthority, ShipmentData> shipmentMapper;
        private readonly IMapWithParameter<DataExportNotification, UKCompetentAuthority, DataExportNotificationData> dataExportMapper;
        private readonly IMapWithParameter<DataImportNotification, UKCompetentAuthority, DataImportNotificationData> dataImportMapper;

        private readonly GetEADataReportHandler handler;

        public GetEADataReportHandlerTests()
        {
            shipmentsRepository = A.Fake<IShipmentsRepository>();
            financeReportRepository = A.Fake<IFinanceReportRepository>();
            producerReportRepository = A.Fake<IProducerRepository>();
            foiRepository = A.Fake<IFreedomOfInformationRepository>();
            exportNotificationsRepository = A.Fake<IExportNotificationsRepository>();
            importNotificationsRepository = A.Fake<IImportNotificationsRepository>();

            shipmentMapper =
                A.Fake<IMapWithParameter<Shipment, UKCompetentAuthority, ShipmentData>>();

            dataExportMapper =
                A.Fake<IMapWithParameter<DataExportNotification, UKCompetentAuthority, DataExportNotificationData>>();

            dataImportMapper =
                A.Fake<IMapWithParameter<DataImportNotification, UKCompetentAuthority, DataImportNotificationData>>();

            handler = new GetEADataReportHandler(
                shipmentsRepository,
                shipmentMapper,
                financeReportRepository,
                producerReportRepository,
                foiRepository,
                exportNotificationsRepository,
                dataExportMapper,
                importNotificationsRepository,
                dataImportMapper,
                A.Fake<Domain.IInternalUserRepository>());
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnShipmentReportData_WhenShipmentReportSelected()
        {
            // Arrange
            var shipment = new Shipment();
            var shipmentData = new ShipmentData();

            A.CallTo(() => shipmentsRepository.GetShipmentReportData(
                    A<DateTime>._,
                    A<DateTime>._,
                    UKCompetentAuthority.England))
                .Returns(new List<Shipment> { shipment });

            A.CallTo(() => shipmentMapper.Map(
                    shipment,
                    UKCompetentAuthority.England))
                .Returns(shipmentData);

            var fromDate = DateTime.UtcNow.AddDays(-30);
            var toDate = DateTime.UtcNow;
            var selectedReportList = new List<EAReportList> { EAReportList.ShipmentReport };

            var request = new GetEADataReport(fromDate, toDate, selectedReportList);

            // Act
            var result = await handler.HandleAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.ShipmentReportData);
            Assert.Single(result.ShipmentReportData);
            Assert.Equal(shipmentData, result.ShipmentReportData[0]);

            A.CallTo(() => shipmentsRepository.GetShipmentReportData(
                    request.FromDate,
                    request.ToDate,
                    UKCompetentAuthority.England))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => shipmentMapper.Map(
                    shipment,
                    UKCompetentAuthority.England))
                    .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFinanceReportData_WhenFinanceReportSelected()
        {
            // Arrange
            var financeData = new List<FinanceReportData>
            {
                new FinanceReportData()
            };

            A.CallTo(() => financeReportRepository.GetFinanceReportData(
                    A<DateTime>._,
                    A<DateTime>._,
                    UKCompetentAuthority.England))
                .Returns(financeData);

            var fromDate = DateTime.UtcNow.AddDays(-30);
            var toDate = DateTime.UtcNow;
            var selectedReportList = new List<EAReportList> { EAReportList.FinanceReport };

            var request = new GetEADataReport(fromDate, toDate, selectedReportList);

            // Act
            var result = await handler.HandleAsync(request);

            // Assert
            Assert.NotNull(result.FinanceReportData);
            Assert.Single(result.FinanceReportData);

            A.CallTo(() => financeReportRepository.GetFinanceReportData(
                    request.FromDate,
                    request.ToDate,
                    UKCompetentAuthority.England))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnExportNotificationData_WhenSelected()
        {
            // Arrange
            var exportNotification = new DataExportNotification();
            var exportNotificationData = new DataExportNotificationData();

            A.CallTo(() => exportNotificationsRepository.GetDataExportNotificationData(
                    A<DateTime>._,
                    A<DateTime>._,
                    UKCompetentAuthority.England))
                .Returns(new List<DataExportNotification>
                {
                    exportNotification
                });

            A.CallTo(() => dataExportMapper.Map(
                    exportNotification,
                    UKCompetentAuthority.England))
                .Returns(exportNotificationData);

            var fromDate = DateTime.UtcNow.AddDays(-30);
            var toDate = DateTime.UtcNow;
            var selectedReportList = new List<EAReportList> { EAReportList.DataExportNotification };

            var request = new GetEADataReport(fromDate, toDate, selectedReportList);

            // Act
            var result = await handler.HandleAsync(request);

            // Assert
            Assert.NotNull(result.DataExportNotificationData);
            Assert.Single(result.DataExportNotificationData);

            A.CallTo(() => dataExportMapper.Map(
                    exportNotification,
                    UKCompetentAuthority.England))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnEmptyResult_WhenNoReportsSelected()
        {
            // Arrange
            var fromDate = DateTime.UtcNow.AddDays(-30);
            var toDate = DateTime.UtcNow;
            var selectedReportList = new List<EAReportList>();

            var request = new GetEADataReport(fromDate, toDate, selectedReportList);

            // Act
            var result = await handler.HandleAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.ShipmentReportData);
            Assert.Null(result.FinanceReportData);
            Assert.Null(result.ProducerReportData);
            Assert.Null(result.FreedomOfInformationReportData);
            Assert.Null(result.DataExportNotificationData);
            Assert.Null(result.DataImportNotificationData);
        }

        [Fact]
        public async Task HandleAsync_ShouldCallMultipleRepositories_WhenMultipleReportsSelected()
        {
            // Arrange
            A.CallTo(() => shipmentsRepository.GetShipmentReportData(
                    A<DateTime>._,
                    A<DateTime>._,
                    UKCompetentAuthority.England))
                .Returns(new List<Shipment>());

            A.CallTo(() => financeReportRepository.GetFinanceReportData(
                    A<DateTime>._,
                    A<DateTime>._,
                    UKCompetentAuthority.England))
                .Returns(new List<FinanceReportData>());

            var fromDate = DateTime.UtcNow.AddDays(-30);
            var toDate = DateTime.UtcNow;
            var selectedReportList = new List<EAReportList> { EAReportList.ShipmentReport, EAReportList.FinanceReport };

            var request = new GetEADataReport(fromDate, toDate, selectedReportList);

            // Act
            await handler.HandleAsync(request);

            // Assert
            A.CallTo(() => shipmentsRepository.GetShipmentReportData(
                    request.FromDate,
                    request.ToDate,
                    UKCompetentAuthority.England))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => financeReportRepository.GetFinanceReportData(
                    request.FromDate,
                    request.ToDate,
                    UKCompetentAuthority.England))
                .MustHaveHappenedOnceExactly();
        }
    }
}
