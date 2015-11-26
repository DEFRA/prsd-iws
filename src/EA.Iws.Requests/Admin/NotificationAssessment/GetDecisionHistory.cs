namespace EA.Iws.Requests.Admin.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using Core.NotificationAssessment;
    using Prsd.Core.Mediator;

    public class GetDecisionHistory : IRequest<IList<NotificationAssessmentDecision>>
    {
        public GetDecisionHistory(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}
