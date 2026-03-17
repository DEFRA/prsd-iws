namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using EA.Iws.Core.NotificationAssessment;
    using EA.Iws.DataAccess;
    using EA.Iws.Domain.NotificationAssessment;
    using EA.Iws.Requests.NotificationAssessment;
    using EA.Prsd.Core.Mediator;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class GetProhibitionStatusChangesHandler : IRequestHandler<GetProhibitionStatusChanges, List<NotificationStatusChangeData>>
    {
        private readonly INotificationAssessmentRepository assessmentRepository;

        public GetProhibitionStatusChangesHandler(INotificationAssessmentRepository assessmentRepository)
        {
            this.assessmentRepository = assessmentRepository;
        }

        public async Task<List<NotificationStatusChangeData>> HandleAsync(GetProhibitionStatusChanges message)
        {
            List<NotificationStatusChangeData> result = await assessmentRepository.GetUnderProhibitionHistory(message.NotificationId);

            return result;
        }
    }
}
