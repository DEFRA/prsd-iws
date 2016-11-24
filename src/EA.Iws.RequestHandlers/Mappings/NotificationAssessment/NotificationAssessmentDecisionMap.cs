namespace EA.Iws.RequestHandlers.Mappings.NotificationAssessment
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mapper;

    internal class NotificationAssessmentDecisionMap : IMap<NotificationAssessment, NotificationAssessmentDecisionData>
    {
        private readonly IFacilityRepository facilityRepository;

        public NotificationAssessmentDecisionMap(IFacilityRepository repo)
        {
            this.facilityRepository = repo;
        }

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
                AvailableDecisions = source.GetAvailableDecisions(),
                AcknowledgedOnDate = source.Dates.AcknowledgedDate.GetValueOrDefault(),
                IsPreconsented = Task.Run(() => facilityRepository.GetByNotificationId(source.NotificationApplicationId)).Result.AllFacilitiesPreconsented.GetValueOrDefault(),
                ConsentedDate = source.Dates.ConsentedDate,
                NotificationReceivedDate = source.Dates.NotificationReceivedDate
            };

            return data;
        }
    }
}
