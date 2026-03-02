namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using EA.Iws.Core.ImportNotificationAssessment;
    using EA.Iws.Domain.ImportNotification;
    using EA.Iws.Domain.ImportNotificationAssessment;
    using EA.Iws.Requests.ImportNotificationAssessment;
    using EA.Prsd.Core.Mediator;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class GetImportNotificationProhibitionStatusChangesHandler : IRequestHandler<GetImportNotificationProhibitionStatusChanges, List<ImportNotificationStatusChangeData>>
    {
        private readonly IImportNotificationAssessmentRepository assessmentRepository;

        public GetImportNotificationProhibitionStatusChangesHandler(IImportNotificationAssessmentRepository assessmentRepository)
        {
            this.assessmentRepository = assessmentRepository;
        }

        public async Task<List<ImportNotificationStatusChangeData>> HandleAsync(GetImportNotificationProhibitionStatusChanges message)
        {
            List<ImportNotificationStatusChangeData> result = await assessmentRepository.GetUnderProhibitionHistory(message.NotificationId);

            return result;
        }
    }
}
