namespace EA.Iws.RequestHandlers.Admin.NotificationAssessment
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using DataAccess;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;

    internal class GetDatesHandler : IRequestHandler<GetDates, NotificationDatesData>
    {
        private readonly IwsContext context;
        private readonly IMapWithParameter<NotificationDates, Guid, NotificationDatesData> mapper;

        public GetDatesHandler(IwsContext context, IMapWithParameter<NotificationDates, Guid, NotificationDatesData> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<NotificationDatesData> HandleAsync(GetDates message)
        {
            var dates = await context.NotificationAssessments
                .Where(p => p.NotificationApplicationId == message.NotificationId)
                .Select(p => p.Dates)
                .SingleAsync();

            return mapper.Map(dates, message.NotificationId);
        }
    }
}