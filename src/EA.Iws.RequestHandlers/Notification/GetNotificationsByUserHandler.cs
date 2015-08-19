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

        public async Task<IList<NotificationApplicationSummaryData>> HandleAsync(GetNotificationsByUser query)
        {
            return
                (await
                    context.NotificationApplications.Where(na => na.UserId == userContext.UserId)
                        .Join(context.NotificationAssessments, application => application.Id,
                            assessment => assessment.NotificationApplicationId,
                            (application, assessment) => new 
                            { 
                                Notification = application, 
                                Assessment = assessment
                            })
                            .OrderBy(p => p.Notification.NotificationNumber)
                            .ToListAsync())
                        .Select(n => new NotificationApplicationSummaryData
                        {
                            Id = n.Notification.Id,
                            NotificationNumber = n.Notification.NotificationNumber,
                            StatusDate = n.Assessment.StatusChanges.OrderByDescending(p => p.ChangeDate).FirstOrDefault() == null 
                                        ? n.Notification.CreatedDate 
                                        : n.Assessment.StatusChanges.OrderByDescending(p => p.ChangeDate).FirstOrDefault().ChangeDate,
                            Status = n.Assessment.Status
                        }).ToList();
        }
    }
}