namespace EA.Iws.RequestHandlers.Facilities
{
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Facilities;

    internal class DeleteFacilityForNotificationHandler : IRequestHandler<DeleteFacilityForNotification, bool>
    {
        private readonly IwsContext context;

        public DeleteFacilityForNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(DeleteFacilityForNotification query)
        {
            var notification = await context.GetNotificationApplication(query.NotificationId);
            notification.RemoveFacility(query.FacilityId);
            await context.SaveChangesAsync();
            return true;
        }
    }
}