namespace EA.Iws.RequestHandlers.MeansOfTransport
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.MeansOfTransport;

    internal class SetMeansOfTransportForNotificationHandler : IRequestHandler<SetMeansOfTransportForNotification, Guid>
    {
        private readonly IwsContext context;

        public SetMeansOfTransportForNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetMeansOfTransportForNotification message)
        {
            var notification = await context.GetNotificationApplication(message.Id);

            notification.SetMeansOfTransport(message.MeansOfTransport);

            await context.SaveChangesAsync();

            return notification.Id;
        }
    }
}
