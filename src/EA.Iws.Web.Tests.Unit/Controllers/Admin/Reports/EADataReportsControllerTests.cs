namespace EA.Iws.Web.Tests.Unit.Controllers.Admin.Reports
{
    using EA.Iws.Core.Admin.Reports;
    using EA.Iws.Core.Reports;
    using EA.Iws.Requests.Admin.Reports;
    using EA.Iws.Web.Areas.Reports.Controllers;
    using EA.Iws.Web.Areas.Reports.ViewModels.EADataReports;
    using EA.Iws.Web.Infrastructure.Validation;
    using EA.Iws.Web.ViewModels.Shared;
    using EA.Prsd.Core.Mediator;
    using FakeItEasy;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Xunit;

    public class EADataReportsControllerTests
    {
        private readonly IMediator mediator;
        private readonly EADataReportsController controller;

        public EADataReportsControllerTests()
        {
            mediator = A.Fake<IMediator>();

            controller = new EADataReportsController(mediator);
        }

        [Fact]
        public void GetIndex_ReturnsViewModel_Valid()
        {
            var result = controller.Index() as ViewResult;

            var model = result.Model as IndexViewModel;
            Assert.NotNull(model);
        }

        [Fact]
        public void GetIndex_ReturnsViewModel_ReportListCount()
        {
            var result = controller.Index() as ViewResult;

            var model = result.Model as IndexViewModel;
            Assert.Equal(6, model.EAReportLists.Count);
        }

        [Fact]
        public async Task PostIndexWithoutFromDate_NotValid()
        {
            IndexViewModel model = new IndexViewModel();
            controller.ModelState.AddModelError("From", "Please enter a valid number in the 'Day' field");
            controller.ModelState.AddModelError("From", "Please enter a valid number in the 'Month' field");
            controller.ModelState.AddModelError("From", "Please enter a valid number in the 'Year' field");

            var result = await controller.Index(model) as ViewResult;
            Assert.False(controller.ModelState.IsValid);
        }

        [Fact]
        public async Task PostIndexWithoutselectAtLeastOneReport_NotValid()
        {
            IndexViewModel model = new IndexViewModel();
            controller.ModelState.AddModelError("SelectedList", "Please select at least one report");

            var result = await controller.Index(model) as ViewResult;
            Assert.False(controller.ModelState.IsValid);
        }

        [Fact]
        public async Task Index_WhenModelStateIsInvalid_ReturnsViewWithSameModel()
        {
            // Arrange
            var model = new IndexViewModel();

            controller.ModelState.AddModelError("From", "Required");

            // Act
            var result = await controller.Index(model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal(model, viewResult.Model);

            A.CallTo(() => mediator.SendAsync(A<GetEADataReport>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task Index_WhenModelIsValid_ReturnsExcelFile()
        {
            // Arrange
            var fromDate = new DateTime(2018, 01, 01);
            var toDate = new DateTime(2026, 01, 01);

            var selectedReports = new List<EAReportList>
            {
                EAReportList.ShipmentReport,
                EAReportList.FinanceReport
            };

            var model = new IndexViewModel
            {
                From = new RequiredDateInputViewModel() { Day = 1, Month = 1, Year = 2018 },
                To = new RequiredDateInputViewModel() { Day = 1, Month = 1, Year = 2026 },
                EAReportLists = new List<KeyValuePairViewModel<EAReportList, bool>>
                {
                    new KeyValuePairViewModel<EAReportList, bool>(EAReportList.ShipmentReport, true),
                    new KeyValuePairViewModel<EAReportList, bool>(EAReportList.FinanceReport, true),
                    new KeyValuePairViewModel<EAReportList, bool>(EAReportList.ProducerReport, false),
                    new KeyValuePairViewModel<EAReportList, bool>(EAReportList.FOIReport, false),
                    new KeyValuePairViewModel<EAReportList, bool>(EAReportList.DataExportNotification, false),
                    new KeyValuePairViewModel<EAReportList, bool>(EAReportList.DataImportNotification, false)
                }
            };

            var reportResult = new EADataReportsData
            {
                ShipmentReportData = new ShipmentData[1],
                FinanceReportData = new FinanceReportData[1]
            };

            A.CallTo(() => mediator.SendAsync(A<GetEADataReport>._)).Returns(reportResult);

            // Act
            var result = await controller.Index(model);

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(result);

            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileResult.ContentType);
            Assert.Equal("EADataReport-20180101-20260101.xlsx", fileResult.FileDownloadName);

            Assert.NotNull(fileResult.FileContents);
            Assert.NotEmpty(fileResult.FileContents);

            A.CallTo(() => mediator.SendAsync(A<GetEADataReport>.That.Matches(q => q.FromDate == fromDate && q.ToDate == toDate)))
                                   .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Index_WhenAllReportsSelected_ReturnsExcelFile()
        {
            // Arrange
            var model = new IndexViewModel
            {
                From = new RequiredDateInputViewModel() { Day = 1, Month = 1, Year = 2018 },
                To = new RequiredDateInputViewModel() { Day = 1, Month = 1, Year = 2026 },
                EAReportLists = new List<KeyValuePairViewModel<EAReportList, bool>>
                {
                    new KeyValuePairViewModel<EAReportList, bool>(EAReportList.ShipmentReport, true),
                    new KeyValuePairViewModel<EAReportList, bool>(EAReportList.FinanceReport, true),
                    new KeyValuePairViewModel<EAReportList, bool>(EAReportList.ProducerReport, true),
                    new KeyValuePairViewModel<EAReportList, bool>(EAReportList.FOIReport, true),
                    new KeyValuePairViewModel<EAReportList, bool>(EAReportList.DataExportNotification, true),
                    new KeyValuePairViewModel<EAReportList, bool>(EAReportList.DataImportNotification, true)
                }
            };

            var reportResult = new EADataReportsData()
            {
                FinanceReportData = new FinanceReportData[1],
                DataExportNotificationData = new DataExportNotificationData[1],
                DataImportNotificationData = new DataImportNotificationData[1],
                FreedomOfInformationReportData = new FreedomOfInformationData[1],
                ProducerReportData = new ProducerData[1],
                ShipmentReportData = new ShipmentData[1]
            };

            A.CallTo(() => mediator.SendAsync(A<GetEADataReport>._)).Returns(reportResult);

            // Act
            var result = await controller.Index(model);

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(result);

            Assert.NotNull(fileResult.FileContents);
            Assert.NotEmpty(fileResult.FileContents);

            A.CallTo(() => mediator.SendAsync(A<GetEADataReport>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Index_WhenShipmentReportSelected_CallsMediatorWithCorrectParameters()
        {
            // Arrange
            var model = new IndexViewModel
            {
                From = new RequiredDateInputViewModel() { Day = 1, Month = 1, Year = 2018 },
                To = new RequiredDateInputViewModel() { Day = 1, Month = 1, Year = 2026 },
                EAReportLists = new List<KeyValuePairViewModel<EAReportList, bool>>
                {
                    new KeyValuePairViewModel<EAReportList, bool>(EAReportList.ShipmentReport, true),
                    new KeyValuePairViewModel<EAReportList, bool>(EAReportList.FinanceReport, false),
                    new KeyValuePairViewModel<EAReportList, bool>(EAReportList.ProducerReport, false),
                    new KeyValuePairViewModel<EAReportList, bool>(EAReportList.FOIReport, false),
                    new KeyValuePairViewModel<EAReportList, bool>(EAReportList.DataExportNotification, false),
                    new KeyValuePairViewModel<EAReportList, bool>(EAReportList.DataImportNotification, false)
                }
            };

            var reportResult = new EADataReportsData
            {
                ShipmentReportData = new ShipmentData[1]
            };

            A.CallTo(() => mediator.SendAsync(A<GetEADataReport>._)).Returns(reportResult);

            // Act
            await controller.Index(model);

            // Assert
            A.CallTo(() => mediator.SendAsync(A<GetEADataReport>.That.Matches(q => q.FromDate == new DateTime(2018, 01, 01) &&
                                                                                   q.ToDate == new DateTime(2026, 01, 01) &&
                                                                                   q.SelectedReportList.Contains(EAReportList.ShipmentReport))))
                                  .MustHaveHappenedOnceExactly();
        }
    }
}
