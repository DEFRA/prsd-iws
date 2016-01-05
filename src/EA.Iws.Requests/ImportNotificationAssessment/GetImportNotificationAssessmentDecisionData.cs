namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using Core.ImportNotificationAssessment;
    using Prsd.Core.Mediator;

    public class GetImportNotificationAssessmentDecisionData : IRequest<ImportNotificationAssessmentDecisionData>
    {
        public Guid ImportNotificationId { get; private set; }

        public GetImportNotificationAssessmentDecisionData(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }
    }
}
