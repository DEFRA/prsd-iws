namespace EA.Iws.RequestHandlers.Submit
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Notification;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Submit;

    internal class GetSubmitSummaryInformationHandlerForNotificationHandler : IRequestHandler<GetSubmitSummaryInformationForNotification, SubmitSummaryData>
    {
        private readonly IwsContext context;

        public GetSubmitSummaryInformationHandlerForNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<SubmitSummaryData> HandleAsync(GetSubmitSummaryInformationForNotification message)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.Id);

            var competentAuthority = notification.CompetentAuthority.AsCompetentAuthority();
            
            return new SubmitSummaryData
            {
                CompetentAuthority = competentAuthority,
                CreatedDate = notification.CreatedDate,
                NotificationNumber = notification.NotificationNumber,
                Status = Status.New
            };
        }
    }
}
