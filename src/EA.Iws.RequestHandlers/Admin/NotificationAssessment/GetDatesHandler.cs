namespace EA.Iws.RequestHandlers.Admin.NotificationAssessment
{
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;

    internal class GetDatesHandler : IRequestHandler<GetDates, NotificationDatesData>
    {
        private readonly IMap<NotificationDatesSummary, NotificationDatesData> mapper;
        private readonly INotificationAssessmentDatesSummaryRepository datesSummaryRepository;

        public GetDatesHandler(INotificationAssessmentDatesSummaryRepository datesSummaryRepository,
            IMap<NotificationDatesSummary, NotificationDatesData> mapper)
        {
            this.datesSummaryRepository = datesSummaryRepository;
            this.mapper = mapper;
        }

        public async Task<NotificationDatesData> HandleAsync(GetDates message)
        {
            var datesSummary = await datesSummaryRepository.GetById(message.NotificationId);

            return mapper.Map(datesSummary);
        }
    }
}