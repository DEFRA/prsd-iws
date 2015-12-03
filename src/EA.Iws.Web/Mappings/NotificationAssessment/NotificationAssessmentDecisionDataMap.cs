namespace EA.Iws.Web.Mappings.NotificationAssessment
{
    using System.Linq;
    using Areas.AdminExportAssessment.ViewModels;
    using Areas.AdminExportAssessment.ViewModels.Decision;
    using Core.NotificationAssessment;
    using Prsd.Core.Mapper;

    public class NotificationAssessmentDecisionDataMap : IMap<NotificationAssessmentDecisionData, NotificationAssessmentDecisionViewModel>
    {
        public NotificationAssessmentDecisionViewModel Map(NotificationAssessmentDecisionData source)
        {
            return new NotificationAssessmentDecisionViewModel
            {
                NotificationId = source.NotificationId,
                Status = source.Status,
                PreviousDecisions = source.StatusHistory.Select(d => new DecisionRecordViewModel()).ToList(),
                DecisionTypes = source.AvailableDecisions.ToList()
            };
        }
    }
}