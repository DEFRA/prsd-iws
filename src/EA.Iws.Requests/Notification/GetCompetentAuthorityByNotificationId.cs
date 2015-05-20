namespace EA.Iws.Requests.Notification
{
    using System;
    using Prsd.Core.Mediator;

    public class GetCompetentAuthorityByNotificationId : IRequest<CompetentAuthorityData>
    {
        public Guid NotificationId { get; set; }

        public GetCompetentAuthorityByNotificationId(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
