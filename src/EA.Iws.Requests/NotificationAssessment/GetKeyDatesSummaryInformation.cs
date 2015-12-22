namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.NotificationAssessment;
    using Prsd.Core.Mediator;

    public class GetKeyDatesSummaryInformation : IRequest<KeyDatesSummaryData>
    {
        public Guid NotificationId { get; private set; }

        public GetKeyDatesSummaryInformation(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
