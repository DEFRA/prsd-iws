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
                from importer in context.Importers.Where(e => e.NotificationId == application.Id).DefaultIfEmpty()
                from producerCollection in context.Producers.Where(e => e.NotificationId == application.Id).DefaultIfEmpty()
                from assessment in context.NotificationAssessments
                where assessment.NotificationApplicationId == application.Id
                orderby application.NotificationNumber
                select new
                {
                    Notification = application,
                    Exporter = exporter,
                    Importer = importer,
                    Assessment = assessment,
                    ProducerCollection = producerCollection
                };

            return
                (await query.ToListAsync())
                    .Select(n => new NotificationApplicationSummaryData
                    {
                        Id = n.Notification.Id,
                        NotificationNumber = n.Notification.NotificationNumber,
                        StatusDate =
                            n.Assessment.StatusChanges.OrderByDescending(p => p.ChangeDate).FirstOrDefault() == null
                                ? n.Notification.CreatedDate.UtcDateTime
                                : n.Assessment.StatusChanges.OrderByDescending(p => p.ChangeDate)
                                    .FirstOrDefault()
                                    .ChangeDate.UtcDateTime,
                        Status = n.Assessment.Status,
                        Exporter = n.Exporter == null ? null : n.Exporter.Business.Name,
                        Importer = n.Importer == null ? null : n.Importer.Business.Name,
                        Producer = n.ProducerCollection.Producers.Where(p => p.IsSiteOfExport).Select(p => p.Business.Name).SingleOrDefault() ?? string.Empty
                    }).ToList();
        }
    }
}