namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using DataAccess;

    internal class GetNotificationAssessmentSummaryInformationHandler : IRequestHandler<GetNotificationAssessmentSummaryInformation, NotificationAssessmentSummaryInformationData>
    {
        private readonly IwsContext context;

        public GetNotificationAssessmentSummaryInformationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<NotificationAssessmentSummaryInformationData> HandleAsync(GetNotificationAssessmentSummaryInformation message)
        {
            var data = await context.NotificationApplications.Join(
                context.NotificationAssessments,
                nap => nap.Id,
                nas => nas.NotificationApplicationId,
                (application, assessment) => new 
                {
                    Id = application.Id,
                    Number = application.NotificationNumber,
                    CompetentAuthority = application.CompetentAuthority,
                    Status = assessment.Status
                }).SingleAsync(o => o.Id == message.Id);

            return new NotificationAssessmentSummaryInformationData
            {
                Id = data.Id,
                CompetentAuthority = data.CompetentAuthority.AsCompetentAuthority(),
                Number = data.Number,
                Status = data.Status
            };
        }
    }
}
