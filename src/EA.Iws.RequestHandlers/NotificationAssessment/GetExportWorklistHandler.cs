namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using Domain;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class GetExportWorklistHandler : IRequestHandler<GetExportWorklist, ExportWorklistResult>
    {
        private const int PageSize = 25;

        private readonly IExportWorklistRepository worklistRepository;
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly DecisionRequiredBy decisionRequiredByCalculator;
        private readonly DaysRemainingCalculator daysRemainingCalculator;
        private readonly IWorkingDayCalculator workingDayCalculator;
        private readonly IInternalUserRepository internalUserRepository;
        private readonly IUserContext userContext;

        public GetExportWorklistHandler(
            IExportWorklistRepository worklistRepository,
            INotificationAssessmentRepository notificationAssessmentRepository,
            INotificationApplicationRepository notificationApplicationRepository,
            DecisionRequiredBy decisionRequiredByCalculator,
            DaysRemainingCalculator daysRemainingCalculator,
            IWorkingDayCalculator workingDayCalculator,
            IInternalUserRepository internalUserRepository,
            IUserContext userContext)
        {
            this.worklistRepository = worklistRepository;
            this.notificationAssessmentRepository = notificationAssessmentRepository;
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.decisionRequiredByCalculator = decisionRequiredByCalculator;
            this.daysRemainingCalculator = daysRemainingCalculator;
            this.workingDayCalculator = workingDayCalculator;
            this.internalUserRepository = internalUserRepository;
            this.userContext = userContext;
        }

        public async Task<ExportWorklistResult> HandleAsync(GetExportWorklist message)
        {
            var internalUser = await internalUserRepository.GetByUserId(userContext.UserId);

            var pageNumber = message.PageNumber < 1 ? 1 : message.PageNumber;

            var queryResult = await worklistRepository.GetByCompetentAuthority(
                internalUser.CompetentAuthority,
                message.NotificationNumber,
                message.Officer,
                message.Status,
                pageNumber,
                PageSize);

            var tableRows = new List<ExportWorklistTableData>();
            foreach (var summary in queryResult.PagedRows)
            {
                tableRows.Add(await BuildTableData(summary));
            }

            return new ExportWorklistResult
            {
                Results = tableRows,
                TotalCount = queryResult.TotalCount,
                PageNumber = pageNumber,
                PageSize = PageSize
            };
        }

        private async Task<ExportWorklistTableData> BuildTableData(ExportWorklistSummary summary)
        {
            var assessment = await notificationAssessmentRepository.GetByNotificationId(summary.NotificationId);
            var notification = await notificationApplicationRepository.GetById(summary.NotificationId);

            var decisionRequiredBy = await decisionRequiredByCalculator.GetDecisionRequiredByDate(notification, assessment);

            var daysRemaining = decisionRequiredBy.HasValue
                ? daysRemainingCalculator.Calculate(decisionRequiredBy.Value)
                : null;

            int? workingDaysInAssessment = null;
            if (summary.DatePickedUpByOfficer.HasValue)
            {
                workingDaysInAssessment = workingDayCalculator.GetWorkingDays(
                    summary.DatePickedUpByOfficer.Value,
                    SystemTime.UtcNow,
                    false,
                    notification.CompetentAuthority);
            }

            return new ExportWorklistTableData
            {
                NotificationId = summary.NotificationId,
                NotificationNumber = summary.NotificationNumber,
                Notifier = summary.Notifier,
                Officer = summary.Officer,
                DatePickedUpByOfficer = summary.DatePickedUpByOfficer,
                WorkingDaysInAssessment = workingDaysInAssessment,
                TransmittedDate = summary.TransmittedDate,
                AcknowledgedDate = summary.AcknowledgedDate,
                DecisionRequiredDate = decisionRequiredBy,
                DaysRemaining = daysRemaining,
                ConsentedDate = summary.ConsentedDate,
                LastActionDate = summary.LastAuditDate,
                LastActionType = summary.LastAuditType,
                LastCommentDate = summary.LastCommentDate,
                Status = summary.Status
            };
        }
    }
}