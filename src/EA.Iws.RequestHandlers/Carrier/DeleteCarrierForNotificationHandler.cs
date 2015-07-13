namespace EA.Iws.RequestHandlers.Carrier
{
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
            var notification = await context.GetNotificationApplication(query.NotificationId);
            notification.RemoveCarrier(query.CarrierId);
            await context.SaveChangesAsync();
            return true;
        }
    }
}