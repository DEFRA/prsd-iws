namespace EA.Iws.RequestHandlers.Notification
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetNotificationsByUserHandler :
        IRequestHandler<GetNotificationsByUser, IList<NotificationApplicationSummaryData>>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public GetNotificationsByUserHandler(IwsContext context, IUserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<IList<NotificationApplicationSummaryData>> HandleAsync(GetNotificationsByUser message)
        {
            var query = from application in context.NotificationApplications
                where application.UserId == userContext.UserId
                from exporter in context.Exporters.Where(e => e.NotificationId == application.Id).DefaultIfEmpty()
                from assessment in context.NotificationAssessments
                where assessment.NotificationApplicationId == application.Id
                orderby application.NotificationNumber
                select new
                {
                    Notification = application,
                    Exporter = exporter,
                    Assessment = assessment
                };

            return
                (await query.ToListAsync())
                    .Select(n => new NotificationApplicationSummaryData
                    {
                        Id = n.Notification.Id,
                        NotificationNumber = n.Notification.NotificationNumber,
                        StatusDate =
                            n.Assessment.StatusChanges.OrderByDescending(p => p.ChangeDate).FirstOrDefault() == null
                                ? n.Notification.CreatedDate
                                : n.Assessment.StatusChanges.OrderByDescending(p => p.ChangeDate)
                                    .FirstOrDefault()
                                    .ChangeDate,
                        Status = n.Assessment.Status,
                        Exporter = n.Exporter == null ? null : n.Exporter.Business.Name,
                        Importer = n.Notification.Importer == null ? null : n.Notification.Importer.Business.Name,
                        Producer = n.Notification.Producers.Where(p => p.IsSiteOfExport).Select(p => p.Business.Name).SingleOrDefault() ?? string.Empty
                    }).ToList();
        }
    }
}