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
            var data = new NotificationAssessmentDecisionData
            {
                NotificationId = source.NotificationApplicationId,
                Status = source.Status,
                StatusHistory = source.StatusChanges.Select(sc => new NotificationAssessmentDecisionRecord
                {
                    Date = sc.ChangeDate.UtcDateTime,
                    Status = sc.Status
                }).ToArray(),
                AvailableDecisions = source.GetAvailableDecisions()
            };

            return data;
        }
    }
}
