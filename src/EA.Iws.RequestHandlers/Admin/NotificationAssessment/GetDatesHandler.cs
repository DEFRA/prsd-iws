namespace EA.Iws.RequestHandlers.Admin.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using DataAccess;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;

    internal class GetDatesHandler : IRequestHandler<GetDates, NotificationDatesData>
    {
        private readonly IwsContext context;
        private readonly IMapWithParameter<NotificationDates, Guid, NotificationDatesData> mapper;
        private readonly DecisionRequiredBy decisionRequiredBy;
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public GetDatesHandler(IwsContext context, 
            IMapWithParameter<NotificationDates, Guid, NotificationDatesData> mapper,
            DecisionRequiredBy decisionRequiredBy,
            INotificationAssessmentRepository notificationAssessmentRepository,
            INotificationApplicationRepository notificationApplicationRepository)
        {
            this.context = context;
            this.mapper = mapper;
            this.decisionRequiredBy = decisionRequiredBy;
            this.notificationAssessmentRepository = notificationAssessmentRepository;
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<NotificationDatesData> HandleAsync(GetDates message)
        {
            var assessment = await notificationAssessmentRepository.GetByNotificationId(message.NotificationId);
            var notification = await notificationApplicationRepository.GetById(message.NotificationId);

            var datesData = mapper.Map(assessment.Dates, message.NotificationId);

            datesData.DecisionRequiredDate = decisionRequiredBy.GetDecisionRequiredByDate(notification, assessment);

            return datesData;
        }
    }
}