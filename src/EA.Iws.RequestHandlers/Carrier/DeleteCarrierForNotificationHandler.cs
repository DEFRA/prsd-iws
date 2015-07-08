namespace EA.Iws.RequestHandlers.Carrier
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Carriers;

    internal class DeleteCarrierForNotificationHandler : IRequestHandler<DeleteCarrierForNotification, bool>
    {
        private readonly IwsContext context;

        public DeleteCarrierForNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(DeleteCarrierForNotification query)
        {
            var notification = await context.NotificationApplications.Where(n => n.Id == query.NotificationId).SingleAsync();
            notification.RemoveCarrier(query.CarrierId);
            await context.SaveChangesAsync();
            return true;
        }
    }
}