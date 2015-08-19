namespace EA.Iws.Requests.Admin.NotificationAssessment
{
    using System;
    using Core.NotificationAssessment;
    using Prsd.Core.Mediator;

    public class GetDates : IRequest<NotificationDatesData>
    {
        public GetDates(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}