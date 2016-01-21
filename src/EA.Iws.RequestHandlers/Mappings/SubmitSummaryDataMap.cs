namespace EA.Iws.RequestHandlers.Mappings
{
    using System.Linq;
    using Core.Notification;
    using Domain.NotificationApplication;
    using EA.Iws.DataAccess;
    using Prsd.Core.Mapper;

    internal class SubmitSummaryDataMap : IMap<NotificationApplication, SubmitSummaryData>
    {
        private readonly IwsContext context;

        public SubmitSummaryDataMap(IwsContext context)
        {
            this.context = context;
        }

        public SubmitSummaryData Map(NotificationApplication notification)
        {
            var status = context.NotificationAssessments.Single(na => na.NotificationApplicationId == notification.Id).Status;

            return new SubmitSummaryData
            {
                NotificationId = notification.Id,
                CompetentAuthority = notification.CompetentAuthority.AsCompetentAuthority(),
                CreatedDate = notification.CreatedDate.Date,
                NotificationNumber = notification.NotificationNumber,
                Status = status,
            };
        }
    }
}
