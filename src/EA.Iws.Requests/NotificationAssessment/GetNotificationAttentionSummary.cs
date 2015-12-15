namespace EA.Iws.Requests.NotificationAssessment
{
    using System.Collections.Generic;
    using Core.NotificationAssessment;
    using Prsd.Core.Mediator;

    public class GetNotificationAttentionSummary : IRequest<IEnumerable<NotificationAttentionSummaryTableData>>
    {
    }
}