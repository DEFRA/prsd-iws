namespace EA.Iws.RequestHandlers
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Requests.Organisations;

    public class GetProducersByNotificationIdHandler : IRequestHandler<GetProducersByNotificationId, IList<ProducerData>>
    {
        private readonly IwsContext db;

        public GetProducersByNotificationIdHandler(IwsContext db)
        {
            this.db = db;
        }

        public async Task<IList<ProducerData>> HandleAsync(GetProducersByNotificationId message)
        {
            var result =
                await
                    db.NotificationApplications.Where(n => n.Id == message.NotificationId)
                        .Include("ProducersCollection").SingleAsync();

            return new List<ProducerData>();
        }
    }
}
