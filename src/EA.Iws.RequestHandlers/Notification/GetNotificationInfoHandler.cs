namespace EA.Iws.RequestHandlers.Notification
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetNotificationInfoHandler : IRequestHandler<GetNotificationInfo, CompetentAuthorityData>
    {
        private readonly IwsContext db;

        public GetNotificationInfoHandler(IwsContext db)
        {
            this.db = db;
        }

        public async Task<CompetentAuthorityData> HandleAsync(GetNotificationInfo message)
        {
            return await db.NotificationApplications.Where(n => n.Id == message.NotificationId).Select(n =>
                new CompetentAuthorityData
                {
                    NotificationId = message.NotificationId,
                    CompetentAuthority = (CompetentAuthority)n.CompetentAuthority.Value,
                    NotificationNumber = n.NotificationNumber
                }).SingleAsync();
        }
    }
}
