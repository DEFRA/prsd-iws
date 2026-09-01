namespace EA.Iws.Web.Tests.Unit.Controllers.Admin
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.ImportNotificationAssessment;
    using Core.NotificationAssessment;
    using EA.Iws.Web.Areas.Admin.Controllers;
    using EA.Iws.Web.Areas.Admin.ViewModels.Worklist;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;
    using Requests.NotificationAssessment;
    using Xunit;

    public class WorklistControllerTests
    {
        private readonly IMediator mediator;
        private readonly WorklistController controller;

        public WorklistControllerTests()
        {
            mediator = A.Fake<IMediator>();
            controller = new WorklistController(mediator);
        }

        [Fact]
        public async Task Index_Get_ExportTab_LoadsOnlyExportData()
        {
            // Arrange
            var exportResult = new ExportWorklistResult
            {
                Results = new ExportWorklistTableData[0],
                TotalCount = 0,
                PageNumber = 1,
                PageSize = 25
            };

            A.CallTo(() => mediator.SendAsync(A<IRequest<ExportWorklistResult>>._))
                .Returns(exportResult);

            // Act
            var result = await controller.Index(null, null, 1, "export") as ViewResult;
            var model = result.Model as WorklistViewModel;

            // Assert
            Assert.NotNull(model);
            Assert.NotNull(model.ExportResult);
            Assert.Null(model.ImportResult);
            Assert.Equal("export", controller.ViewBag.CurrentTab);
            A.CallTo(() => mediator.SendAsync(A<IRequest<ExportWorklistResult>>._))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => mediator.SendAsync(A<IRequest<ImportWorklistResult>>._))
                .MustNotHaveHappened();
        }

        [Fact]
        public async Task Index_Get_ImportTab_LoadsOnlyImportData()
        {
            // Arrange
            var importResult = new ImportWorklistResult
            {
                Results = new ImportWorklistTableData[0],
                TotalCount = 0,
                PageNumber = 1,
                PageSize = 25
            };

            A.CallTo(() => mediator.SendAsync(A<IRequest<ImportWorklistResult>>._))
                .Returns(importResult);

            // Act
            var result = await controller.Index(null, null, 1, "import") as ViewResult;
            var model = result.Model as WorklistViewModel;

            // Assert
            Assert.NotNull(model);
            Assert.NotNull(model.ImportResult);
            Assert.Null(model.ExportResult);
            Assert.Equal("import", controller.ViewBag.CurrentTab);
            A.CallTo(() => mediator.SendAsync(A<IRequest<ImportWorklistResult>>._))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => mediator.SendAsync(A<IRequest<ExportWorklistResult>>._))
                .MustNotHaveHappened();
        }

        [Fact]
        public async Task Index_Get_ExportTab_AppliesDefaultStatuses_WhenNoFiltersProvided()
        {
            // Arrange
            var exportResult = new ExportWorklistResult
            {
                Results = new ExportWorklistTableData[0],
                TotalCount = 0,
                PageNumber = 1,
                PageSize = 25
            };

            GetExportWorklist capturedRequest = null;
            A.CallTo(() => mediator.SendAsync(A<IRequest<ExportWorklistResult>>._))
                .Invokes((IRequest<ExportWorklistResult> req) => capturedRequest = req as GetExportWorklist)
                .Returns(exportResult);

            // Act
            var result = await controller.Index(null, null, 1, "export") as ViewResult;
            var model = result.Model as WorklistViewModel;

            // Assert
            Assert.NotNull(capturedRequest);
            Assert.NotNull(capturedRequest.Statuses);
            Assert.Equal(2, capturedRequest.Statuses.Length);
            Assert.Contains(NotificationStatus.DecisionRequiredBy, capturedRequest.Statuses);
            Assert.Contains(NotificationStatus.InAssessment, capturedRequest.Statuses);
            Assert.NotNull(model.ExportFilter.SelectedStatuses);
            Assert.Equal(2, model.ExportFilter.SelectedStatuses.Length);
        }

        [Fact]
        public async Task Index_Get_ImportTab_AppliesDefaultStatuses_WhenNoFiltersProvided()
        {
            // Arrange
            var importResult = new ImportWorklistResult
            {
                Results = new ImportWorklistTableData[0],
                TotalCount = 0,
                PageNumber = 1,
                PageSize = 25
            };

            GetImportWorklist capturedRequest = null;
            A.CallTo(() => mediator.SendAsync(A<IRequest<ImportWorklistResult>>._))
                .Invokes((IRequest<ImportWorklistResult> req) => capturedRequest = req as GetImportWorklist)
                .Returns(importResult);

            // Act
            var result = await controller.Index(null, null, 1, "import") as ViewResult;
            var model = result.Model as WorklistViewModel;

            // Assert
            Assert.NotNull(capturedRequest);
            Assert.NotNull(capturedRequest.Statuses);
            Assert.Equal(2, capturedRequest.Statuses.Length);
            Assert.Contains(ImportNotificationStatus.ReadyToAcknowledge, capturedRequest.Statuses);
            Assert.Contains(ImportNotificationStatus.InAssessment, capturedRequest.Statuses);
            Assert.NotNull(model.ImportFilter.SelectedStatuses);
            Assert.Equal(2, model.ImportFilter.SelectedStatuses.Length);
        }

        [Fact]
        public async Task Index_Get_ExportTab_DoesNotApplyDefaultStatuses_WhenFiltersProvided()
        {
            // Arrange
            var exportResult = new ExportWorklistResult
            {
                Results = new ExportWorklistTableData[0],
                TotalCount = 0,
                PageNumber = 1,
                PageSize = 25
            };

            var filter = new ExportWorklistFilterViewModel
            {
                NotificationNumber = "GB 0001 000001"
            };

            GetExportWorklist capturedRequest = null;
            A.CallTo(() => mediator.SendAsync(A<IRequest<ExportWorklistResult>>._))
                .Invokes((IRequest<ExportWorklistResult> req) => capturedRequest = req as GetExportWorklist)
                .Returns(exportResult);

            // Act
            var result = await controller.Index(filter, null, 1, "export") as ViewResult;

            // Assert
            Assert.NotNull(capturedRequest);
            Assert.Equal("GB 0001 000001", capturedRequest.NotificationNumber);
            Assert.Null(capturedRequest.Statuses); // No default statuses applied
        }

        [Fact]
        public async Task Index_Get_ExportTab_PassesFiltersToMediator()
        {
            // Arrange
            var exportResult = new ExportWorklistResult
            {
                Results = new ExportWorklistTableData[0],
                TotalCount = 0,
                PageNumber = 1,
                PageSize = 25
            };

            var filter = new ExportWorklistFilterViewModel
            {
                NotificationNumber = "GB 0001 000001",
                Officer = "John Doe",
                SelectedStatuses = new[] { NotificationStatus.Consented }
            };

            GetExportWorklist capturedRequest = null;
            A.CallTo(() => mediator.SendAsync(A<IRequest<ExportWorklistResult>>._))
                .Invokes((IRequest<ExportWorklistResult> req) => capturedRequest = req as GetExportWorklist)
                .Returns(exportResult);

            // Act
            await controller.Index(filter, null, 2, "export");

            // Assert
            Assert.NotNull(capturedRequest);
            Assert.Equal("GB 0001 000001", capturedRequest.NotificationNumber);
            Assert.Equal("John Doe", capturedRequest.Officer);
            Assert.Single(capturedRequest.Statuses);
            Assert.Equal(NotificationStatus.Consented, capturedRequest.Statuses[0]);
            Assert.Equal(2, capturedRequest.PageNumber);
        }

        [Fact]
        public async Task Index_Get_ImportTab_PassesFiltersToMediator()
        {
            // Arrange
            var importResult = new ImportWorklistResult
            {
                Results = new ImportWorklistTableData[0],
                TotalCount = 0,
                PageNumber = 1,
                PageSize = 25
            };

            var filter = new ImportWorklistFilterViewModel
            {
                NotificationNumber = "GB 0001 000002",
                Officer = "Jane Smith",
                SelectedStatuses = new[] { ImportNotificationStatus.Consented }
            };

            GetImportWorklist capturedRequest = null;
            A.CallTo(() => mediator.SendAsync(A<IRequest<ImportWorklistResult>>._))
                .Invokes((IRequest<ImportWorklistResult> req) => capturedRequest = req as GetImportWorklist)
                .Returns(importResult);

            // Act
            await controller.Index(null, filter, 3, "import");

            // Assert
            Assert.NotNull(capturedRequest);
            Assert.Equal("GB 0001 000002", capturedRequest.NotificationNumber);
            Assert.Equal("Jane Smith", capturedRequest.Officer);
            Assert.Single(capturedRequest.Statuses);
            Assert.Equal(ImportNotificationStatus.Consented, capturedRequest.Statuses[0]);
            Assert.Equal(3, capturedRequest.PageNumber);
        }

        [Fact]
        public async Task Index_Get_SetsExportStatusesInModel()
        {
            // Arrange
            var exportResult = new ExportWorklistResult
            {
                Results = new ExportWorklistTableData[0],
                TotalCount = 0,
                PageNumber = 1,
                PageSize = 25
            };

            A.CallTo(() => mediator.SendAsync(A<IRequest<ExportWorklistResult>>._))
                .Returns(exportResult);

            // Act
            var result = await controller.Index(null, null, 1, "export") as ViewResult;
            var model = result.Model as WorklistViewModel;

            // Assert
            Assert.NotNull(model.ExportStatuses);
            Assert.Equal(8, model.ExportStatuses.Length);
            Assert.Contains(NotificationStatus.NotificationReceived, model.ExportStatuses);
            Assert.Contains(NotificationStatus.InAssessment, model.ExportStatuses);
            Assert.Contains(NotificationStatus.DecisionRequiredBy, model.ExportStatuses);
        }

        [Fact]
        public async Task Index_Get_SetsImportStatusesInModel()
        {
            // Arrange
            var importResult = new ImportWorklistResult
            {
                Results = new ImportWorklistTableData[0],
                TotalCount = 0,
                PageNumber = 1,
                PageSize = 25
            };

            A.CallTo(() => mediator.SendAsync(A<IRequest<ImportWorklistResult>>._))
                .Returns(importResult);

            // Act
            var result = await controller.Index(null, null, 1, "import") as ViewResult;
            var model = result.Model as WorklistViewModel;

            // Assert
            Assert.NotNull(model.ImportStatuses);
            Assert.Equal(9, model.ImportStatuses.Length);
            Assert.Contains(ImportNotificationStatus.ReadyToAcknowledge, model.ImportStatuses);
            Assert.Contains(ImportNotificationStatus.InAssessment, model.ImportStatuses);
            Assert.Contains(ImportNotificationStatus.Consented, model.ImportStatuses);
        }

        [Fact]
        public void Index_Post_ExportTab_RedirectsWithFilters()
        {
            // Arrange
            var model = new WorklistViewModel
            {
                ExportFilter = new ExportWorklistFilterViewModel
                {
                    NotificationNumber = "GB 0001 000001",
                    Officer = "John Doe",
                    SelectedStatuses = new[] { NotificationStatus.Consented }
                }
            };

            // Act
            var result = controller.Index(model, "export") as RedirectToRouteResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("export", result.RouteValues["tab"]);
            Assert.Equal(1, result.RouteValues["page"]);
            Assert.Equal("GB 0001 000001", result.RouteValues["exportFilter.NotificationNumber"]);
            Assert.Equal("John Doe", result.RouteValues["exportFilter.Officer"]);
            Assert.Equal((int)NotificationStatus.Consented, result.RouteValues["exportFilter.SelectedStatuses[0]"]);
        }

        [Fact]
        public void Index_Post_ImportTab_RedirectsWithFilters()
        {
            // Arrange
            var model = new WorklistViewModel
            {
                ImportFilter = new ImportWorklistFilterViewModel
                {
                    NotificationNumber = "GB 0001 000002",
                    Officer = "Jane Smith",
                    SelectedStatuses = new[] { ImportNotificationStatus.Consented }
                }
            };

            // Act
            var result = controller.Index(model, "import") as RedirectToRouteResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("import", result.RouteValues["tab"]);
            Assert.Equal(1, result.RouteValues["page"]);
            Assert.Equal("GB 0001 000002", result.RouteValues["importFilter.NotificationNumber"]);
            Assert.Equal("Jane Smith", result.RouteValues["importFilter.Officer"]);
            Assert.Equal((int)ImportNotificationStatus.Consented, result.RouteValues["importFilter.SelectedStatuses[0]"]);
        }

        [Fact]
        public void Index_Post_ExportTab_HandlesMultipleStatuses()
        {
            // Arrange
            var model = new WorklistViewModel
            {
                ExportFilter = new ExportWorklistFilterViewModel
                {
                    SelectedStatuses = new[]
                    {
                        NotificationStatus.InAssessment,
                        NotificationStatus.DecisionRequiredBy,
                        NotificationStatus.Consented
                    }
                }
            };

            // Act
            var result = controller.Index(model, "export") as RedirectToRouteResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)NotificationStatus.InAssessment, result.RouteValues["exportFilter.SelectedStatuses[0]"]);
            Assert.Equal((int)NotificationStatus.DecisionRequiredBy, result.RouteValues["exportFilter.SelectedStatuses[1]"]);
            Assert.Equal((int)NotificationStatus.Consented, result.RouteValues["exportFilter.SelectedStatuses[2]"]);
        }

        [Fact]
        public void Index_Post_ImportTab_HandlesNullFilters()
        {
            // Arrange
            var model = new WorklistViewModel
            {
                ImportFilter = new ImportWorklistFilterViewModel()
            };

            // Act
            var result = controller.Index(model, "import") as RedirectToRouteResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("import", result.RouteValues["tab"]);
            Assert.Equal(1, result.RouteValues["page"]);
            // Should only have tab and page in route values
            Assert.Equal(3, result.RouteValues.Count);
        }
    }
}