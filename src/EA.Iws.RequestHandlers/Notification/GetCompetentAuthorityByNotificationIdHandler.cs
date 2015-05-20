namespace EA.Iws.RequestHandlers.Notification
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    public class GetCompetentAuthorityByNotificationIdHandler : IRequestHandler<GetCompetentAuthorityByNotificationId, CompetentAuthorityData>
    {
        private readonly IwsContext db;

        public GetCompetentAuthorityByNotificationIdHandler(IwsContext db)
        {
            this.db = db;
        }

        public async Task<CompetentAuthorityData> HandleAsync(GetCompetentAuthorityByNotificationId message)
        {
            return await db.NotificationApplications.Where(n => n.Id == message.NotificationId).Select(n =>
                new CompetentAuthorityData
                {
                    NotificationId = message.NotificationId,
                    CompetentAuthority = n.CompetentAuthority.Value,
                    NotificationNumber = n.NotificationNumber
                }).SingleAsync();
        }
    }
}
