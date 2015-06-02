namespace EA.Iws.RequestHandlers.Notification
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    public class SetPreconsentedRecoveryFacilityHandler : IRequestHandler<SetPreconsentedRecoveryFacility, string>
    {
        private readonly IwsContext context;

        public SetPreconsentedRecoveryFacilityHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<string> HandleAsync(SetPreconsentedRecoveryFacility query)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == query.NotificationId);
            notification.SetPreconsentedRecoveryFacility(query.IsPreconsentedRecoveryFacility);

            await context.SaveChangesAsync();

            return notification.NotificationNumber;
        }
    }
}