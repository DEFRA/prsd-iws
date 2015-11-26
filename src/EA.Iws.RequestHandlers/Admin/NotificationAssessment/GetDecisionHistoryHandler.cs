namespace EA.Iws.RequestHandlers.Admin.NotificationAssessment
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;

    internal class GetDecisionHistoryHandler : IRequestHandler<GetDecisionHistory, IList<NotificationAssessmentDecision>>
    {
        private readonly INotificationAssessmentDecisionRepository decisionRepository;

        public GetDecisionHistoryHandler(INotificationAssessmentDecisionRepository decisionRepository)
        {
            this.decisionRepository = decisionRepository;
        }
         
        public async Task<IList<NotificationAssessmentDecision>> HandleAsync(GetDecisionHistory message)
        {
            return await decisionRepository.GetByNotificationId(message.NotificationId);
        }
    }
}
