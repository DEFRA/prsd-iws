namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using Core.ImportNotificationAssessment;
    using Prsd.Core.Mediator;

    public class GetKeyDates : IRequest<KeyDatesData>
    {
        public Guid ImportNotificationId { get; private set; }

        public GetKeyDates(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }
    }
}
