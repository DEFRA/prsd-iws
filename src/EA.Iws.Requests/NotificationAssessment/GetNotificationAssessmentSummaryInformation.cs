namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.NotificationAssessment;
    using Prsd.Core.Mediator;

    public class GetNotificationAssessmentSummaryInformation : IRequest<NotificationAssessmentSummaryInformationData>
    {
        public Guid Id { get; private set; }

        public GetNotificationAssessmentSummaryInformation(Guid id)
        {
            Id = id;
        }
    }
}
