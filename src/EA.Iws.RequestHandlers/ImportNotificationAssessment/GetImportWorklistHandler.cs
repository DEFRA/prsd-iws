namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.ImportNotificationAssessment;
    using Domain;
    using Domain.ImportNotification;
    using Domain.ImportNotificationAssessment;
    using Domain.ImportNotificationAssessment.Decision;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    internal class GetImportWorklistHandler : IRequestHandler<GetImportWorklist, ImportWorklistResult>
    {
        private const int PageSize = 25;

        private readonly IImportWorklistRepository worklistRepository;
        private readonly IImportNotificationAssessmentRepository importNotificationAssessmentRepository;
        private readonly IImportNotificationRepository importNotificationRepository;
        private readonly IWorkingDayCalculator workingDayCalculator;
        private readonly IInternalUserRepository internalUserRepository;
        private readonly IUserContext userContext;
        private readonly DecisionRequiredBy decisionRequiredByCalculator;
        private readonly DaysRemainingCalculator daysRemainingCalculator;

        public GetImportWorklistHandler(
            IImportWorklistRepository worklistRepository,
            IImportNotificationAssessmentRepository importNotificationAssessmentRepository,
            IImportNotificationRepository importNotificationRepository,
            IWorkingDayCalculator workingDayCalculator,
            IInternalUserRepository internalUserRepository,
            IUserContext userContext,
            DecisionRequiredBy decisionRequiredByCalculator,
            DaysRemainingCalculator daysRemainingCalculator)
        {
            this.worklistRepository = worklistRepository;
            this.importNotificationAssessmentRepository = importNotificationAssessmentRepository;
            this.importNotificationRepository = importNotificationRepository;
            this.workingDayCalculator = workingDayCalculator;
            this.internalUserRepository = internalUserRepository;
            this.userContext = userContext;
            this.decisionRequiredByCalculator = decisionRequiredByCalculator;
            this.daysRemainingCalculator = daysRemainingCalculator;
        }

        public async Task<ImportWorklistResult> HandleAsync(GetImportWorklist message)
        {
            var internalUser = await internalUserRepository.GetByUserId(userContext.UserId);

            var pageNumber = message.PageNumber < 1 ? 1 : message.PageNumber;

            var queryResult = await worklistRepository.GetByCompetentAuthority(
                internalUser.CompetentAuthority,
                message.NotificationNumber,
                message.Officer,
                message.Statuses,
                pageNumber,
                PageSize);

            var tableRows = new List<ImportWorklistTableData>();
            foreach (var summary in queryResult.PagedRows)
            {
                tableRows.Add(await BuildTableData(summary));
            }

            return new ImportWorklistResult
            {
                Results = tableRows,
                TotalCount = queryResult.TotalCount,
                PageNumber = pageNumber,
                PageSize = PageSize
            };
        }

        private async Task<ImportWorklistTableData> BuildTableData(ImportWorklistSummary summary)
        {
            var notification = await importNotificationRepository.Get(summary.NotificationId);

            // Calculate decision required date if not stored
            var decisionRequiredDate = summary.DecisionRequiredDate;
            if (!decisionRequiredDate.HasValue && summary.AcknowledgedDate.HasValue)
            {
                var assessment = await importNotificationAssessmentRepository.GetByNotification(summary.NotificationId);
                if (assessment != null)
                {
                    decisionRequiredDate = await decisionRequiredByCalculator.GetDecisionRequiredByDate(assessment);
                }
            }

            // Calculate days remaining using the same logic as "Notifications requiring attention"
            // Returns either number of days or "Overdue"
            string daysRemaining = null;
            if (decisionRequiredDate.HasValue)
            {
                daysRemaining = daysRemainingCalculator.Calculate(decisionRequiredDate.Value);
            }

            // Calculate working days in assessment from date picked up by officer to today
            int? workingDaysInAssessment = null;
            if (summary.DatePickedUpByOfficer.HasValue)
            {
                workingDaysInAssessment = workingDayCalculator.GetWorkingDays(
                    summary.DatePickedUpByOfficer.Value,
                    SystemTime.UtcNow,
                    false,
                    notification.CompetentAuthority);
            }

            return new ImportWorklistTableData
            {
                NotificationId = summary.NotificationId,
                NotificationNumber = summary.NotificationNumber,
                Exporter = summary.Exporter,
                Officer = summary.Officer,
                DatePickedUpByOfficer = summary.DatePickedUpByOfficer,
                NotificationReceivedDate = summary.NotificationReceivedDate,
                AcknowledgedDate = summary.AcknowledgedDate,
                ConsentedDate = summary.ConsentedDate,
                DecisionRequiredDate = decisionRequiredDate,
                Status = summary.Status,
                WorkingDaysInAssessment = workingDaysInAssessment,
                DaysRemaining = daysRemaining,
                LastCommentDate = summary.LastCommentDate,
                FinancialGuaranteeStatus = summary.FinancialGuaranteeStatus,
                FinancialGuaranteeStatusDescription = summary.FinancialGuaranteeStatusDescription,
                LastAction = summary.LastAction,
                LastComment = summary.LastComment,
                LastCommentUser = summary.LastCommentUser
            };
        }
    }
}