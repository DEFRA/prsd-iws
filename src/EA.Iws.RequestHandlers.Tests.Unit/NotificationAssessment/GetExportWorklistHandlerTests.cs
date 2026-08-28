namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationAssessment
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.NotificationAssessment;
    using Domain;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using FakeItEasy;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using RequestHandlers.NotificationAssessment;
    using Requests.NotificationAssessment;
    using Xunit;

    public class GetExportWorklistHandlerTests
    {
        private readonly IExportWorklistRepository worklistRepository;
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly DecisionRequiredBy decisionRequiredByCalculator;
        private readonly DaysRemainingCalculator daysRemainingCalculator;
        private readonly IWorkingDayCalculator workingDayCalculator;
        private readonly IInternalUserRepository internalUserRepository;
        private readonly IUserContext userContext;
        private readonly GetExportWorklistHandler handler;

        private readonly Guid notificationId = Guid.NewGuid();
        private readonly Guid userId = Guid.NewGuid();

        public GetExportWorklistHandlerTests()
        {
            worklistRepository = A.Fake<IExportWorklistRepository>();
            notificationAssessmentRepository = A.Fake<INotificationAssessmentRepository>();
            notificationApplicationRepository = A.Fake<INotificationApplicationRepository>();
            decisionRequiredByCalculator = A.Fake<DecisionRequiredBy>();
            daysRemainingCalculator = A.Fake<DaysRemainingCalculator>();
            workingDayCalculator = A.Fake<IWorkingDayCalculator>();
            internalUserRepository = A.Fake<IInternalUserRepository>();
            userContext = A.Fake<IUserContext>();

            A.CallTo(() => userContext.UserId).Returns(userId);

            handler = new GetExportWorklistHandler(
                worklistRepository,
                notificationAssessmentRepository,
                notificationApplicationRepository,
                decisionRequiredByCalculator,
                daysRemainingCalculator,
                workingDayCalculator,
                internalUserRepository,
                userContext);
        }

        [Fact]
        public async Task HandleAsync_ReturnsWorklistResult_WithCorrectPaging()
        {
            // Arrange
            var internalUser = A.Fake<InternalUser>();
            A.CallTo(() => internalUser.CompetentAuthority).Returns(UKCompetentAuthority.England);
            A.CallTo(() => internalUserRepository.GetByUserId(userId)).Returns(internalUser);

            var worklistSummary = CreateExportWorklistSummary();
            var queryResult = new ExportWorklistQueryResult
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

            var request = new GetExportWorklist
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
        public async Task HandleAsync_FiltersWithNotificationNumber()
        {
            // Arrange
            var internalUser = A.Fake<InternalUser>();
            A.CallTo(() => internalUser.CompetentAuthority).Returns(UKCompetentAuthority.England);
            A.CallTo(() => internalUserRepository.GetByUserId(userId)).Returns(internalUser);

            var worklistSummary = CreateExportWorklistSummary();
            var queryResult = new ExportWorklistQueryResult
            {
                TotalCount = 1,
                PagedRows = new[] { worklistSummary }
            };

            A.CallTo(() => worklistRepository.GetByCompetentAuthority(
                UKCompetentAuthority.England,
                "GB 0001 000001",
                null,
                null,
                1,
                25)).Returns(queryResult);

            SetupMocks(worklistSummary);

            var request = new GetExportWorklist
            {
                NotificationNumber = "GB 0001 000001",
                Officer = null,
                Statuses = null,
                PageNumber = 1
            };

            // Act
            var result = await handler.HandleAsync(request);

            // Assert
            Assert.Single(result.Results);
            A.CallTo(() => worklistRepository.GetByCompetentAuthority(
                UKCompetentAuthority.England,
                "GB 0001 000001",
                null,
                null,
                1,
                25)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HandleAsync_FiltersWithOfficer()
        {
            // Arrange
            var internalUser = A.Fake<InternalUser>();
            A.CallTo(() => internalUser.CompetentAuthority).Returns(UKCompetentAuthority.England);
            A.CallTo(() => internalUserRepository.GetByUserId(userId)).Returns(internalUser);

            var worklistSummary = CreateExportWorklistSummary();
            var queryResult = new ExportWorklistQueryResult
            {
                TotalCount = 1,
                PagedRows = new[] { worklistSummary }
            };

            A.CallTo(() => worklistRepository.GetByCompetentAuthority(
                UKCompetentAuthority.England,
                null,
                "John Doe",
                null,
                1,
                25)).Returns(queryResult);

            SetupMocks(worklistSummary);

            var request = new GetExportWorklist
            {
                NotificationNumber = null,
                Officer = "John Doe",
                Statuses = null,
                PageNumber = 1
            };

            // Act
            var result = await handler.HandleAsync(request);

            // Assert
            Assert.Single(result.Results);
            A.CallTo(() => worklistRepository.GetByCompetentAuthority(
                UKCompetentAuthority.England,
                null,
                "John Doe",
                null,
                1,
                25)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HandleAsync_FiltersWithStatuses()
        {
            // Arrange
            var internalUser = A.Fake<InternalUser>();
            A.CallTo(() => internalUser.CompetentAuthority).Returns(UKCompetentAuthority.England);
            A.CallTo(() => internalUserRepository.GetByUserId(userId)).Returns(internalUser);

            var worklistSummary = CreateExportWorklistSummary();
            var queryResult = new ExportWorklistQueryResult
            {
                TotalCount = 1,
                PagedRows = new[] { worklistSummary }
            };

            var statuses = new[] { NotificationStatus.DecisionRequiredBy, NotificationStatus.InAssessment };

            A.CallTo(() => worklistRepository.GetByCompetentAuthority(
                UKCompetentAuthority.England,
                null,
                null,
                statuses,
                1,
                25)).Returns(queryResult);

            SetupMocks(worklistSummary);

            var request = new GetExportWorklist
            {
                NotificationNumber = null,
                Officer = null,
                Statuses = statuses,
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
                statuses,
                1,
                25)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HandleAsync_CalculatesDaysRemaining_WhenDecisionRequiredByExists()
        {
            // Arrange
            var internalUser = A.Fake<InternalUser>();
            A.CallTo(() => internalUser.CompetentAuthority).Returns(UKCompetentAuthority.England);
            A.CallTo(() => internalUserRepository.GetByUserId(userId)).Returns(internalUser);

            var worklistSummary = CreateExportWorklistSummary();
            var queryResult = new ExportWorklistQueryResult
            {
                TotalCount = 1,
                PagedRows = new[] { worklistSummary }
            };

            A.CallTo(() => worklistRepository.GetByCompetentAuthority(
                A<UKCompetentAuthority>._,
                A<string>._,
                A<string>._,
                A<NotificationStatus[]>._,
                A<int>._,
                A<int>._)).Returns(queryResult);

            var assessment = A.Fake<NotificationAssessment>();
            var notification = A.Fake<NotificationApplication>();
            var decisionRequiredDate = new DateTime(2024, 12, 31);

            A.CallTo(() => notificationAssessmentRepository.GetByNotificationId(notificationId))
                .Returns(assessment);
            A.CallTo(() => notificationApplicationRepository.GetById(notificationId))
                .Returns(notification);
            A.CallTo(() => decisionRequiredByCalculator.GetDecisionRequiredByDate(notification, assessment))
                .Returns(decisionRequiredDate);
            A.CallTo(() => daysRemainingCalculator.Calculate(decisionRequiredDate))
                .Returns("15");

            var request = new GetExportWorklist
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
        public async Task HandleAsync_CalculatesWorkingDaysInAssessment_WhenDatePickedUpExists()
        {
            // Arrange
            var internalUser = A.Fake<InternalUser>();
            A.CallTo(() => internalUser.CompetentAuthority).Returns(UKCompetentAuthority.England);
            A.CallTo(() => internalUserRepository.GetByUserId(userId)).Returns(internalUser);

            var datePickedUp = new DateTime(2024, 1, 1);
            var worklistSummary = CreateExportWorklistSummary(datePickedUp);
            var queryResult = new ExportWorklistQueryResult
            {
                TotalCount = 1,
                PagedRows = new[] { worklistSummary }
            };

            A.CallTo(() => worklistRepository.GetByCompetentAuthority(
                A<UKCompetentAuthority>._,
                A<string>._,
                A<string>._,
                A<NotificationStatus[]>._,
                A<int>._,
                A<int>._)).Returns(queryResult);

            var assessment = A.Fake<NotificationAssessment>();
            var notification = A.Fake<NotificationApplication>();
            A.CallTo(() => notification.CompetentAuthority).Returns(UKCompetentAuthority.England);

            A.CallTo(() => notificationAssessmentRepository.GetByNotificationId(notificationId))
                .Returns(assessment);
            A.CallTo(() => notificationApplicationRepository.GetById(notificationId))
                .Returns(notification);
            A.CallTo(() => decisionRequiredByCalculator.GetDecisionRequiredByDate(notification, assessment))
                .Returns((DateTime?)null);
            A.CallTo(() => workingDayCalculator.GetWorkingDays(
                datePickedUp,
                A<DateTime>._,
                false,
                UKCompetentAuthority.England))
                .Returns(10);

            var request = new GetExportWorklist
            {
                PageNumber = 1
            };

            // Act
            var result = await handler.HandleAsync(request);

            // Assert
            Assert.Equal(10, result.Results.First().WorkingDaysInAssessment);
            A.CallTo(() => workingDayCalculator.GetWorkingDays(
                datePickedUp,
                A<DateTime>._,
                false,
                UKCompetentAuthority.England))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HandleAsync_HandlesInvalidPageNumber()
        {
            // Arrange
            var internalUser = A.Fake<InternalUser>();
            A.CallTo(() => internalUser.CompetentAuthority).Returns(UKCompetentAuthority.England);
            A.CallTo(() => internalUserRepository.GetByUserId(userId)).Returns(internalUser);

            var worklistSummary = CreateExportWorklistSummary();
            var queryResult = new ExportWorklistQueryResult
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

            var request = new GetExportWorklist
            {
                PageNumber = 0  // Invalid page number
            };

            // Act
            var result = await handler.HandleAsync(request);

            // Assert
            Assert.Equal(1, result.PageNumber);
        }

        [Fact]
        public async Task HandleAsync_IncludesTransmittedDate()
        {
            // Arrange
            var internalUser = A.Fake<InternalUser>();
            A.CallTo(() => internalUser.CompetentAuthority).Returns(UKCompetentAuthority.England);
            A.CallTo(() => internalUserRepository.GetByUserId(userId)).Returns(internalUser);

            var transmittedDate = new DateTime(2024, 5, 15);
            var worklistSummary = CreateExportWorklistSummary(transmittedDate: transmittedDate);
            var queryResult = new ExportWorklistQueryResult
            {
                TotalCount = 1,
                PagedRows = new[] { worklistSummary }
            };

            A.CallTo(() => worklistRepository.GetByCompetentAuthority(
                A<UKCompetentAuthority>._,
                A<string>._,
                A<string>._,
                A<NotificationStatus[]>._,
                A<int>._,
                A<int>._)).Returns(queryResult);

            SetupMocks(worklistSummary);

            var request = new GetExportWorklist
            {
                PageNumber = 1
            };

            // Act
            var result = await handler.HandleAsync(request);

            // Assert
            Assert.Equal(transmittedDate, result.Results.First().TransmittedDate);
        }

        [Fact]
        public async Task HandleAsync_IncludesLastCommentData()
        {
            // Arrange
            var internalUser = A.Fake<InternalUser>();
            A.CallTo(() => internalUser.CompetentAuthority).Returns(UKCompetentAuthority.England);
            A.CallTo(() => internalUserRepository.GetByUserId(userId)).Returns(internalUser);

            var lastCommentDate = new DateTimeOffset(2024, 6, 1, 10, 30, 0, TimeSpan.Zero);
            var lastComment = "Test comment content";
            var worklistSummary = CreateExportWorklistSummary(lastCommentDate: lastCommentDate, lastComment: lastComment);
            var queryResult = new ExportWorklistQueryResult
            {
                TotalCount = 1,
                PagedRows = new[] { worklistSummary }
            };

            A.CallTo(() => worklistRepository.GetByCompetentAuthority(
                A<UKCompetentAuthority>._,
                A<string>._,
                A<string>._,
                A<NotificationStatus[]>._,
                A<int>._,
                A<int>._)).Returns(queryResult);

            SetupMocks(worklistSummary);

            var request = new GetExportWorklist
            {
                PageNumber = 1
            };

            // Act
            var result = await handler.HandleAsync(request);

            // Assert
            Assert.Equal(lastCommentDate, result.Results.First().LastCommentDate);
            Assert.Equal(lastComment, result.Results.First().LastComment);
        }

        private ExportWorklistSummary CreateExportWorklistSummary(
            DateTime? datePickedUp = null,
            DateTime? transmittedDate = null,
            DateTimeOffset? lastCommentDate = null,
            string lastComment = null)
        {
            return ExportWorklistSummary.Load(
                notificationId,
                "GB 0001 000001",
                "Test Notifier",
                "Test Officer",
                datePickedUp,
                transmittedDate,
                new DateTime(2024, 1, 15),
                null,
                null,
                NotificationStatus.InAssessment,
                null,
                null,
                lastCommentDate ?? new DateTimeOffset(2024, 1, 10, 0, 0, 0, TimeSpan.Zero),
                "Approved",
                lastComment ?? "Test comment");
        }

        private void SetupMocks(ExportWorklistSummary summary)
        {
            var assessment = A.Fake<NotificationAssessment>();
            var notification = A.Fake<NotificationApplication>();
            A.CallTo(() => notification.CompetentAuthority).Returns(UKCompetentAuthority.England);

            A.CallTo(() => notificationAssessmentRepository.GetByNotificationId(summary.NotificationId))
                .Returns(assessment);
            A.CallTo(() => notificationApplicationRepository.GetById(summary.NotificationId))
                .Returns(notification);
            A.CallTo(() => decisionRequiredByCalculator.GetDecisionRequiredByDate(notification, assessment))
                .Returns((DateTime?)null);
        }
    }
}