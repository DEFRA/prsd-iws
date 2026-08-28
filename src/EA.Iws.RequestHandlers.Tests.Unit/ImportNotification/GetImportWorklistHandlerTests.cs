namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ImportNotificationAssessment;
    using Core.Notification;
    using Domain;
    using Domain.ImportNotification;
    using Domain.ImportNotificationAssessment;
    using Domain.ImportNotificationAssessment.Decision;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using RequestHandlers.ImportNotificationAssessment;
    using Requests.ImportNotificationAssessment;
    using Xunit;

    public class GetImportWorklistHandlerTests
    {
        private readonly IImportWorklistRepository worklistRepository;
        private readonly IImportNotificationAssessmentRepository importNotificationAssessmentRepository;
        private readonly IImportNotificationRepository importNotificationRepository;
        private readonly IWorkingDayCalculator workingDayCalculator;
        private readonly IInternalUserRepository internalUserRepository;
        private readonly IUserContext userContext;
        private readonly DecisionRequiredBy decisionRequiredByCalculator;
        private readonly DaysRemainingCalculator daysRemainingCalculator;
        private readonly GetImportWorklistHandler handler;

        private readonly Guid notificationId = Guid.NewGuid();
        private readonly Guid userId = Guid.NewGuid();

        public GetImportWorklistHandlerTests()
        {
            worklistRepository = A.Fake<IImportWorklistRepository>();
            importNotificationAssessmentRepository = A.Fake<IImportNotificationAssessmentRepository>();
            importNotificationRepository = A.Fake<IImportNotificationRepository>();
            workingDayCalculator = A.Fake<IWorkingDayCalculator>();
            internalUserRepository = A.Fake<IInternalUserRepository>();
            userContext = A.Fake<IUserContext>();
            decisionRequiredByCalculator = A.Fake<DecisionRequiredBy>();
            daysRemainingCalculator = A.Fake<DaysRemainingCalculator>();

            A.CallTo(() => userContext.UserId).Returns(userId);

            handler = new GetImportWorklistHandler(
                worklistRepository,
                importNotificationAssessmentRepository,
                importNotificationRepository,
                workingDayCalculator,
                internalUserRepository,
                userContext,
                decisionRequiredByCalculator,
                daysRemainingCalculator);
        }

        [Fact]
        public async Task HandleAsync_ReturnsWorklistResult_WithCorrectPaging()
        {
            // Arrange
            var internalUser = A.Fake<InternalUser>();
            A.CallTo(() => internalUser.CompetentAuthority).Returns(UKCompetentAuthority.England);
            A.CallTo(() => internalUserRepository.GetByUserId(userId)).Returns(internalUser);

            var worklistSummary = CreateImportWorklistSummary();
            var queryResult = new ImportWorklistQueryResult
            {
                TotalCount = 50,
                PagedRows = new[] { worklistSummary }
            };

            A.CallTo(() => worklistRepository.GetByCompetentAuthority(
                UKCompetentAuthority.England,
                null,
                null,
                null,
                1,
                25)).Returns(queryResult);

            SetupMocks(worklistSummary);

            var request = new GetImportWorklist
            {
                NotificationNumber = null,
                Officer = null,
                Statuses = null,
                PageNumber = 1
            };

            // Act
            var result = await handler.HandleAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(50, result.TotalCount);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(25, result.PageSize);
            Assert.Single(result.Results);
        }

        [Fact]
        public async Task HandleAsync_FiltersWithDefaultStatuses()
        {
            // Arrange
            var internalUser = A.Fake<InternalUser>();
            A.CallTo(() => internalUser.CompetentAuthority).Returns(UKCompetentAuthority.England);
            A.CallTo(() => internalUserRepository.GetByUserId(userId)).Returns(internalUser);

            var worklistSummary = CreateImportWorklistSummary();
            var queryResult = new ImportWorklistQueryResult
            {
                TotalCount = 1,
                PagedRows = new[] { worklistSummary }
            };

            var defaultStatuses = new[] { ImportNotificationStatus.ReadyToAcknowledge, ImportNotificationStatus.InAssessment };

            A.CallTo(() => worklistRepository.GetByCompetentAuthority(
                UKCompetentAuthority.England,
                null,
                null,
                defaultStatuses,
                1,
                25)).Returns(queryResult);

            SetupMocks(worklistSummary);

            var request = new GetImportWorklist
            {
                NotificationNumber = null,
                Officer = null,
                Statuses = defaultStatuses,
                PageNumber = 1
            };

            // Act
            var result = await handler.HandleAsync(request);

            // Assert
            Assert.Single(result.Results);
            A.CallTo(() => worklistRepository.GetByCompetentAuthority(
                UKCompetentAuthority.England,
                null,
                null,
                defaultStatuses,
                1,
                25)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HandleAsync_CalculatesWorkingDaysInAssessment()
        {
            // Arrange
            var internalUser = A.Fake<InternalUser>();
            A.CallTo(() => internalUser.CompetentAuthority).Returns(UKCompetentAuthority.England);
            A.CallTo(() => internalUserRepository.GetByUserId(userId)).Returns(internalUser);

            var datePickedUp = new DateTime(2024, 1, 1);
            var worklistSummary = CreateImportWorklistSummary(datePickedUp);
            var queryResult = new ImportWorklistQueryResult
            {
                TotalCount = 1,
                PagedRows = new[] { worklistSummary }
            };

            A.CallTo(() => worklistRepository.GetByCompetentAuthority(
                A<UKCompetentAuthority>._,
                A<string>._,
                A<string>._,
                A<ImportNotificationStatus[]>._,
                A<int>._,
                A<int>._)).Returns(queryResult);

            var notification = A.Fake<ImportNotification>();
            A.CallTo(() => notification.CompetentAuthority).Returns(UKCompetentAuthority.England);

            A.CallTo(() => importNotificationRepository.Get(notificationId))
                .Returns(notification);
            A.CallTo(() => decisionRequiredByCalculator.GetDecisionRequiredByDate(A<ImportNotificationAssessment>._))
                .Returns((DateTime?)null);
            A.CallTo(() => workingDayCalculator.GetWorkingDays(
                datePickedUp,
                A<DateTime>._,
                false,
                UKCompetentAuthority.England))
                .Returns(12);

            var request = new GetImportWorklist
            {
                PageNumber = 1
            };

            // Act
            var result = await handler.HandleAsync(request);

            // Assert
            Assert.Equal(12, result.Results.First().WorkingDaysInAssessment);
        }

        [Fact]
        public async Task HandleAsync_CalculatesDaysRemaining_WhenDecisionRequiredByExists()
        {
            // Arrange
            var internalUser = A.Fake<InternalUser>();
            A.CallTo(() => internalUser.CompetentAuthority).Returns(UKCompetentAuthority.England);
            A.CallTo(() => internalUserRepository.GetByUserId(userId)).Returns(internalUser);

            var decisionRequiredDate = new DateTime(2024, 12, 31);
            var worklistSummary = CreateImportWorklistSummary(decisionRequiredDate: decisionRequiredDate);
            var queryResult = new ImportWorklistQueryResult
            {
                TotalCount = 1,
                PagedRows = new[] { worklistSummary }
            };

            A.CallTo(() => worklistRepository.GetByCompetentAuthority(
                A<UKCompetentAuthority>._,
                A<string>._,
                A<string>._,
                A<ImportNotificationStatus[]>._,
                A<int>._,
                A<int>._)).Returns(queryResult);

            var notification = A.Fake<ImportNotification>();
            A.CallTo(() => notification.CompetentAuthority).Returns(UKCompetentAuthority.England);

            A.CallTo(() => importNotificationRepository.Get(notificationId))
                .Returns(notification);
            A.CallTo(() => daysRemainingCalculator.Calculate(decisionRequiredDate))
                .Returns("15");

            var request = new GetImportWorklist
            {
                PageNumber = 1
            };

            // Act
            var result = await handler.HandleAsync(request);

            // Assert
            Assert.Equal("15", result.Results.First().DaysRemaining);
            A.CallTo(() => daysRemainingCalculator.Calculate(decisionRequiredDate))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HandleAsync_CalculatesDecisionRequiredDate_WhenNotStored()
        {
            // Arrange
            var internalUser = A.Fake<InternalUser>();
            A.CallTo(() => internalUser.CompetentAuthority).Returns(UKCompetentAuthority.England);
            A.CallTo(() => internalUserRepository.GetByUserId(userId)).Returns(internalUser);

            var acknowledgedDate = new DateTime(2024, 1, 15);
            var worklistSummary = CreateImportWorklistSummary(acknowledgedDate: acknowledgedDate, decisionRequiredDate: null);
            var queryResult = new ImportWorklistQueryResult
            {
                TotalCount = 1,
                PagedRows = new[] { worklistSummary }
            };

            A.CallTo(() => worklistRepository.GetByCompetentAuthority(
                A<UKCompetentAuthority>._,
                A<string>._,
                A<string>._,
                A<ImportNotificationStatus[]>._,
                A<int>._,
                A<int>._)).Returns(queryResult);

            var notification = A.Fake<ImportNotification>();
            var assessment = A.Fake<ImportNotificationAssessment>();
            var calculatedDecisionDate = new DateTime(2024, 3, 15);

            A.CallTo(() => notification.CompetentAuthority).Returns(UKCompetentAuthority.England);
            A.CallTo(() => importNotificationRepository.Get(notificationId))
                .Returns(notification);
            A.CallTo(() => importNotificationAssessmentRepository.GetByNotification(notificationId))
                .Returns(assessment);
            A.CallTo(() => decisionRequiredByCalculator.GetDecisionRequiredByDate(assessment))
                .Returns(calculatedDecisionDate);

            var request = new GetImportWorklist
            {
                PageNumber = 1
            };

            // Act
            var result = await handler.HandleAsync(request);

            // Assert
            Assert.Equal(calculatedDecisionDate, result.Results.First().DecisionRequiredDate);
            A.CallTo(() => decisionRequiredByCalculator.GetDecisionRequiredByDate(assessment))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HandleAsync_HandlesInvalidPageNumber()
        {
            // Arrange
            var internalUser = A.Fake<InternalUser>();
            A.CallTo(() => internalUser.CompetentAuthority).Returns(UKCompetentAuthority.England);
            A.CallTo(() => internalUserRepository.GetByUserId(userId)).Returns(internalUser);

            var worklistSummary = CreateImportWorklistSummary();
            var queryResult = new ImportWorklistQueryResult
            {
                TotalCount = 1,
                PagedRows = new[] { worklistSummary }
            };

            A.CallTo(() => worklistRepository.GetByCompetentAuthority(
                UKCompetentAuthority.England,
                null,
                null,
                null,
                1,  // Should default to 1
                25)).Returns(queryResult);

            SetupMocks(worklistSummary);

            var request = new GetImportWorklist
            {
                PageNumber = 0  // Invalid page number
            };

            // Act
            var result = await handler.HandleAsync(request);

            // Assert
            Assert.Equal(1, result.PageNumber);
        }

        [Fact]
        public async Task HandleAsync_IncludesLastCommentData()
        {
            // Arrange
            var internalUser = A.Fake<InternalUser>();
            A.CallTo(() => internalUser.CompetentAuthority).Returns(UKCompetentAuthority.England);
            A.CallTo(() => internalUserRepository.GetByUserId(userId)).Returns(internalUser);

            var lastCommentDate = new DateTimeOffset(2024, 6, 1, 10, 30, 0, TimeSpan.Zero);
            var lastComment = "Import test comment";
            var worklistSummary = CreateImportWorklistSummary(lastCommentDate: lastCommentDate, lastComment: lastComment);
            var queryResult = new ImportWorklistQueryResult
            {
                TotalCount = 1,
                PagedRows = new[] { worklistSummary }
            };

            A.CallTo(() => worklistRepository.GetByCompetentAuthority(
                A<UKCompetentAuthority>._,
                A<string>._,
                A<string>._,
                A<ImportNotificationStatus[]>._,
                A<int>._,
                A<int>._)).Returns(queryResult);

            SetupMocks(worklistSummary);

            var request = new GetImportWorklist
            {
                PageNumber = 1
            };

            // Act
            var result = await handler.HandleAsync(request);

            // Assert
            Assert.Equal(lastCommentDate, result.Results.First().LastCommentDate);
            Assert.Equal(lastComment, result.Results.First().LastComment);
        }

        private ImportWorklistSummary CreateImportWorklistSummary(
            DateTime? datePickedUp = null,
            DateTime? acknowledgedDate = null,
            DateTime? decisionRequiredDate = null,
            DateTimeOffset? lastCommentDate = null,
            string lastComment = null)
        {
            return ImportWorklistSummary.Load(
                notificationId,
                "GB 0001 000002",
                "Test Exporter",
                "Test Officer",
                datePickedUp,
                new DateTime(2024, 1, 20),
                acknowledgedDate ?? new DateTime(2024, 1, 25),
                null,
                decisionRequiredDate,
                ImportNotificationStatus.InAssessment,
                lastCommentDate ?? new DateTimeOffset(2024, 1, 12, 0, 0, 0, TimeSpan.Zero),
                1,
                "Approved",
                "Test action",
                lastComment ?? "Test comment");
        }

        private void SetupMocks(ImportWorklistSummary summary)
        {
            var notification = A.Fake<ImportNotification>();
            A.CallTo(() => notification.CompetentAuthority).Returns(UKCompetentAuthority.England);

            A.CallTo(() => importNotificationRepository.Get(summary.NotificationId))
                .Returns(notification);
            A.CallTo(() => decisionRequiredByCalculator.GetDecisionRequiredByDate(A<ImportNotificationAssessment>._))
                .Returns((DateTime?)null);
        }
    }
}