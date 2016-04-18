namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using Domain;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class GetNotificationAttentionSummaryHandler :
        IRequestHandler<GetNotificationAttentionSummary, IEnumerable<NotificationAttentionSummaryTableData>>
    {
        private readonly DaysRemainingCalculator daysRemainingCalculator;
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;
        private readonly DecisionRequiredBy decisionRequiredByCalculator;
        private readonly IInternalUserRepository internalUserRepository;
        private readonly IUserContext userContext;
        private readonly INotificationAttentionSummaryRepository attentionSummaryRepository;

        public GetNotificationAttentionSummaryHandler(INotificationAttentionSummaryRepository attentionSummaryRepository,
            IUserContext userContext,
            IInternalUserRepository internalUserRepository,
            DecisionRequiredBy decisionRequiredByCalculator,
            INotificationAssessmentRepository notificationAssessmentRepository,
            INotificationApplicationRepository notificationApplicationRepository,
            DaysRemainingCalculator daysRemainingCalculator)
        {
            this.attentionSummaryRepository = attentionSummaryRepository;
            this.userContext = userContext;
            this.internalUserRepository = internalUserRepository;
            this.decisionRequiredByCalculator = decisionRequiredByCalculator;
            this.notificationAssessmentRepository = notificationAssessmentRepository;
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.daysRemainingCalculator = daysRemainingCalculator;
        }

        public async Task<IEnumerable<NotificationAttentionSummaryTableData>> HandleAsync(GetNotificationAttentionSummary message)
        {
            var internalUser = await internalUserRepository.GetByUserId(userContext.UserId);
            var summaryData = await attentionSummaryRepository.GetByCompetentAuthority(internalUser.CompetentAuthority);

            var result = new List<NotificationAttentionSummaryTableData>();
            foreach (var summary in summaryData)
            {
                var data = await GetTableDataFromSummary(summary);

                result.Add(data);
            }

            return result.OrderBy(x => x.DecisionRequiredDate).ToArray();
        }

        private async Task<NotificationAttentionSummaryTableData> GetTableDataFromSummary(NotificationAttentionSummary summary)
        {
            var assessment = await notificationAssessmentRepository.GetByNotificationId(summary.NotificationId);
            var notification = await notificationApplicationRepository.GetById(summary.NotificationId);

            var decisionRequiredBy = await decisionRequiredByCalculator.GetDecisionRequiredByDate(notification, assessment);
            var daysRemaining = daysRemainingCalculator.Calculate(decisionRequiredBy.Value);

            return new NotificationAttentionSummaryTableData
            {
                NotificationId = summary.NotificationId,
                NotificationNumber = summary.NotificationNumber,
                Officer = summary.Officer,
                AcknowledgedDate = summary.AcknowledgedDate,
                DecisionRequiredDate = decisionRequiredBy.Value,
                DaysRemaining = daysRemaining
            };
        }
    }
}