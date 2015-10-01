namespace EA.Iws.RequestHandlers.Mappings.NotificationAssessment
{
    using System.Linq;
    using Core.NotificationAssessment;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mapper;

    internal class NotificationAssessmentDecisionMap : IMap<NotificationAssessment, NotificationAssessmentDecisionData>
    {
        public NotificationAssessmentDecisionData Map(NotificationAssessment source)
        {
            return new NotificationAssessmentDecisionData
            {
                NotificationId = source.NotificationApplicationId,
                Status = source.Status,
                StatusHistory = source.StatusChanges.Select(sc => sc.Status).ToArray(),
                AvailableDecisions = source.GetAvailableDecisions()
            };
        }
    }
}
